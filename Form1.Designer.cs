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
            this.button2 = new System.Windows.Forms.Button();
            this.button_Save_depth = new System.Windows.Forms.Button();
            this.pictureBox_Mixed = new System.Windows.Forms.PictureBox();
            this.button_Save_mixed = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Mixed)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "label1";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(157, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(496, 426);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Location = new System.Drawing.Point(678, 12);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(496, 426);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 2;
            this.pictureBox2.TabStop = false;
            // 
            // button_Save_RGB
            // 
            this.button_Save_RGB.Location = new System.Drawing.Point(12, 386);
            this.button_Save_RGB.Name = "button_Save_RGB";
            this.button_Save_RGB.Size = new System.Drawing.Size(75, 23);
            this.button_Save_RGB.TabIndex = 3;
            this.button_Save_RGB.Text = "Save RGB";
            this.button_Save_RGB.UseVisualStyleBackColor = true;
            this.button_Save_RGB.Click += new System.EventHandler(this.button_Save_Click);
            // 
            // textBox_Index
            // 
            this.textBox_Index.Location = new System.Drawing.Point(12, 359);
            this.textBox_Index.Name = "textBox_Index";
            this.textBox_Index.Size = new System.Drawing.Size(100, 22);
            this.textBox_Index.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 344);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "Index";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(14, 52);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 8;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button_Save_depth
            // 
            this.button_Save_depth.Location = new System.Drawing.Point(12, 415);
            this.button_Save_depth.Name = "button_Save_depth";
            this.button_Save_depth.Size = new System.Drawing.Size(75, 23);
            this.button_Save_depth.TabIndex = 9;
            this.button_Save_depth.Text = "Save Dmap";
            this.button_Save_depth.UseVisualStyleBackColor = true;
            this.button_Save_depth.Click += new System.EventHandler(this.button_Save_depth_Click);
            // 
            // pictureBox_Mixed
            // 
            this.pictureBox_Mixed.Location = new System.Drawing.Point(157, 444);
            this.pictureBox_Mixed.Name = "pictureBox_Mixed";
            this.pictureBox_Mixed.Size = new System.Drawing.Size(496, 426);
            this.pictureBox_Mixed.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_Mixed.TabIndex = 10;
            this.pictureBox_Mixed.TabStop = false;
            // 
            // button_Save_mixed
            // 
            this.button_Save_mixed.Location = new System.Drawing.Point(12, 444);
            this.button_Save_mixed.Name = "button_Save_mixed";
            this.button_Save_mixed.Size = new System.Drawing.Size(75, 23);
            this.button_Save_mixed.TabIndex = 11;
            this.button_Save_mixed.Text = "Save Mixed";
            this.button_Save_mixed.UseVisualStyleBackColor = true;
            this.button_Save_mixed.Click += new System.EventHandler(this.button_Save_mixed_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1193, 880);
            this.Controls.Add(this.button_Save_mixed);
            this.Controls.Add(this.pictureBox_Mixed);
            this.Controls.Add(this.button_Save_depth);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox_Index);
            this.Controls.Add(this.button_Save_RGB);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Mixed)).EndInit();
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
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button_Save_depth;
        private System.Windows.Forms.PictureBox pictureBox_Mixed;
        private System.Windows.Forms.Button button_Save_mixed;
    }
}

