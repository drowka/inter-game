using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Kinect;
using System.Diagnostics;


namespace XNA_Debug
{
    /// <summary>
    /// Klasa KinectGame odpowiada za cała mechanikę interakcji 
    /// z uzytkownikiem z wykorzystaniem Kinecta: wykrywanie ruchu, 
    /// rysowanie elementów gry, wyswietlanie wizualizacji i odpowiednich wideo postaci. 
    /// </summary>
    public class KinectGame : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        // wideo animacji
        private List<Video> videoA = new List<Video>();
        private List<Video> videoB = new List<Video>();
        private List<Video> videoC = new List<Video>();
        private List<Video> videoD = new List<Video>();
        private List<Video> videoE = new List<Video>();
        private VideoPlayer player;
        private SamplerState clampTextureAddressMode;

        private SpriteFont font;
        private int il_oczek;

        /// <summary>
        /// Formularz WinForms do kontrolowania informacji z webcama
        /// </summary>
        private WebcamForm form;

        // sensor i jego tablica przechowująca info o szkieletach graczy
        private KinectSensor sensor;
        private Skeleton[] skeletonData;

        /// <summary>
        /// Współrzędne tekstury dla przegubów.
        /// </summary>
        private Vector2 jointOrigin;

        /// <summary>
        /// Tekstura przegubów.
        /// </summary>
        private Texture2D jointTexture;

        /// <summary>
        /// Współrzędne tekstury dla kości.
        /// </summary>
        private Vector2 boneOrigin;

        /// <summary>
        /// Tekstura kości.
        /// </summary>
        private Texture2D boneTexture;

        private Vector2 srodek_kosci;
        private SkeletonPoint skele_srodek_kosci;

        // Obszary interakcji:
        string obszar = "A";

        int Width =  GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
        int Height = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

        // flagi sprawdzające czy procedura interakcji jest w toku
        private bool interakcja = false;
        private bool inter_odtwarzanie = false;

        // flaga czy tryb wyswietlania "debug" jest włączony
        private bool debug = true;

        public KinectGame()
        {
            form = new WebcamForm();
            srodek_kosci = new Vector2(0f, 0f);

            this.Window.Title = "XNA";

            // opcje wyświetlania gry na ekranie w wersjach DEBUG i RELEASE
            graphics = new GraphicsDeviceManager(this);
            graphics.PreparingDeviceSettings += new EventHandler<PreparingDeviceSettingsEventArgs>
                (GraphicsDevicePreparingDeviceSettings);
#if DEBUG
            this.IsMouseVisible = true;
            graphics.PreferredBackBufferWidth = Width;
            graphics.PreferredBackBufferHeight = Height;
            this.graphics.SynchronizeWithVerticalRetrace = true;
            graphics.ToggleFullScreen();

#else
            graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            graphics.IsFullScreen = true;
            debug = false;

#endif
            Content.RootDirectory = "Content";

            // przygotowujemy program na podłączenie Kinecta (jeśli jakiś został wykryty)
            if (!KinectSensor.KinectSensors.Count.Equals(0))
            {
                sensor = form.przekazSensor();

                sensor.SkeletonFrameReady += SkeletonFrameReady2;
                skeletonData = new Skeleton[sensor.SkeletonStream.FrameSkeletonArrayLength];
            }

        }

        /// <summary>
        /// Sprawdzenie czy strumień z układu głębokości (szkieletowego) jest gotowy 
        /// i przekopiowanie info o szkieletach do tablicy skeletonData
        /// </summary>
        public void SkeletonFrameReady2(object sender, SkeletonFrameReadyEventArgs e)
        {
            using (SkeletonFrame skeletonFrame = e.OpenSkeletonFrame())
            {
                if (skeletonFrame != null)
                    skeletonFrame.CopySkeletonDataTo(skeletonData);
            }
        }
        /// <summary>
        /// Inicjalizacja wszystkich elementów niezbędnych do rozpoczęcia gry (w naszym przypadku tylko base)
        /// </summary>
        protected override void Initialize()
        {

            base.Initialize();
        }

