using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using Microsoft.Kinect;
using Coding4Fun.Kinect.WinForm;
using AForge.Imaging;
using AForge.Imaging.Filters;
using AForge.Math.Geometry;
using AForge;
using AForge.Video;
using AForge.Video.DirectShow;
using Microsoft.Xna.Framework;


namespace XNA_Debug
{
    public partial class WebcamForm : Form
    {
        KinectSensor sensor;
        BackgroundWorker bw_kinect, bw_webcam;

        // zmienne do webcama
        private bool DeviceExist = false;
        private FilterInfoCollection videoDevices;
        private VideoCaptureDevice videoSource = null;

        // zmienne do wykrywania oczek
        CannyEdgeDetector ced;
        SimpleShapeChecker shapeChecker;
        AForge.Point poprzedni_srodek = new AForge.Point(0, 0);
        Vector2 K_srodek;
        SkeletonPoint skele_srodek;
        int il_oczek = 0; int licznik = 0;
        int srX, srY;
        bool lock_kostki = false;

        System.Drawing.Point point = new System.Drawing.Point(3,3);

        Vector2 w1 = new Vector2(1, 0);
        Vector2 w2 = new Vector2(0, 1);

        /// <summary>
        /// timer do aktualizacji wyswietlanych wspolrzednych srodka kostki oraz liczby oczek
        /// </summary>
        Timer timer;

        /// <summary>
        /// tablica do przechowywania szkieletow
        /// </summary>
        Skeleton[] skeletonData;

        // tablice punktow do kalibracji
        List<System.Drawing.Point> webc_points = new List<System.Drawing.Point>();
        List<System.Drawing.Point> kinect_points; // punkty pobrane z pictureboxa Kinecta
        List<SkeletonPoint> kinc_spoints = new List<SkeletonPoint>(); // odpowiadajace im SkeletonPoints
        bool kalibracja = false;

        /// <summary>
        /// wersory ukladu wspolrzednych 3D Kinecta
        /// </summary>
        List<Vector3> kinc_wersory;
        /// <summary>
        /// baza w przestrzeni 2D webcamu
        /// </summary>
        List<Vector2> webc_wersory;

        //pedzle i olowki
        Pen redPen = new Pen(System.Drawing.Color.Red, 2);
        Brush yellowBrush = new SolidBrush(System.Drawing.Color.GreenYellow);
        Pen trackedBonePen = new Pen(System.Drawing.Color.Green, 6);
        Pen infferedBonePen = new Pen(System.Drawing.Color.GreenYellow, 1);
        Brush blueBrush = new SolidBrush(System.Drawing.Color.Blue);
        Brush pinkBrush = new SolidBrush(System.Drawing.Color.HotPink);

        public WebcamForm()
        {

            timer = new Timer();
            timer.Interval = 50;
            timer.Tick += new EventHandler(timer_Tick);
            timer.Enabled = true;

            bw_kinect = new BackgroundWorker();
            bw_kinect.WorkerSupportsCancellation = true;
            bw_kinect.DoWork += ProcessKinectFrame;

            bw_webcam = new BackgroundWorker();
            bw_webcam.WorkerSupportsCancellation = true;
            bw_webcam.DoWork += ProcessWebcamFrame;

            InitializeComponent();

            // sprawdzamy czy Kinect zostal podlaczony do komputera
            if (!KinectSensor.KinectSensors.Count.Equals(0))
            {
                // jesli tak, to przygotowujemy go do dzialania i uruchamiamy odbieranie strumieni z obu kamer
                label3.Text = "Kinect podłączony";
                sensor = KinectSensor.KinectSensors[0];
                sensor.SkeletonStream.Enable();
                skeletonData = new Skeleton[sensor.SkeletonStream.FrameSkeletonArrayLength];

                sensor.SkeletonFrameReady += new EventHandler<SkeletonFrameReadyEventArgs>(SkeletonFrameReady);

                sensor.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);
                sensor.ColorFrameReady += FrameReady;
                sensor.DepthFrameReady += DepthFrameReady;
                sensor.Start();
            }
            this.Shown += DebugForm_Load;

            getCamList();

        }


