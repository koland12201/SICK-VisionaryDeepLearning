namespace WindowsFormsApp1
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.button_Save_RGB = new System.Windows.Forms.Button();
            this.textBox_Index = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button_Save_depth = new System.Windows.Forms.Button();
            this.pictureBox_Mixed = new System.Windows.Forms.PictureBox();
            this.button_Save_mixed = new System.Windows.Forms.Button();
            this.button_Save_all = new System.Windows.Forms.Button();
            this.checkBox_MinAreaRect = new System.Windows.Forms.CheckBox();
            this.listBox_BoxList = new System.Windows.Forms.ListBox();
            this.checkBox_UseBackend = new System.Windows.Forms.CheckBox();
            this.textBox_DynamicRange = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox_ZmapOffset = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.button_SaveResult = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.textBox_MinPixelArea = new System.Windows.Forms.TextBox();
            this.checkBox_AngleCorr = new System.Windows.Forms.CheckBox();
            this.checkBox_RGBAsZmap = new System.Windows.Forms.CheckBox();
            this.trackBar_RatioFilter = new System.Windows.Forms.TrackBar();
            this.label7 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox_BackendPort = new System.Windows.Forms.TextBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.label13 = new System.Windows.Forms.Label();
            this.textBox_Rounding = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.button_AutoCali = new System.Windows.Forms.Button();
            this.label15 = new System.Windows.Forms.Label();
            this.textBox_BackgroundH = new System.Windows.Forms.TextBox();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.label12 = new System.Windows.Forms.Label();
            this.textBox_Savepath = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.textBox_IP = new System.Windows.Forms.TextBox();
            this.button_Connect = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.trackBar_ROIy = new System.Windows.Forms.TrackBar();
            this.trackBar_ROIScale = new System.Windows.Forms.TrackBar();
            this.trackBar_ROIx = new System.Windows.Forms.TrackBar();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Mixed)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_RatioFilter)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_ROIy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_ROIScale)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_ROIx)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(130, 95);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "label1";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.Control;
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(496, 426);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.SystemColors.Control;
            this.pictureBox2.Location = new System.Drawing.Point(524, 12);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(640, 512);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 2;
            this.pictureBox2.TabStop = false;
            // 
            // button_Save_RGB
            // 
            this.button_Save_RGB.Location = new System.Drawing.Point(96, 47);
            this.button_Save_RGB.Name = "button_Save_RGB";
            this.button_Save_RGB.Size = new System.Drawing.Size(75, 23);
            this.button_Save_RGB.TabIndex = 3;
            this.button_Save_RGB.Text = "Save RGB";
            this.button_Save_RGB.UseVisualStyleBackColor = true;
            this.button_Save_RGB.Click += new System.EventHandler(this.button_Save_Click);
            // 
            // textBox_Index
            // 
            this.textBox_Index.Location = new System.Drawing.Point(50, 19);
            this.textBox_Index.Name = "textBox_Index";
            this.textBox_Index.Size = new System.Drawing.Size(75, 22);
            this.textBox_Index.TabIndex = 4;
            this.textBox_Index.Text = "0";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(131, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "File Index (.png)";
            // 
            // button_Save_depth
            // 
            this.button_Save_depth.Location = new System.Drawing.Point(15, 47);
            this.button_Save_depth.Name = "button_Save_depth";
            this.button_Save_depth.Size = new System.Drawing.Size(75, 23);
            this.button_Save_depth.TabIndex = 9;
            this.button_Save_depth.Text = "Save Zmap";
            this.button_Save_depth.UseVisualStyleBackColor = true;
            this.button_Save_depth.Click += new System.EventHandler(this.button_Save_depth_Click);
            // 
            // pictureBox_Mixed
            // 
            this.pictureBox_Mixed.BackColor = System.Drawing.SystemColors.Control;
            this.pictureBox_Mixed.Location = new System.Drawing.Point(12, 444);
            this.pictureBox_Mixed.Name = "pictureBox_Mixed";
            this.pictureBox_Mixed.Size = new System.Drawing.Size(496, 426);
            this.pictureBox_Mixed.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_Mixed.TabIndex = 10;
            this.pictureBox_Mixed.TabStop = false;
            // 
            // button_Save_mixed
            // 
            this.button_Save_mixed.Location = new System.Drawing.Point(15, 74);
            this.button_Save_mixed.Name = "button_Save_mixed";
            this.button_Save_mixed.Size = new System.Drawing.Size(75, 23);
            this.button_Save_mixed.TabIndex = 11;
            this.button_Save_mixed.Text = "Save Mixed";
            this.button_Save_mixed.UseVisualStyleBackColor = true;
            this.button_Save_mixed.Click += new System.EventHandler(this.button_Save_mixed_Click);
            // 
            // button_Save_all
            // 
            this.button_Save_all.Location = new System.Drawing.Point(96, 74);
            this.button_Save_all.Name = "button_Save_all";
            this.button_Save_all.Size = new System.Drawing.Size(75, 23);
            this.button_Save_all.TabIndex = 12;
            this.button_Save_all.Text = "Save All";
            this.button_Save_all.UseVisualStyleBackColor = true;
            this.button_Save_all.Click += new System.EventHandler(this.button_Save_all_Click);
            // 
            // checkBox_MinAreaRect
            // 
            this.checkBox_MinAreaRect.AutoSize = true;
            this.checkBox_MinAreaRect.Location = new System.Drawing.Point(6, 6);
            this.checkBox_MinAreaRect.Name = "checkBox_MinAreaRect";
            this.checkBox_MinAreaRect.Size = new System.Drawing.Size(294, 16);
            this.checkBox_MinAreaRect.TabIndex = 16;
            this.checkBox_MinAreaRect.Text = "locate boxes (Zmap, locate box via contour MinAreaRect)";
            this.checkBox_MinAreaRect.UseVisualStyleBackColor = true;
            this.checkBox_MinAreaRect.CheckedChanged += new System.EventHandler(this.checkBox_MinAreaRect_CheckedChanged);
            // 
            // listBox_BoxList
            // 
            this.listBox_BoxList.Font = new System.Drawing.Font("Arial", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listBox_BoxList.FormattingEnabled = true;
            this.listBox_BoxList.ItemHeight = 32;
            this.listBox_BoxList.Location = new System.Drawing.Point(524, 581);
            this.listBox_BoxList.Name = "listBox_BoxList";
            this.listBox_BoxList.Size = new System.Drawing.Size(858, 196);
            this.listBox_BoxList.TabIndex = 17;
            // 
            // checkBox_UseBackend
            // 
            this.checkBox_UseBackend.AutoSize = true;
            this.checkBox_UseBackend.Location = new System.Drawing.Point(15, 9);
            this.checkBox_UseBackend.Name = "checkBox_UseBackend";
            this.checkBox_UseBackend.Size = new System.Drawing.Size(108, 16);
            this.checkBox_UseBackend.TabIndex = 18;
            this.checkBox_UseBackend.Text = "Use Backend ROI";
            this.checkBox_UseBackend.UseVisualStyleBackColor = true;
            // 
            // textBox_DynamicRange
            // 
            this.textBox_DynamicRange.Location = new System.Drawing.Point(8, 10);
            this.textBox_DynamicRange.Name = "textBox_DynamicRange";
            this.textBox_DynamicRange.Size = new System.Drawing.Size(116, 22);
            this.textBox_DynamicRange.TabIndex = 19;
            this.textBox_DynamicRange.Text = "25000";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(130, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(110, 12);
            this.label4.TabIndex = 20;
            this.label4.Text = "Zmap Dynamic Range";
            // 
            // textBox_ZmapOffset
            // 
            this.textBox_ZmapOffset.Location = new System.Drawing.Point(8, 40);
            this.textBox_ZmapOffset.Name = "textBox_ZmapOffset";
            this.textBox_ZmapOffset.Size = new System.Drawing.Size(116, 22);
            this.textBox_ZmapOffset.TabIndex = 21;
            this.textBox_ZmapOffset.Text = "0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(130, 43);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 12);
            this.label5.TabIndex = 22;
            this.label5.Text = "Zmap Offset";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Location = new System.Drawing.Point(524, 794);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(433, 140);
            this.tabControl1.TabIndex = 23;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.button_SaveResult);
            this.tabPage1.Controls.Add(this.label11);
            this.tabPage1.Controls.Add(this.textBox_MinPixelArea);
            this.tabPage1.Controls.Add(this.checkBox_AngleCorr);
            this.tabPage1.Controls.Add(this.checkBox_RGBAsZmap);
            this.tabPage1.Controls.Add(this.trackBar_RatioFilter);
            this.tabPage1.Controls.Add(this.label7);
            this.tabPage1.Controls.Add(this.checkBox_MinAreaRect);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(425, 114);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Box Detection Algorithm";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // button_SaveResult
            // 
            this.button_SaveResult.Location = new System.Drawing.Point(346, 6);
            this.button_SaveResult.Name = "button_SaveResult";
            this.button_SaveResult.Size = new System.Drawing.Size(73, 23);
            this.button_SaveResult.TabIndex = 47;
            this.button_SaveResult.Text = "Save Result";
            this.button_SaveResult.UseVisualStyleBackColor = true;
            this.button_SaveResult.Click += new System.EventHandler(this.button_SaveResult_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(65, 53);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(97, 12);
            this.label11.TabIndex = 45;
            this.label11.Text = "Min pixel area filter";
            // 
            // textBox_MinPixelArea
            // 
            this.textBox_MinPixelArea.Enabled = false;
            this.textBox_MinPixelArea.Location = new System.Drawing.Point(8, 50);
            this.textBox_MinPixelArea.Name = "textBox_MinPixelArea";
            this.textBox_MinPixelArea.Size = new System.Drawing.Size(51, 22);
            this.textBox_MinPixelArea.TabIndex = 44;
            this.textBox_MinPixelArea.Text = "2000";
            // 
            // checkBox_AngleCorr
            // 
            this.checkBox_AngleCorr.AutoSize = true;
            this.checkBox_AngleCorr.Checked = true;
            this.checkBox_AngleCorr.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_AngleCorr.Enabled = false;
            this.checkBox_AngleCorr.Location = new System.Drawing.Point(147, 28);
            this.checkBox_AngleCorr.Name = "checkBox_AngleCorr";
            this.checkBox_AngleCorr.Size = new System.Drawing.Size(154, 16);
            this.checkBox_AngleCorr.TabIndex = 43;
            this.checkBox_AngleCorr.Text = "Prespective angle correction";
            this.checkBox_AngleCorr.UseVisualStyleBackColor = true;
            this.checkBox_AngleCorr.CheckedChanged += new System.EventHandler(this.checkBox_AngleCorr_CheckedChanged);
            // 
            // checkBox_RGBAsZmap
            // 
            this.checkBox_RGBAsZmap.AutoSize = true;
            this.checkBox_RGBAsZmap.Enabled = false;
            this.checkBox_RGBAsZmap.Location = new System.Drawing.Point(31, 28);
            this.checkBox_RGBAsZmap.Name = "checkBox_RGBAsZmap";
            this.checkBox_RGBAsZmap.Size = new System.Drawing.Size(110, 16);
            this.checkBox_RGBAsZmap.TabIndex = 20;
            this.checkBox_RGBAsZmap.Text = "Use RGB as Zmap";
            this.checkBox_RGBAsZmap.UseVisualStyleBackColor = true;
            this.checkBox_RGBAsZmap.CheckedChanged += new System.EventHandler(this.checkBox_RGBAsZmap_CheckedChanged);
            // 
            // trackBar_RatioFilter
            // 
            this.trackBar_RatioFilter.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.trackBar_RatioFilter.Enabled = false;
            this.trackBar_RatioFilter.Location = new System.Drawing.Point(90, 81);
            this.trackBar_RatioFilter.Maximum = 20;
            this.trackBar_RatioFilter.Name = "trackBar_RatioFilter";
            this.trackBar_RatioFilter.Size = new System.Drawing.Size(305, 45);
            this.trackBar_RatioFilter.TabIndex = 19;
            this.trackBar_RatioFilter.Value = 10;
            this.trackBar_RatioFilter.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 81);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(87, 12);
            this.label7.TabIndex = 18;
            this.label7.Text = "Ratio Filter (0-1):";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.label9);
            this.tabPage2.Controls.Add(this.label8);
            this.tabPage2.Controls.Add(this.label6);
            this.tabPage2.Controls.Add(this.textBox_BackendPort);
            this.tabPage2.Controls.Add(this.checkBox_UseBackend);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(425, 114);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Backend Detection";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.Color.Red;
            this.label9.Location = new System.Drawing.Point(18, 80);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(0, 12);
            this.label9.TabIndex = 27;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.Color.Red;
            this.label8.Location = new System.Drawing.Point(6, 92);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(285, 12);
            this.label8.TabIndex = 26;
            this.label8.Text = "Send \"REQUEST\\n\"  after connecting to request for a frame";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(313, 10);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(27, 12);
            this.label6.TabIndex = 25;
            this.label6.Text = "Port:";
            // 
            // textBox_BackendPort
            // 
            this.textBox_BackendPort.Location = new System.Drawing.Point(346, 6);
            this.textBox_BackendPort.Name = "textBox_BackendPort";
            this.textBox_BackendPort.Size = new System.Drawing.Size(49, 22);
            this.textBox_BackendPort.TabIndex = 24;
            this.textBox_BackendPort.Text = "12201";
            this.textBox_BackendPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.label13);
            this.tabPage3.Controls.Add(this.textBox_Rounding);
            this.tabPage3.Controls.Add(this.label16);
            this.tabPage3.Controls.Add(this.button_AutoCali);
            this.tabPage3.Controls.Add(this.label15);
            this.tabPage3.Controls.Add(this.textBox_BackgroundH);
            this.tabPage3.Controls.Add(this.textBox_ZmapOffset);
            this.tabPage3.Controls.Add(this.textBox_DynamicRange);
            this.tabPage3.Controls.Add(this.label4);
            this.tabPage3.Controls.Add(this.label5);
            this.tabPage3.Controls.Add(this.label1);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(425, 114);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Settings";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(306, 16);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(116, 12);
            this.label13.TabIndex = 47;
            this.label13.Text = "Detection step size(mm)";
            // 
            // textBox_Rounding
            // 
            this.textBox_Rounding.Location = new System.Drawing.Point(246, 10);
            this.textBox_Rounding.Name = "textBox_Rounding";
            this.textBox_Rounding.Size = new System.Drawing.Size(54, 22);
            this.textBox_Rounding.TabIndex = 46;
            this.textBox_Rounding.Text = "1";
            this.textBox_Rounding.TextChanged += new System.EventHandler(this.textBox_Rounding_TextChanged);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(7, 95);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(117, 12);
            this.label16.TabIndex = 27;
            this.label16.Text = "Relative height (center):";
            // 
            // button_AutoCali
            // 
            this.button_AutoCali.Location = new System.Drawing.Point(332, 40);
            this.button_AutoCali.Name = "button_AutoCali";
            this.button_AutoCali.Size = new System.Drawing.Size(90, 23);
            this.button_AutoCali.TabIndex = 26;
            this.button_AutoCali.Text = "Auto calibration";
            this.button_AutoCali.UseVisualStyleBackColor = true;
            this.button_AutoCali.Click += new System.EventHandler(this.button_AutoCali_Click);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(130, 71);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(115, 12);
            this.label15.TabIndex = 25;
            this.label15.Text = "Background height (m)";
            // 
            // textBox_BackgroundH
            // 
            this.textBox_BackgroundH.Location = new System.Drawing.Point(8, 68);
            this.textBox_BackgroundH.Name = "textBox_BackgroundH";
            this.textBox_BackgroundH.Size = new System.Drawing.Size(116, 22);
            this.textBox_BackgroundH.TabIndex = 23;
            this.textBox_BackgroundH.Text = "1.5";
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.label12);
            this.tabPage4.Controls.Add(this.textBox_Savepath);
            this.tabPage4.Controls.Add(this.textBox_Index);
            this.tabPage4.Controls.Add(this.label2);
            this.tabPage4.Controls.Add(this.button_Save_all);
            this.tabPage4.Controls.Add(this.button_Save_RGB);
            this.tabPage4.Controls.Add(this.button_Save_depth);
            this.tabPage4.Controls.Add(this.button_Save_mixed);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(425, 114);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Save Image";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(386, 52);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(25, 12);
            this.label12.TabIndex = 46;
            this.label12.Text = "Path";
            // 
            // textBox_Savepath
            // 
            this.textBox_Savepath.Location = new System.Drawing.Point(189, 49);
            this.textBox_Savepath.Name = "textBox_Savepath";
            this.textBox_Savepath.Size = new System.Drawing.Size(191, 22);
            this.textBox_Savepath.TabIndex = 13;
            this.textBox_Savepath.Text = "C:\\";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(1263, 12);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(47, 12);
            this.label10.TabIndex = 35;
            this.label10.Text = "ROI Size";
            // 
            // textBox_IP
            // 
            this.textBox_IP.Location = new System.Drawing.Point(1192, 912);
            this.textBox_IP.Name = "textBox_IP";
            this.textBox_IP.Size = new System.Drawing.Size(98, 22);
            this.textBox_IP.TabIndex = 13;
            this.textBox_IP.Text = "192.168.1.10";
            this.textBox_IP.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // button_Connect
            // 
            this.button_Connect.Location = new System.Drawing.Point(1302, 911);
            this.button_Connect.Name = "button_Connect";
            this.button_Connect.Size = new System.Drawing.Size(75, 23);
            this.button_Connect.TabIndex = 14;
            this.button_Connect.Text = "Connect";
            this.button_Connect.UseVisualStyleBackColor = true;
            this.button_Connect.Click += new System.EventHandler(this.button_Connect_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(1168, 915);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(18, 12);
            this.label3.TabIndex = 15;
            this.label3.Text = "IP:";
            // 
            // trackBar_ROIy
            // 
            this.trackBar_ROIy.Enabled = false;
            this.trackBar_ROIy.LargeChange = 10;
            this.trackBar_ROIy.Location = new System.Drawing.Point(1169, 12);
            this.trackBar_ROIy.Maximum = 512;
            this.trackBar_ROIy.Name = "trackBar_ROIy";
            this.trackBar_ROIy.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackBar_ROIy.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.trackBar_ROIy.Size = new System.Drawing.Size(45, 526);
            this.trackBar_ROIy.TabIndex = 43;
            this.trackBar_ROIy.Value = 512;
            this.trackBar_ROIy.Scroll += new System.EventHandler(this.trackBar_ROIy_Scroll);
            // 
            // trackBar_ROIScale
            // 
            this.trackBar_ROIScale.Enabled = false;
            this.trackBar_ROIScale.LargeChange = 3;
            this.trackBar_ROIScale.Location = new System.Drawing.Point(1220, 27);
            this.trackBar_ROIScale.Minimum = 1;
            this.trackBar_ROIScale.Name = "trackBar_ROIScale";
            this.trackBar_ROIScale.Size = new System.Drawing.Size(157, 45);
            this.trackBar_ROIScale.TabIndex = 44;
            this.trackBar_ROIScale.Value = 10;
            this.trackBar_ROIScale.Scroll += new System.EventHandler(this.trackBar_ROIScale_Scroll);
            // 
            // trackBar_ROIx
            // 
            this.trackBar_ROIx.Enabled = false;
            this.trackBar_ROIx.LargeChange = 10;
            this.trackBar_ROIx.Location = new System.Drawing.Point(514, 530);
            this.trackBar_ROIx.Maximum = 640;
            this.trackBar_ROIx.Name = "trackBar_ROIx";
            this.trackBar_ROIx.Size = new System.Drawing.Size(650, 45);
            this.trackBar_ROIx.TabIndex = 45;
            this.trackBar_ROIx.Scroll += new System.EventHandler(this.trackBar_ROIx_Scroll);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.ClientSize = new System.Drawing.Size(1394, 946);
            this.Controls.Add(this.trackBar_ROIx);
            this.Controls.Add(this.trackBar_ROIScale);
            this.Controls.Add(this.trackBar_ROIy);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.listBox_BoxList);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button_Connect);
            this.Controls.Add(this.textBox_IP);
            this.Controls.Add(this.pictureBox_Mixed);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.pictureBox1);
            this.Name = "Form1";
            this.Text = "Visionary S Demo- koland";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Mixed)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_RatioFilter)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_ROIy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_ROIScale)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_ROIx)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Button button_Save_RGB;
        private System.Windows.Forms.TextBox textBox_Index;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button_Save_depth;
        private System.Windows.Forms.PictureBox pictureBox_Mixed;
        private System.Windows.Forms.Button button_Save_mixed;
        private System.Windows.Forms.Button button_Save_all;
        private System.Windows.Forms.CheckBox checkBox_MinAreaRect;
        private System.Windows.Forms.ListBox listBox_BoxList;
        private System.Windows.Forms.CheckBox checkBox_UseBackend;
        private System.Windows.Forms.TextBox textBox_DynamicRange;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox_ZmapOffset;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBox_BackendPort;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TrackBar trackBar_RatioFilter;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.CheckBox checkBox_RGBAsZmap;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Button button_AutoCali;
        private System.Windows.Forms.TextBox textBox_BackgroundH;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TextBox textBox_IP;
        private System.Windows.Forms.Button button_Connect;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.CheckBox checkBox_AngleCorr;
        private System.Windows.Forms.TrackBar trackBar_ROIy;
        private System.Windows.Forms.TrackBar trackBar_ROIScale;
        private System.Windows.Forms.TrackBar trackBar_ROIx;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox textBox_MinPixelArea;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox textBox_Savepath;
        private System.Windows.Forms.Button button_SaveResult;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox textBox_Rounding;
    }
}