        /// <summary>
        /// LoadContent jest wzywany tylko raz na grę i ładuje wszystkie zasoby programu, 
        /// by były gotowe do użycia.
        /// </summary>
        protected override void LoadContent()
        {
            form.Show();

            // tworzymy nowy spriteBatch na którym będziemy rysować
            this.spriteBatch = new SpriteBatch(this.GraphicsDevice);
            this.Services.AddService(typeof(SpriteBatch), this.spriteBatch);

            font = Content.Load<SpriteFont>("Font");

            // ładujemy wideo
            for (int i = 1; i < 7; i++)
            {
                videoA.Add(Content.Load<Video>(i.ToString()+"A"));
                videoB.Add(Content.Load<Video>(i.ToString() + "B"));
                videoC.Add(Content.Load<Video>(i.ToString() + "C"));
                videoD.Add(Content.Load<Video>(i.ToString() + "D"));
                videoE.Add(Content.Load<Video>(i.ToString() + "E"));
            }

            // inicjalizujemy odtwarzacz 
            player = new VideoPlayer();
            player.IsLooped = true;
            player.IsMuted = true;


            clampTextureAddressMode = new SamplerState
            {
                AddressU = TextureAddressMode.Clamp,
                AddressV = TextureAddressMode.Clamp
            };
            
            // ustawiamy wybrane tekstury dla kości i przegubów
            this.jointTexture = Content.Load<Texture2D>("Joint");
            this.jointOrigin = new Vector2(this.jointTexture.Width / 2, this.jointTexture.Height / 2);

            this.boneTexture = Content.Load<Texture2D>("Bone");
            this.boneOrigin = new Vector2(0.5f, 0.0f);
        }