        private void DebugForm_Load(object sender, EventArgs e)
        {
            // ustawiamy na którym ekranie ma wyświetlać się okno i w jakim miejscu
            var ekran_dom = Screen.FromControl(this);
            var ekran_deb = Screen.AllScreens.FirstOrDefault(s => s != ekran_dom) ?? ekran_dom;
            this.Top = ekran_deb.WorkingArea.Top + 50;
            this.Left = ekran_deb.WorkingArea.Left + 10;

            K_srodek = new Vector2(0, 0);
        }

        //--- metody do przekazywania odpowiednich wartosci klasie Game:
        //   sensor
        public KinectSensor przekazSensor() {
            if(sensor.Status == KinectStatus.Connected)
                return sensor;
            else
                return null;
        }

        //  wykryta ilosc oczek
        public int przekazIloscOczek()
        {
            return il_oczek;
        }

        //   wyestymowany srodek kostki
        public Vector2 przekazSrodek() {
            return K_srodek;
        }

        //   srodek kostki we wspolrzednych szkieletowych Kinecta
        public SkeletonPoint przekazSkeleSrodek() {
            return skele_srodek;
        }
        // ------------------------------------------------------------

        // obsluga timera do odswiezania wyswietlanego srodka kosci i wykrytej ilosci oczek
        private void timer_Tick(object sender, EventArgs e)
        {
            srlabel.Text = "(" + srX.ToString() + ";" + srY.ToString() + ")";
            label4.Text = il_oczek.ToString();
        }

        // tworzenie listy kamer podlaczonych do komputera
        private void getCamList()
        {
            try
            {
                videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
                webcamSet.Items.Clear();

                if (videoDevices.Count == 0)
                    throw new ApplicationException();

                DeviceExist = true;

                foreach (FilterInfo device in videoDevices)
                {
                    webcamSet.Items.Add(device.Name);
                }
            }
            catch
            {
                DeviceExist = false;
                webcamSet.Items.Add("Nie wykryto webcama");
            }
        }

        // rozpoczecie pobierania streamu z wybranej kamery
        private void startCam()
        {

            if (DeviceExist)
            {
                videoSource = new VideoCaptureDevice(videoDevices[webcamSet.SelectedIndex].MonikerString);
                videoSource.NewFrame += new NewFrameEventHandler(CamFrameReady);
                CloseVideoSource();
                videoSource.DesiredFrameSize = new Size(640, 480);
                videoSource.ProvideSnapshots = false;
                videoSource.Start();
                timer1.Enabled = true;
            }
            else
            {
                Console.WriteLine("Error: nie wybrano zadnego webcama");
            }
        }

        // reakcja na zmiane kamery
        private void webcamSet_SelectedIndexChanged(object sender, EventArgs e)
        {
            CloseVideoSource();
            startCam();
        }

        // handler do ramek obrazu z wybranej kamery
        private void CamFrameReady(object sender, NewFrameEventArgs e)
        {
            // jesli background worker jest gotowy, przekazujemy do niego ramke obrazu
            if (!bw_webcam.IsBusy)
            {
                Bitmap img = (Bitmap)e.Frame.Clone();
                bw_webcam.RunWorkerAsync(img);
            }

            e.Frame.Dispose();
        }

        // funkcja konczaca pobieranie strumienia obrazu z kamery
        private void CloseVideoSource()
        {
            if (!(videoSource == null))
            {
                if (videoSource.IsRunning)
                {
                    videoSource.Stop();
                    videoSource.WaitForStop();
                    videoSource = null;
                }
            }
        }

        /// <summary>
        /// przetwarzanie pojedynczej ramki z kamery RGB Kinecta
        /// </summary>
        void ProcessKinectFrame(object sender, DoWorkEventArgs e)
        {
            Bitmap source = e.Argument as Bitmap;
            // odbicie lustrzane by orientacja byla bardziej naturalna
            Bitmap mirror_source = new Bitmap(source); 

            // tworzymy obiekt Graphics na ktorym bedziemy mogli rysowac
            Graphics g = Graphics.FromImage(mirror_source);

            // rysujemy wykryte szkielety jesli ta opcja zostala zaznaczona
            if (szkieletToolStripMenuItem.Checked)
            {
                //TrackClosestSkeleton();
                DrawSkeletons(g);
            }
            
            // rysujemy punkty kalibracji
            if(kinc_spoints.Count == 3)
                foreach(SkeletonPoint s in kinc_spoints) {
                    g.FillRectangle(blueBrush, SkeletonPointToScreen(s).X - 3, SkeletonPointToScreen(s).Y - 3, 6, 6);
                }

            // jesli uklad nie jest akurat w trakcie kalibracji, rysujemy estymowany srodek kostki
            if(!kalibracja)
                g.FillRectangle(new SolidBrush(System.Drawing.Color.Red), K_srodek.X - 3, K_srodek.Y - 3, 6, 6);
            
            g.Dispose();

            // przesylamy obraz do odpowiedniego PictureBox'a
            DrawOnKinectPixtureBox(mirror_source);
        }

