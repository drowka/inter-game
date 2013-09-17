namespace XNA_Debug
{
    partial class WebcamForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.plikToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zapiszToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zamknijToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.opcjeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.filtryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.szkieletToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.prawyEkranToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.wykrywanieKrawedziToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zarysujOczkaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.kalibracjaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.kinectPictureBox = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.downButton = new System.Windows.Forms.Button();
            this.upButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.highTset = new System.Windows.Forms.NumericUpDown();
            this.lowTset = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.srlabel = new System.Windows.Forms.Label();
            this.rozmm = new System.Windows.Forms.Label();
            this.kset = new System.Windows.Forms.NumericUpDown();
            this.webcamSet = new System.Windows.Forms.ComboBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label8 = new System.Windows.Forms.Label();
            this.minRSet = new System.Windows.Forms.NumericUpDown();
            this.maxRSet = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.webcamPictureBox = new System.Windows.Forms.PictureBox();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kinectPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.highTset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lowTset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.minRSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxRSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.webcamPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.plikToolStripMenuItem,
            this.opcjeToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(8, 3, 0, 3);
            this.menuStrip1.Size = new System.Drawing.Size(1317, 25);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // plikToolStripMenuItem
            // 
            this.plikToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.zapiszToolStripMenuItem,
            this.zamknijToolStripMenuItem});
            this.plikToolStripMenuItem.Name = "plikToolStripMenuItem";
            this.plikToolStripMenuItem.Size = new System.Drawing.Size(38, 19);
            this.plikToolStripMenuItem.Text = "Plik";
            // 
            // zapiszToolStripMenuItem
            // 
            this.zapiszToolStripMenuItem.Name = "zapiszToolStripMenuItem";
            this.zapiszToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.zapiszToolStripMenuItem.Text = "Zapisz";
            this.zapiszToolStripMenuItem.Click += new System.EventHandler(this.zapiszToolStripMenuItem_Click);
            // 
            // zamknijToolStripMenuItem
            // 
            this.zamknijToolStripMenuItem.Name = "zamknijToolStripMenuItem";
            this.zamknijToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.zamknijToolStripMenuItem.Text = "Zamknij";
            // 
            // opcjeToolStripMenuItem
            // 
            this.opcjeToolStripMenuItem.CheckOnClick = true;
            this.opcjeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.filtryToolStripMenuItem,
            this.prawyEkranToolStripMenuItem,
            this.kalibracjaToolStripMenuItem});
            this.opcjeToolStripMenuItem.Name = "opcjeToolStripMenuItem";
            this.opcjeToolStripMenuItem.Size = new System.Drawing.Size(50, 19);
            this.opcjeToolStripMenuItem.Text = "Opcje";
            // 
            // filtryToolStripMenuItem
            // 
            this.filtryToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.szkieletToolStripMenuItem});
            this.filtryToolStripMenuItem.Name = "filtryToolStripMenuItem";
            this.filtryToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.filtryToolStripMenuItem.Text = "Kinect";
            // 
            // szkieletToolStripMenuItem
            // 
            this.szkieletToolStripMenuItem.Checked = true;
            this.szkieletToolStripMenuItem.CheckOnClick = true;
            this.szkieletToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.szkieletToolStripMenuItem.Name = "szkieletToolStripMenuItem";
            this.szkieletToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
            this.szkieletToolStripMenuItem.Text = "Szkielet";
            // 
            // prawyEkranToolStripMenuItem
            // 
            this.prawyEkranToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.wykrywanieKrawedziToolStripMenuItem,
            this.zarysujOczkaToolStripMenuItem});
            this.prawyEkranToolStripMenuItem.Name = "prawyEkranToolStripMenuItem";
            this.prawyEkranToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.prawyEkranToolStripMenuItem.Text = "Webcam";
            // 
            // wykrywanieKrawedziToolStripMenuItem
            // 
            this.wykrywanieKrawedziToolStripMenuItem.CheckOnClick = true;
            this.wykrywanieKrawedziToolStripMenuItem.Name = "wykrywanieKrawedziToolStripMenuItem";
            this.wykrywanieKrawedziToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.wykrywanieKrawedziToolStripMenuItem.Text = "Wykrywanie krawedzi";
            // 
            // zarysujOczkaToolStripMenuItem
            // 
            this.zarysujOczkaToolStripMenuItem.Checked = true;
            this.zarysujOczkaToolStripMenuItem.CheckOnClick = true;
            this.zarysujOczkaToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.zarysujOczkaToolStripMenuItem.Name = "zarysujOczkaToolStripMenuItem";
            this.zarysujOczkaToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.zarysujOczkaToolStripMenuItem.Text = "Zarysuj oczka";
            // 
            // kalibracjaToolStripMenuItem
            // 
            this.kalibracjaToolStripMenuItem.Name = "kalibracjaToolStripMenuItem";
            this.kalibracjaToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.kalibracjaToolStripMenuItem.Text = "Kalibracja";
            this.kalibracjaToolStripMenuItem.Click += new System.EventHandler(this.kalibracjaToolStripMenuItem_Click);
            // 
            // pictureBox1
            // 
            this.kinectPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.kinectPictureBox.Location = new System.Drawing.Point(16, 23);
            this.kinectPictureBox.Margin = new System.Windows.Forms.Padding(4);
            this.kinectPictureBox.Name = "pictureBox1";
            this.kinectPictureBox.Size = new System.Drawing.Size(640, 480);
            this.kinectPictureBox.TabIndex = 2;
            this.kinectPictureBox.TabStop = false;
            this.kinectPictureBox.Click += new System.EventHandler(this.pictureBox_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(193, 523);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(128, 16);
            this.label1.TabIndex = 4;
            this.label1.Text = "Kontrola nachylenia:";
            // 
            // downButton
            // 
            this.downButton.Location = new System.Drawing.Point(328, 517);
            this.downButton.Name = "downButton";
            this.downButton.Size = new System.Drawing.Size(49, 26);
            this.downButton.TabIndex = 5;
            this.downButton.Text = "dół";
            this.downButton.UseVisualStyleBackColor = true;
            this.downButton.Click += new System.EventHandler(this.downButton_Click);
            // 
            // upButton
            // 
            this.upButton.Location = new System.Drawing.Point(383, 517);
            this.upButton.Name = "upButton";
            this.upButton.Size = new System.Drawing.Size(49, 26);
            this.upButton.TabIndex = 6;
            this.upButton.Text = "góra";
            this.upButton.UseVisualStyleBackColor = true;
            this.upButton.Click += new System.EventHandler(this.upButton_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label3.Location = new System.Drawing.Point(12, 519);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(128, 20);
            this.label3.TabIndex = 8;
            this.label3.Text = "Podłącz Kinect";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(680, 475);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(123, 20);
            this.label2.TabIndex = 10;
            this.label2.Text = "Liczba oczek: ";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(797, 475);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(19, 20);
            this.label4.TabIndex = 11;
            this.label4.Text = "0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(439, 523);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(58, 16);
            this.label5.TabIndex = 15;
            this.label5.Text = "Dolny T:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(557, 522);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(59, 16);
            this.label6.TabIndex = 16;
            this.label6.Text = "Górny T:";
            // 
            // highTset
            // 
            this.highTset.Location = new System.Drawing.Point(623, 522);
            this.highTset.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.highTset.Name = "highTset";
            this.highTset.Size = new System.Drawing.Size(44, 22);
            this.highTset.TabIndex = 20;
            this.highTset.Value = new decimal(new int[] {
            200,
            0,
            0,
            0});
            // 
            // lowTset
            // 
            this.lowTset.Location = new System.Drawing.Point(504, 521);
            this.lowTset.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.lowTset.Name = "lowTset";
            this.lowTset.Size = new System.Drawing.Size(46, 22);
            this.lowTset.TabIndex = 21;
            this.lowTset.Value = new decimal(new int[] {
            180,
            0,
            0,
            0});
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label7.ForeColor = System.Drawing.Color.Red;
            this.label7.Location = new System.Drawing.Point(827, 475);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(76, 20);
            this.label7.TabIndex = 22;
            this.label7.Text = "Środek: ";
            // 
            // srlabel
            // 
            this.srlabel.AutoSize = true;
            this.srlabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.srlabel.ForeColor = System.Drawing.Color.Red;
            this.srlabel.Location = new System.Drawing.Point(900, 475);
            this.srlabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.srlabel.Name = "srlabel";
            this.srlabel.Size = new System.Drawing.Size(19, 20);
            this.srlabel.TabIndex = 23;
            this.srlabel.Text = "0";
            // 
            // rozmm
            // 
            this.rozmm.AutoSize = true;
            this.rozmm.Location = new System.Drawing.Point(674, 523);
            this.rozmm.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.rozmm.Name = "rozmm";
            this.rozmm.Size = new System.Drawing.Size(58, 16);
            this.rozmm.TabIndex = 24;
            this.rozmm.Text = "Macierz:";
            // 
            // kset
            // 
            this.kset.Increment = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.kset.Location = new System.Drawing.Point(739, 522);
            this.kset.Maximum = new decimal(new int[] {
            40,
            0,
            0,
            0});
            this.kset.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.kset.Name = "kset";
            this.kset.Size = new System.Drawing.Size(34, 22);
            this.kset.TabIndex = 25;
            this.kset.Value = new decimal(new int[] {
            21,
            0,
            0,
            0});
            // 
            // webcamSet
            // 
            this.webcamSet.FormattingEnabled = true;
            this.webcamSet.Location = new System.Drawing.Point(779, 520);
            this.webcamSet.Name = "webcamSet";
            this.webcamSet.Size = new System.Drawing.Size(108, 24);
            this.webcamSet.TabIndex = 26;
            this.webcamSet.SelectedIndexChanged += new System.EventHandler(this.webcamSet_SelectedIndexChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(894, 523);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(39, 16);
            this.label8.TabIndex = 27;
            this.label8.Text = "Min r:";
            // 
            // minRSet
            // 
            this.minRSet.Location = new System.Drawing.Point(940, 521);
            this.minRSet.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.minRSet.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.minRSet.Name = "minRSet";
            this.minRSet.Size = new System.Drawing.Size(34, 22);
            this.minRSet.TabIndex = 28;
            this.minRSet.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // maxRSet
            // 
            this.maxRSet.Location = new System.Drawing.Point(1031, 520);
            this.maxRSet.Maximum = new decimal(new int[] {
            40,
            0,
            0,
            0});
            this.maxRSet.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.maxRSet.Name = "maxRSet";
            this.maxRSet.Size = new System.Drawing.Size(34, 22);
            this.maxRSet.TabIndex = 29;
            this.maxRSet.Value = new decimal(new int[] {
            40,
            0,
            0,
            0});
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(981, 523);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(43, 16);
            this.label9.TabIndex = 30;
            this.label9.Text = "Max r:";
            // 
            // webcamPictureBox
            // 
            this.webcamPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.webcamPictureBox.Location = new System.Drawing.Point(664, 23);
            this.webcamPictureBox.Margin = new System.Windows.Forms.Padding(4);
            this.webcamPictureBox.Name = "webcamPictureBox";
            this.webcamPictureBox.Size = new System.Drawing.Size(640, 480);
            this.webcamPictureBox.TabIndex = 9;
            this.webcamPictureBox.TabStop = false;
            this.webcamPictureBox.Click += new System.EventHandler(this.pictureBox_Click);
            // 
            // WebcamForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1317, 558);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.maxRSet);
            this.Controls.Add(this.minRSet);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.webcamSet);
            this.Controls.Add(this.kset);
            this.Controls.Add(this.rozmm);
            this.Controls.Add(this.srlabel);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.lowTset);
            this.Controls.Add(this.highTset);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.webcamPictureBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.upButton);
            this.Controls.Add(this.downButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.kinectPictureBox);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "WebcamForm";
            this.Text = "Kinect - test RGB i Depth";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kinectPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.highTset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lowTset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.minRSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxRSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.webcamPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem plikToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem zapiszToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem zamknijToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem opcjeToolStripMenuItem;
        private System.Windows.Forms.PictureBox kinectPictureBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button downButton;
        private System.Windows.Forms.Button upButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ToolStripMenuItem filtryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem prawyEkranToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem wykrywanieKrawedziToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem szkieletToolStripMenuItem;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown highTset;
        private System.Windows.Forms.NumericUpDown lowTset;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label srlabel;
        private System.Windows.Forms.Label rozmm;
        private System.Windows.Forms.NumericUpDown kset;
        private System.Windows.Forms.ComboBox webcamSet;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ToolStripMenuItem zarysujOczkaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem kalibracjaToolStripMenuItem;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown minRSet;
        private System.Windows.Forms.NumericUpDown maxRSet;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.PictureBox webcamPictureBox;
    }
}