        /// <summary>
        /// Zawiera mechanikę gry jak sprawdzanie czy rozpoczęto interakcje,
        /// zmiane położenia kości itp.
        /// </summary>
        protected override void Update(GameTime gameTime)
        {
            // przekazujemy grze wykrytą ilość oczek 
            // sprawdzamy czy różni się od poprzedniej 
            // i czy mieści się w przedziale
            int temp = form.przekazIloscOczek();
            int pop_il_oczek = il_oczek;
            if (temp > 6)
            {
                Random r = new Random();
                il_oczek = r.Next(6) + 1;
            }
            else
                il_oczek = temp;

            // przekazujemy środek we wsp szkieletowych
            skele_srodek_kosci = form.przekazSkeleSrodek();

            if(Math.Abs(skele_srodek_kosci.Y) > 0)
                srodek_kosci = SkeletonToColorMap(skele_srodek_kosci);

            // odtwarzamy odpowiednie wideo interakcji
            if (il_oczek > 0 && player.State != MediaState.Playing && !interakcja)
                player.Play(videoA[il_oczek - 1]);
            else if (il_oczek != pop_il_oczek){
                interakcja = false;
                inter_odtwarzanie = false;
                obszar = "A";

                if (player.State == MediaState.Playing)
                    player.Stop();
                zmienVideo(form.przekazIloscOczek(), obszar);
            }
            else if (il_oczek <= 0)
                player.Stop();

            // jesli interakcja trwa
            if (interakcja)
            {
                // jesli filmik interakcji jest w toku
                if (!inter_odtwarzanie)
                {
                    if (player.State == MediaState.Playing)
                        player.Stop();
                    inter_odtwarzanie = true;
                    player.IsLooped = false;
                    zmienVideo(il_oczek, obszar);

                }
                else if (player.State == MediaState.Stopped)
                {
                   inter_odtwarzanie = false;
                   interakcja = false;
                }
            }

            // wyłącza wyswietlanie dodatkowych info
            if (Keyboard.GetState().IsKeyDown(Keys.R))
                debug = false;

            // włącza wyswietlanie informacji pomocnych w debugowaniu
            // takie jak srodek kosci i jego wspolrzedne
            if (Keyboard.GetState().IsKeyDown(Keys.D))
                debug = true;

            // przełącza tryb pełnoekranowy/okno
            if (Keyboard.GetState().IsKeyDown(Keys.F))
                graphics.ToggleFullScreen();

            // wyjscie z gry
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                if (player.State == MediaState.Playing)
                    player.Stop();
                this.Exit();
                form.ZamknijWatki();
                form.Dispose();
            }
            base.Update(gameTime);
        }

        private void zmienVideo(int i, string c) {

            if (i > 0)
            switch(c){
                case "A":
                    player.IsLooped = true;
                    player.Play(videoA[i - 1]);
                    break;
                case "B":
                    player.Play(videoB[i - 1]);
                    break;
                case "C":
                    player.Play(videoC[i - 1]);
                    break;
                case "D":
                    player.Play(videoD[i - 1]);
                    break;
                case "E":
                    player.Play(videoE[i - 1]);
                    break;
            }
                
        }

        /// <summary>
        /// Rysuje elementy wizualne gry
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            GraphicsDevice.SamplerStates[0] = clampTextureAddressMode;
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied,
                    clampTextureAddressMode, null, null, null);

            float skala_video = 1.0f;
            Vector2 pozycja_Video = new Vector2();

            // wylicza pozycję wideo
            if (srodek_kosci.Y > 0 && il_oczek > 0)
            {
                srodek_kosci.X += (Width - 640) / 2;
                srodek_kosci.Y += Height - 480;

                pozycja_Video.X = srodek_kosci.X;
                pozycja_Video.Y = srodek_kosci.Y - player.GetTexture().Height / 2 - 20;

            }

            // rysuje odpowiednie wideo
            if (player.State == MediaState.Playing)
            {
                if (pozycja_Video.X > 0)
                    spriteBatch.Draw(player.GetTexture(),
                        pozycja_Video,
                        new Rectangle(0, 0, player.GetTexture().Width, player.GetTexture().Height), Color.White, 0f,
                        new Vector2(player.GetTexture().Width / 2, player.GetTexture().Height / 2),
                        skala_video, SpriteEffects.None, 0.0f);
            }
            else
            {

                if (player.State == MediaState.Stopped && il_oczek > 0)
                {
                    player.Play(videoA[il_oczek - 1]);
                    player.Pause();

                    Debug.WriteLine(player.State.ToString());

                    if (pozycja_Video.X > 0)
                        spriteBatch.Draw(player.GetTexture(),
                            pozycja_Video,
                            new Rectangle(0, 0, player.GetTexture().Width, player.GetTexture().Height), Color.White, 0f,
                            new Vector2(player.GetTexture().Width / 2, player.GetTexture().Height / 2),
                            skala_video, SpriteEffects.None, 0.0f);

                }
                
            }

            // jesli debug, wyswietla pozycje srodka w przestrzeni Skeleton
            if (debug)
            {
                spriteBatch.DrawString(font, skele_srodek_kosci.X.ToString(), new Vector2(10, 220), Color.Yellow);
                spriteBatch.DrawString(font, skele_srodek_kosci.Y.ToString(), new Vector2(10, 290), Color.Yellow);
                spriteBatch.DrawString(font, skele_srodek_kosci.X.ToString(), new Vector2(10, 360), Color.Yellow);

                spriteBatch.DrawString(font, il_oczek.ToString(), new Vector2(Width - 110, 10), Color.WhiteSmoke);
                spriteBatch.DrawString(font, "s", srodek_kosci, Color.Yellow);
            }

            // rysowanie szkieletu gracza
            if (skeletonData != null)
            {
                foreach (var skeleton in skeletonData)
                {
                    if(skeleton != null)
                    switch (skeleton.TrackingState)
                    {

                        case SkeletonTrackingState.Tracked:

                            // rysowanie kości
                            this.DrawBone(skeleton.Joints, JointType.Head, JointType.ShoulderCenter);
                            this.DrawBone(skeleton.Joints, JointType.ShoulderCenter, JointType.ShoulderLeft);
                            this.DrawBone(skeleton.Joints, JointType.ShoulderCenter, JointType.ShoulderRight);
                            this.DrawBone(skeleton.Joints, JointType.ShoulderCenter, JointType.Spine);
                            this.DrawBone(skeleton.Joints, JointType.Spine, JointType.HipCenter);
                            this.DrawBone(skeleton.Joints, JointType.HipCenter, JointType.HipLeft);
                            this.DrawBone(skeleton.Joints, JointType.HipCenter, JointType.HipRight);

                            this.DrawBone(skeleton.Joints, JointType.ShoulderLeft, JointType.ElbowLeft);
                            this.DrawBone(skeleton.Joints, JointType.ElbowLeft, JointType.WristLeft);
                            this.DrawBone(skeleton.Joints, JointType.WristLeft, JointType.HandLeft);

                            this.DrawBone(skeleton.Joints, JointType.ShoulderRight, JointType.ElbowRight);
                            this.DrawBone(skeleton.Joints, JointType.ElbowRight, JointType.WristRight);
                            this.DrawBone(skeleton.Joints, JointType.WristRight, JointType.HandRight);

                            this.DrawBone(skeleton.Joints, JointType.HipLeft, JointType.KneeLeft);
                            this.DrawBone(skeleton.Joints, JointType.KneeLeft, JointType.AnkleLeft);
                            this.DrawBone(skeleton.Joints, JointType.AnkleLeft, JointType.FootLeft);

                            this.DrawBone(skeleton.Joints, JointType.HipRight, JointType.KneeRight);
                            this.DrawBone(skeleton.Joints, JointType.KneeRight, JointType.AnkleRight);
                            this.DrawBone(skeleton.Joints, JointType.AnkleRight, JointType.FootRight);

                            wykryjInterakcje(skeleton.Joints[JointType.HandRight].Position);
                            if (!interakcja)
                                wykryjInterakcje(skeleton.Joints[JointType.HandLeft].Position);
                          
                            // rysowanie przegubów
                            foreach (Joint j in skeleton.Joints)
                            {
                                Color jointColor;
                                float joint_size;
                                float A = 3.5f;
                                float B = 1.8f;
                                float C = 1.0f;
                                float D = 0.5f;

                                // dobieranie rozmiaru i koloru przegubu w zaleznosci od typu
                                switch (j.JointType) { 
                                    case JointType.Head:
                                        joint_size = A;
                                        jointColor = new Color(255, 254, 22);
                                        break;
                                    case JointType.ShoulderCenter:
                                    case JointType.ElbowLeft:
                                    case JointType.ElbowRight:
                                    case JointType.KneeLeft:
                                    case JointType.KneeRight:
                                        joint_size = C;
                                        jointColor = new Color(110, 180, 62);
                                        break;
                                    case JointType.HipCenter:
                                    case JointType.WristLeft:
                                    case JointType.WristRight:
                                    case JointType.AnkleLeft:
                                    case JointType.AnkleRight:
                                        joint_size = D;
                                        jointColor = new Color(255, 254, 0);
                                        break;
                                    default:
                                        joint_size = B;
                                        jointColor = new Color(254, 255, 76);
                                        break;
                                }

                                // jesli nie znajdujemy sie bezposrednio za koscia do rzucania, rysuje staw
                                if ((Math.Abs(j.Position.X - skele_srodek_kosci.X) > 0.6f || j.Position.Z < skele_srodek_kosci.Z - 0.2f) && skele_srodek_kosci.Z != 0)
                                spriteBatch.Draw(
                                    this.jointTexture,
                                    new Vector2(SkeletonToColorMap(j.Position).X + (Width - 640) / 2, SkeletonToColorMap(j.Position).Y + Height - 480),
                                    null,
                                    jointColor,
                                    0.0f,
                                    this.jointOrigin,
                                    joint_size,
                                    SpriteEffects.None,
                                    0.0f);
                            }

                            break;
                    }
                }
            }

            spriteBatch.End();
            
            base.Draw(gameTime);
        }
        /// <summary> 
        /// rysuje kosc dla zadanych punktow pocz i konc
        /// </summary>
        private void DrawBone(JointCollection joints, JointType startJoint, JointType endJoint)
        {

            Vector2 start = SkeletonToColorMap(joints[startJoint].Position);
            start.X += (Width - 640) / 2;
            start.Y += Height - 480;
            Vector2 end = SkeletonToColorMap(joints[endJoint].Position);
            end.X += (Width - 640) / 2;
            end.Y += Height - 480;
            Vector2 diff = end - start;
            Vector2 scale = new Vector2(2.5f, diff.Length() / this.boneTexture.Height);

            float angle = (float)Math.Atan2(diff.Y, diff.X) - MathHelper.PiOver2;

            Color color = new Color(0, 128, 1);
            // jesli nie znajdujemy sie za kostka do rzucania, rysuje zadana kosc szkieletu
            if((Math.Abs(joints[startJoint].Position.X - skele_srodek_kosci.X) > 0.6f || joints[startJoint].Position.Z < skele_srodek_kosci.Z - 0.2f) && skele_srodek_kosci.Z != 0)
                this.spriteBatch.Draw(this.boneTexture, start, null, color, angle, this.boneOrigin, scale, SpriteEffects.None, 0.0f);
        }
        /// <summary>
        /// wykrywa wirtualny dotyk użytkownika, przyporządkowuje go do odpowiedniego obszaru
        /// i zmienia odpowiednie flagi interakcji 
        /// </summary>
        private void wykryjInterakcje(SkeletonPoint dl) {
            if (Math.Abs(dl.X - skele_srodek_kosci.X) <= 0.4f && Math.Abs(dl.Z - skele_srodek_kosci.Z) <= 0.4f && Math.Abs(dl.Y - skele_srodek_kosci.Y) > 0.55f && !interakcja)
            {
                interakcja = true;
                if (dl.X < skele_srodek_kosci.X)
                {
                    if (Math.Abs(dl.Y - skele_srodek_kosci.Y) < 0.95f)
                    {
                        obszar = "B";
                        if(debug)
                            spriteBatch.DrawString(font, ":D", new Vector2(10, 10), Color.Red);
                    }
                    else {
                        obszar = "D";
                        if (debug)
                            spriteBatch.DrawString(font, ":D", new Vector2(10, 10), Color.Green);
                    }
                }
                else
                {
                    if (Math.Abs(dl.Y - skele_srodek_kosci.Y) < 0.95f)
                    {
                        if (il_oczek == 4)
                        {
                            interakcja = false;
                            inter_odtwarzanie = false;
                        }
                        else{
                        obszar = "C";
                        if (debug)
                            spriteBatch.DrawString(font, "D:", new Vector2(10, 10), Color.Yellow);
                        }
                    }
                    else {

                        obszar = "E";
                        if (debug)
                            spriteBatch.DrawString(font, "D:", new Vector2(10, 10), Color.Aquamarine);
                    }
                }
            }
        }

        /// <summary>
        /// przelicza wspolrzedne szkieletowe na ekranowe (ColorMap)
        /// </summary>
        private Vector2 SkeletonToColorMap(SkeletonPoint point)
        {
            if ((null != sensor) && (null != sensor.ColorStream))
            {
                var colorPt = sensor.MapSkeletonPointToColor(point, sensor.ColorStream.Format);
                return new Vector2(colorPt.X, colorPt.Y);
            }

            return Vector2.Zero;
        }

        private void GraphicsDevicePreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
        {
            e.GraphicsDeviceInformation.PresentationParameters.RenderTargetUsage = RenderTargetUsage.PreserveContents;
        }
    }
}