        /// <summary>
        /// przetwarzanie pojedynczej ramki z webcama
        /// </summary>
        void ProcessWebcamFrame(object sender, DoWorkEventArgs e)
        {
            // lokalny licznik wykrytych oczek
            int ilocz = 0;

            // parametry metody Canny i jej inicjalizacja
            byte lowT, highT;
            lowT = (byte)lowTset.Value; //default: 180
            highT = (byte)highTset.Value; // default: 200
            ced = new CannyEdgeDetector(lowT, highT);

            AForge.Point sr_kostki = new AForge.Point(-3, -3);
            
            // inicjalizacja dekektora okręgów
            shapeChecker = new SimpleShapeChecker();

            Bitmap gray = Grayscale.CommonAlgorithms.Y.Apply(e.Argument as Bitmap);
            Bitmap pb = new Bitmap(gray);

            // stosujemy CED
            pb = ced.Apply(gray);

            Bitmap gray_copy = new Bitmap(gray, gray.Width, gray.Height);
            Bitmap target = new Bitmap(wykrywanieKrawedziToolStripMenuItem.Checked ? pb : gray_copy);

            // lokalizujemy obiekty przy wykorzystaniu klasy BlobCounter
            BlobCounter blobCounter = new BlobCounter();
            blobCounter.ProcessImage(pb);

            // przechowujemy info o wykrytych blobach w tablicy blobs[]
            Blob[] blobs = blobCounter.GetObjectsInformation();

            // zapamiętujemy poprzedni środek kostki
            poprzedni_srodek = new AForge.Point(srX, srY);
            int poprz_ilosc = il_oczek;

            // tworzymy obiekt graphics na którym będziemy rysować
            Graphics g = Graphics.FromImage(target);


            // sprawdzamy wszystkie bloby i zaznaczamy te
            // które zostały uznane za okręgi
            for (int i = 0, n = blobs.Length; i < n; i++)
            {
                List<IntPoint> edgePoints = blobCounter.GetBlobsEdgePoints(blobs[i]);

                AForge.Point center;
                float radius;

                if (shapeChecker.IsCircle(edgePoints, out center, out radius) && radius > (float)minRSet.Value && radius < (float)maxRSet.Value)
                {
                    ilocz++;
                    sr_kostki += center;

                    if (zarysujOczkaToolStripMenuItem.Checked)
                    {
                        try
                        {
                            g.DrawEllipse(redPen,
                                (int)(center.X - radius),
                                (int)(center.Y - radius),
                                (int)(radius * 2),
                                (int)(radius * 2));
                        }
                        catch
                        {
                            Console.WriteLine("Blad przy probie rysowania kola");
                            Console.WriteLine("x = " + center.X + "  y = " + center.Y);
                            Console.WriteLine("r = " + radius);
                        }
                    }
                }
            }


            g.FillRectangle(new SolidBrush(System.Drawing.Color.Red), point.X - 3, point.Y - 3, 6, 6);

            sr_kostki /= ilocz > 0 ? ilocz : 1;

            // blokujemy ilosc oczek i jej polozenie jesli te wartosci nie zmienily sie gwaltownie
            // w ciagu kilku sekund
            if (Math.Abs(poprzedni_srodek.X - sr_kostki.X) < 80 && Math.Abs(poprzedni_srodek.Y - sr_kostki.Y) < 80 && il_oczek > 0)
                licznik += licznik < 10? 1: 0;
            else
            {
                licznik = 0;
                lock_kostki = false;
            }

            if (licznik >= 5)
                lock_kostki = true;

            if (!lock_kostki)
            {
                srX = (int)sr_kostki.X;
                srY = (int)sr_kostki.Y;
                il_oczek = ilocz;
               
                // sprawdzamy blad kalibracji
                // i czy srodek wyestymowany oraz rzeczywisty nakladaja sie
                if (K_srodek.Y > 3 && (!kalibracja))
                {
                    skele_srodek = obliczWspolrzedneSrodka(srX, srY);
                    K_srodek.X = SkeletonPointToScreen(skele_srodek).X;
                    K_srodek.Y = SkeletonPointToScreen(skele_srodek).Y;
                }
            }

            g.FillRectangle(yellowBrush, sr_kostki.X - 3, sr_kostki.Y - 3, 6, 6); ;

            if (kalibracja && webc_points.Count > 0)
            {
                foreach (System.Drawing.Point p in webc_points)
                    g.FillRectangle(pinkBrush, p.X - 3, p.Y - 3, 6, 6);
            }
            g.Dispose();

            DrawOnWebcamPictureBox(target);

        }

