namespace DesktopApp
{
    partial class AppUI
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AppUI));
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.labelIP = new System.Windows.Forms.Label();
            this.btnListen = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxPort = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.comboBoxSpeakers = new System.Windows.Forms.ComboBox();
            this.videoView1 = new LibVLCSharp.WinForms.VideoView();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.textBoxSuffix = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.textBoxType = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            ((System.ComponentModel.ISupportInitialize)(this.videoView1)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.HotTrack;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(225, 10);
            this.panel1.TabIndex = 0;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(134, 309);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(47, 24);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Quit";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.button1_Click);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "notifyIcon1";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.Click += new System.EventHandler(this.notifyIcon1_Click);
            // 
            // labelIP
            // 
            this.labelIP.BackColor = System.Drawing.SystemColors.Control;
            this.labelIP.Cursor = System.Windows.Forms.Cursors.Default;
            this.labelIP.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.labelIP.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.labelIP.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelIP.Location = new System.Drawing.Point(10, 32);
            this.labelIP.Margin = new System.Windows.Forms.Padding(0);
            this.labelIP.Name = "labelIP";
            this.labelIP.Size = new System.Drawing.Size(171, 35);
            this.labelIP.TabIndex = 2;
            this.labelIP.Text = "999.999.999.999";
            this.labelIP.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnListen
            // 
            this.btnListen.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnListen.Location = new System.Drawing.Point(15, 238);
            this.btnListen.Name = "btnListen";
            this.btnListen.Size = new System.Drawing.Size(166, 65);
            this.btnListen.TabIndex = 3;
            this.btnListen.Text = "Start";
            this.btnListen.UseVisualStyleBackColor = true;
            this.btnListen.Click += new System.EventHandler(this.btnListen_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(122, 15);
            this.label1.TabIndex = 4;
            this.label1.Text = "Computer IP Address:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 58);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 15);
            this.label3.TabIndex = 7;
            this.label3.Text = "Port:";
            // 
            // textBoxPort
            // 
            this.textBoxPort.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.textBoxPort.Location = new System.Drawing.Point(3, 76);
            this.textBoxPort.MaxLength = 6;
            this.textBoxPort.Name = "textBoxPort";
            this.textBoxPort.Size = new System.Drawing.Size(121, 33);
            this.textBoxPort.TabIndex = 6;
            this.textBoxPort.Text = "8080";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(102, 15);
            this.label2.TabIndex = 4;
            this.label2.Text = "Phone IP Address:";
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.textBox1.Location = new System.Drawing.Point(3, 22);
            this.textBox1.MaxLength = 15;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(166, 33);
            this.textBox1.TabIndex = 6;
            this.textBox1.Text = "127.0.0.1";
            // 
            // comboBoxSpeakers
            // 
            this.comboBoxSpeakers.FormattingEnabled = true;
            this.comboBoxSpeakers.Location = new System.Drawing.Point(3, 21);
            this.comboBoxSpeakers.Name = "comboBoxSpeakers";
            this.comboBoxSpeakers.Size = new System.Drawing.Size(165, 23);
            this.comboBoxSpeakers.TabIndex = 8;
            // 
            // videoView1
            // 
            this.videoView1.BackColor = System.Drawing.Color.Black;
            this.videoView1.Location = new System.Drawing.Point(3, 18);
            this.videoView1.MediaPlayer = null;
            this.videoView1.Name = "videoView1";
            this.videoView1.Size = new System.Drawing.Size(183, 103);
            this.videoView1.TabIndex = 9;
            this.videoView1.Text = "videoView1";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(0, 66);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(197, 166);
            this.tabControl1.TabIndex = 10;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.Transparent;
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.textBox1);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.textBoxPort);
            this.tabPage1.Location = new System.Drawing.Point(4, 24);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(189, 138);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Basic";
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.Transparent;
            this.tabPage2.Controls.Add(this.textBoxSuffix);
            this.tabPage2.Controls.Add(this.label8);
            this.tabPage2.Controls.Add(this.textBoxType);
            this.tabPage2.Controls.Add(this.label7);
            this.tabPage2.Controls.Add(this.label6);
            this.tabPage2.Controls.Add(this.comboBoxSpeakers);
            this.tabPage2.Location = new System.Drawing.Point(4, 24);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(189, 138);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Advanced";
            // 
            // textBoxSuffix
            // 
            this.textBoxSuffix.Location = new System.Drawing.Point(3, 109);
            this.textBoxSuffix.Name = "textBoxSuffix";
            this.textBoxSuffix.Size = new System.Drawing.Size(141, 23);
            this.textBoxSuffix.TabIndex = 15;
            this.textBoxSuffix.Text = "h264_pcm.sdp";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 91);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(64, 15);
            this.label8.TabIndex = 14;
            this.label8.Text = "URL Suffix:";
            // 
            // textBoxType
            // 
            this.textBoxType.Location = new System.Drawing.Point(3, 65);
            this.textBoxType.Name = "textBoxType";
            this.textBoxType.Size = new System.Drawing.Size(141, 23);
            this.textBoxType.TabIndex = 13;
            this.textBoxType.Text = "rtsp";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 47);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(95, 15);
            this.label7.TabIndex = 12;
            this.label7.Text = "Stream Protocol:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 3);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(56, 15);
            this.label6.TabIndex = 11;
            this.label6.Text = "Speakers:";
            // 
            // tabPage3
            // 
            this.tabPage3.BackColor = System.Drawing.Color.Transparent;
            this.tabPage3.Controls.Add(this.videoView1);
            this.tabPage3.Location = new System.Drawing.Point(4, 24);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(189, 138);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Preview";
            // 
            // AppUI
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(197, 340);
            this.ControlBox = false;
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnListen);
            this.Controls.Add(this.labelIP);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.panel1);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AppUI";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "AppUI";
            this.TopMost = true;
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.Deactivate += new System.EventHandler(this.Form1_Deactivate);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.videoView1)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label labelIP;
        private System.Windows.Forms.Button btnListen;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxPort;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ComboBox comboBoxSpeakers;
        private LibVLCSharp.WinForms.VideoView videoView1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxSuffix;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBoxType;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TabPage tabPage3;
    }
}