        /// <summary>
        /// Rysuje bitmapę b na kinectPictureBox
        /// </summary>
        private void DrawOnKinectPixtureBox(Bitmap b) {
            if (kinectPictureBox.InvokeRequired)
            {
                try
                {
                    PokaPoka d = new PokaPoka(DrawOnKinectPixtureBox);
                    kinectPictureBox.Invoke(d, new object[] { b });
                }
                catch {
                    Console.WriteLine("Invoke dla kinectPixtureBox nie powiodl sie");
                }
            }
            else
                kinectPictureBox.Image = b;
        }
        /// <summary>
        /// Rysuje bitmapę b na webcamPictureBox
        /// </summary>
        private void DrawOnWebcamPictureBox(Bitmap b)
        {
            if (webcamPictureBox.InvokeRequired)
            {
                try
                {
                    PokaPoka d = new PokaPoka(DrawOnWebcamPictureBox);
                    webcamPictureBox.Invoke(d, new object[] { b });
                }
                catch{
                    Console.WriteLine("Invoke dla webcamPictureBox nie powiodl sie");
                }
            }
            else
                webcamPictureBox.Image = b;
        }

        ///<summary>
        /// delegat dla bitmapy  wykorzystywanyw  w funkcjach webcamPictureBox
        /// i kinectPictureBox
        ///</summary>
        private delegate void PokaPoka(Bitmap b);

        // przetwarzanie pojedynczej klatki z kamery RGB
        void FrameReady(object sender, ColorImageFrameReadyEventArgs e)
        {
            if (bw_kinect.IsBusy) return;
            ColorImageFrame imageFrame = e.OpenColorImageFrame();

            if (imageFrame != null)
            {
                Bitmap bitmap = imageFrame.ToBitmap();
                bw_kinect.RunWorkerAsync(bitmap);

                imageFrame.Dispose();
            }
        }

        ///<summary>
        /// handler do wykrywania szkieletu
        /// </summary>
        private void SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            using (SkeletonFrame skeletonFrame = e.OpenSkeletonFrame())
            {
                if (skeletonFrame != null && szkieletToolStripMenuItem.Checked)
                {
                    skeletonFrame.CopySkeletonDataTo(skeletonData);
                }

            }
        }

        ///<summary>
        /// wybor szkieletow do rysowania
        /// </summary>
        private void DrawSkeletons(Graphics g)
        {
            foreach (Skeleton skeleton in skeletonData)
            {
                if (skeleton == null) continue;
                if (skeleton.TrackingState == SkeletonTrackingState.Tracked || true)
                {
                   DrawTrackedSkeletonJoints(skeleton.Joints, g);   
                }

            }
        }

        ///<summary>
        /// rysowanie calego szkieletu
        /// </summary>
        private void DrawTrackedSkeletonJoints(JointCollection jointCollection, Graphics g)
        {
            // Render głowy i ramion
            DrawBone(g, jointCollection[JointType.Head], jointCollection[JointType.ShoulderCenter]);
            DrawBone(g, jointCollection[JointType.ShoulderCenter], jointCollection[JointType.ShoulderLeft]);
            DrawBone(g, jointCollection[JointType.ShoulderCenter], jointCollection[JointType.ShoulderRight]);

            // Render lewej reki
            DrawBone(g, jointCollection[JointType.ShoulderLeft], jointCollection[JointType.ElbowLeft]);
            DrawBone(g, jointCollection[JointType.ElbowLeft], jointCollection[JointType.WristLeft]);
            DrawBone(g, jointCollection[JointType.WristLeft], jointCollection[JointType.HandLeft]);

            // render prawej reki
            DrawBone(g, jointCollection[JointType.ShoulderRight], jointCollection[JointType.ElbowRight]);
            DrawBone(g, jointCollection[JointType.ElbowRight], jointCollection[JointType.WristRight]);
            DrawBone(g, jointCollection[JointType.WristRight], jointCollection[JointType.HandRight]);

            // Render bioder i kregoslupa
            DrawBone(g, jointCollection[JointType.ShoulderCenter], jointCollection[JointType.HipCenter]);
            DrawBone(g, jointCollection[JointType.HipCenter], jointCollection[JointType.HipRight]);
            DrawBone(g, jointCollection[JointType.HipCenter], jointCollection[JointType.HipLeft]);

            // Render lewej nogi
            DrawBone(g, jointCollection[JointType.HipLeft], jointCollection[JointType.KneeLeft]);
            DrawBone(g, jointCollection[JointType.KneeLeft], jointCollection[JointType.AnkleLeft]);
            DrawBone(g, jointCollection[JointType.AnkleLeft], jointCollection[JointType.FootLeft]);

            // Render prawej nogi
            DrawBone(g, jointCollection[JointType.HipRight], jointCollection[JointType.KneeRight]);
            DrawBone(g, jointCollection[JointType.KneeRight], jointCollection[JointType.AnkleRight]);
            DrawBone(g, jointCollection[JointType.AnkleRight], jointCollection[JointType.FootRight]);

        }

        ///<summary>
        /// rysowanie kosci 
        /// </summary>
        private void DrawBone(Graphics g, Joint jointFrom, Joint jointTo)
        {
            if (jointFrom.TrackingState == JointTrackingState.NotTracked ||
            jointTo.TrackingState == JointTrackingState.NotTracked)
            {
                return; // jeśli jeden z przegubów nie jest śledzony, nie rysujemy nic
            }

            if (jointFrom.TrackingState == JointTrackingState.Inferred ||
            jointTo.TrackingState == JointTrackingState.Inferred)
            {
                DrawNonTrackedBoneLine(g, jointFrom.Position, jointTo.Position);  // Draw thin lines if either one of the joints is inferred
            }

            if (jointFrom.TrackingState == JointTrackingState.Tracked &&
            jointTo.TrackingState == JointTrackingState.Tracked)
            {
                DrawTrackedBoneLine(g, jointFrom.Position, jointTo.Position);  // Draw bold lines if the joints are both tracked
            }
        }

        ///<summary>
        /// rysowanie kości której konce nie są śledzone, a jedynie szacujemy jej położenie
        ///</summary>
        private void DrawNonTrackedBoneLine(Graphics g, SkeletonPoint from, SkeletonPoint to)
        {
            System.Drawing.Point pf = new System.Drawing.Point(SkeletonPointToScreen(from).X, SkeletonPointToScreen(from).Y);
            System.Drawing.Point pt = new System.Drawing.Point(SkeletonPointToScreen(to).X, SkeletonPointToScreen(to).Y);
            g.DrawLine(infferedBonePen, pf, pt);
        }

        ///<summary>
        /// rysowanie kości której końce są śledzone
        ///</summary>
        private void DrawTrackedBoneLine(Graphics g, SkeletonPoint from, SkeletonPoint to)
        {
            System.Drawing.Point pf = new System.Drawing.Point(/*width - */ SkeletonPointToScreen(from).X, SkeletonPointToScreen(from).Y);
            System.Drawing.Point pt = new System.Drawing.Point(/*width - */ SkeletonPointToScreen(to).X, SkeletonPointToScreen(to).Y);
            g.DrawLine(trackedBonePen, pf, pt);
        }

        ///<summary>
        /// funkcja przeliczająca współrzędne punktu z układu szkieletowego 
        /// na wsp ekranu (ColorMap)
        ///</summary>
        private System.Drawing.Point SkeletonPointToScreen(SkeletonPoint skelpoint)
        {
            try
            {
                ColorImagePoint Point = this.sensor.MapSkeletonPointToColor(skelpoint, ColorImageFormat.RgbResolution640x480Fps30);
                return new System.Drawing.Point(Point.X, Point.Y);
            }
            catch
            {
                Console.WriteLine("Cos poszlo nie tak z MapSkeletonPointToColor.");
                return new System.Drawing.Point(640 / 2, 480 / 2);
            }
        }

        ///<summary>
        /// handler dla kamery glebokosci
        /// </summary>
        void DepthFrameReady(object sender, DepthImageFrameReadyEventArgs e)
        {

            DepthImageFrame depthImage = e.OpenDepthImageFrame();

            if (depthImage != null)
            {
                Bitmap bitmap = depthImage.ToBitmap();

                Graphics g = Graphics.FromImage(bitmap);

                // w trakcie procedury kalibracji
                if (kalibracja || kinect_points.Count > 0 && kinc_spoints.Count < 4)
                {

                    for (int i = 0; i < kinect_points.Count; i++ )
                    {
                        
                        SkeletonPoint s = depthImage.MapToSkeletonPoint(kinect_points[i].X, kinect_points[i].Y);
                        kinc_spoints[i] = s;
                        g.FillRectangle(blueBrush, /*640 - */ kinect_points[i].X - 3, kinect_points[i].Y - 3, 6, 6);
                    }
                }

                g.Dispose();
                kinectPictureBox.Image = bitmap;

                depthImage.Dispose();
            }
        }

        ///<summary>
        /// procedura zmiany wektorów bazowych
        /// do kalibracji układu webcam-Kinect
        ///</summary>
        private void kalibracja_wspolrzednych()
        {

            kinc_wersory = new List<Vector3>();
            webc_wersory = new List<Vector2>();

            Console.WriteLine("Skele (wsp 3D)");
            foreach (SkeletonPoint m in kinc_spoints)
            {
                Console.WriteLine("{" + m.X.ToString() + ", " + m.Y.ToString() + ", " + m.Z.ToString() + "}");
            }

            Vector3 s = new Vector3(kinc_spoints[0].X - kinc_spoints[1].X, kinc_spoints[0].Y - kinc_spoints[1].Y, kinc_spoints[0].Z - kinc_spoints[1].Z);
            kinc_wersory.Add(s);
            Console.WriteLine("S1: " + s.ToString());

            s = new Vector3(kinc_spoints[2].X - kinc_spoints[1].X, kinc_spoints[2].Y - kinc_spoints[1].Y, kinc_spoints[2].Z - kinc_spoints[1].Z);
            kinc_wersory.Add(s);
            Console.WriteLine("S2: " + s.ToString());


            w1 = new Vector2(webc_points[0].X - webc_points[1].X, webc_points[0].Y - webc_points[1].Y);
            webc_wersory.Add(w1);
            Console.WriteLine("W1: " + w1.ToString());


            w2 = new Vector2(webc_points[2].X - webc_points[1].X, webc_points[2].Y - webc_points[1].Y);
            webc_wersory.Add(w2);
            Console.WriteLine("W2: " + w2.ToString());


            skele_srodek = obliczWspolrzedneSrodka(srX, srY);
            K_srodek.X = SkeletonPointToScreen(skele_srodek).X;
            K_srodek.Y = SkeletonPointToScreen(skele_srodek).Y;
            Console.WriteLine("Wsp srodka:" + SkeletonPointToScreen(skele_srodek).ToString());


        }

        ///<summary>
        ///oblicza lokalizację kości w układzie ze zmienionymi wektorami bazowymi
        ///</summary>
        private SkeletonPoint obliczWspolrzedneSrodka(int sX, int sY)
        {
            SkeletonPoint k = new SkeletonPoint();
            float wyz = 1 / (w2.Y * w1.X - w2.X * w1.Y);

            Console.WriteLine("Srodek na webcamie: {" + srX.ToString() + "," + srY.ToString() + "}");
            Console.WriteLine("nowy X:" + wyz * ((srX - webc_points[1].X) * w2.Y - (srY - webc_points[1].Y) * w2.X) + ", nowy Y: " + wyz * ((srY - webc_points[1].Y) * w1.X - (srX - webc_points[1].X) * w1.Y));
            Vector2 w = new Vector2(wyz * ((srX - webc_points[1].X) * w2.Y - (srY - webc_points[1].Y) * w2.X), wyz * ((srY - webc_points[1].Y) * w1.X - (srX - webc_points[1].X) * w1.Y));
            Console.WriteLine("Srodek w nowych wsp: " + w.ToString());
            point.X = (int)(w.X * w1.X + w.Y * w2.X + webc_points[1].X);
            point.Y = (int)(w.X * w1.Y + w.Y * w2.Y + webc_points[1].Y);
            Console.WriteLine("Punkt spr: " + point.ToString());


            k.X = w.X * kinc_wersory[0].X + w.Y * kinc_wersory[1].X + kinc_spoints[1].X;
            k.Y = w.X * kinc_wersory[0].Y + w.Y * kinc_wersory[1].Y + kinc_spoints[1].Y;
            k.Z = w.X * kinc_wersory[0].Z + w.Y * kinc_wersory[1].Z + kinc_spoints[1].Z;
            Console.WriteLine("Skele srodka: {" + k.X.ToString() + ", " + k.Y.ToString() + ", " + k.Z.ToString() + "}");

            return k;

        }

        // ---------------------------- Handlery do elementów formularza WinForms ---------------:
        private void kalibracjaToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (!KinectSensor.KinectSensors.Count.Equals(0))
            {
                kinect_points = new List<System.Drawing.Point>();
                for (int i = 0; i < 3; i++) {
                    SkeletonPoint s = new SkeletonPoint();
                    kinc_spoints.Add(s);
                }

                webc_points = new List<System.Drawing.Point>();

                kalibracja = true;
                Console.WriteLine("Kalibracja:" + kalibracja);


                sensor.ColorStream.Disable();
                sensor.DepthStream.Enable(DepthImageFormat.Resolution640x480Fps30);
            }

        }

        private void pictureBox_Click(object sender, EventArgs e)
        {
            if (kalibracja)
            {
                System.Drawing.Point p = ((PictureBox)sender).PointToClient(MousePosition);
                Bitmap b = ((PictureBox)sender).Image as Bitmap;
                Console.WriteLine("Pobrano punkt " + p.ToString());
                if (sender == kinectPictureBox && kinect_points.Count < 3)
                {
                    kinect_points.Add(p);
                }
                else if (sender == webcamPictureBox && webc_points.Count < 3)
                {
                    webc_points.Add(p);
                }

                if (kinect_points.Count == 3 && webc_points.Count == 3)
                {
                    kalibracja = false;

                    Console.WriteLine("Skele:");
                    foreach(SkeletonPoint k in kinc_spoints)
                        Console.WriteLine(SkeletonPointToScreen(k).ToString());

                    Console.WriteLine("Web:");
                    foreach (System.Drawing.Point w in webc_points)
                        Console.WriteLine(w.ToString());
                   
                    kalibracja_wspolrzednych();
                    sensor.DepthStream.Disable();
                    sensor.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);
                    
                }
            }
        }

        

        private void zapiszToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string timeString = string.Format("{0:dd-MM_hhmmss}", DateTime.Now);

            kinectPictureBox.Image.Save(@"C:\Users\drowka\Documents\KINECT\Data\Kinect-" + timeString + ".png", ImageFormat.Png);
            webcamPictureBox.Image.Save(@"C:\Users\drowka\Documents\KINECT\Data\Webcam-" + timeString + ".png", ImageFormat.Png);
        }

        private void downButton_Click(object sender, EventArgs e)
        {
            if (!sensor.ElevationAngle.Equals(-24))
            {
                sensor.ElevationAngle -= 4;
            }
        }

        private void upButton_Click(object sender, EventArgs e)
        {
            if (!sensor.ElevationAngle.Equals(24))
                sensor.ElevationAngle += 4;
        }

        public void ZamknijWatki()
        {
            szkieletToolStripMenuItem.CheckState = CheckState.Unchecked;

            if (!KinectSensor.KinectSensors.Count.Equals(0))
            {
                sensor.SkeletonStream.Disable();
                sensor.ColorStream.Disable();
                sensor.Stop();
            }
            CloseVideoSource();

            Close();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            bw_kinect.CancelAsync();
            bw_kinect.Dispose();
            bw_webcam.CancelAsync();
            bw_webcam.Dispose();

            if (!KinectSensor.KinectSensors.Count.Equals(0))
            {
                sensor.SkeletonStream.Disable();
                sensor.ColorStream.Disable();
                sensor.Stop();
            }
            CloseVideoSource();

        }
    }
}
