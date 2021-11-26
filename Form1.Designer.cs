
namespace ScreenCapt
{
    partial class Form1
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lbl_fps = new System.Windows.Forms.Label();
            this.btn_getReg = new System.Windows.Forms.Button();
            this.rbtn_ch = new System.Windows.Forms.RadioButton();
            this.rbtn_ft = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.lbl_status = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(19, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(96, 64);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "FPS:";
            // 
            // lbl_fps
            // 
            this.lbl_fps.AutoSize = true;
            this.lbl_fps.Location = new System.Drawing.Point(49, 15);
            this.lbl_fps.Name = "lbl_fps";
            this.lbl_fps.Size = new System.Drawing.Size(13, 13);
            this.lbl_fps.TabIndex = 2;
            this.lbl_fps.Text = "0";
            // 
            // btn_getReg
            // 
            this.btn_getReg.Location = new System.Drawing.Point(133, 52);
            this.btn_getReg.Name = "btn_getReg";
            this.btn_getReg.Size = new System.Drawing.Size(83, 24);
            this.btn_getReg.TabIndex = 3;
            this.btn_getReg.Text = "Get Region";
            this.btn_getReg.UseVisualStyleBackColor = true;
            this.btn_getReg.Click += new System.EventHandler(this.btn_getReg_Click);
            // 
            // rbtn_ch
            // 
            this.rbtn_ch.AutoSize = true;
            this.rbtn_ch.Location = new System.Drawing.Point(133, 6);
            this.rbtn_ch.Name = "rbtn_ch";
            this.rbtn_ch.Size = new System.Drawing.Size(58, 17);
            this.rbtn_ch.TabIndex = 4;
            this.rbtn_ch.Text = "CH341";
            this.rbtn_ch.UseVisualStyleBackColor = true;
            // 
            // rbtn_ft
            // 
            this.rbtn_ft.AutoSize = true;
            this.rbtn_ft.Checked = true;
            this.rbtn_ft.Location = new System.Drawing.Point(133, 29);
            this.rbtn_ft.Name = "rbtn_ft";
            this.rbtn_ft.Size = new System.Drawing.Size(70, 17);
            this.rbtn_ft.TabIndex = 5;
            this.rbtn_ft.TabStop = true;
            this.rbtn_ft.Text = "FT232RL";
            this.rbtn_ft.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 76);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "oled status:";
            // 
            // lbl_status
            // 
            this.lbl_status.AutoSize = true;
            this.lbl_status.Location = new System.Drawing.Point(74, 76);
            this.lbl_status.Name = "lbl_status";
            this.lbl_status.Size = new System.Drawing.Size(29, 13);
            this.lbl_status.TabIndex = 7;
            this.lbl_status.Text = "false";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(223, 98);
            this.Controls.Add(this.lbl_status);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.rbtn_ft);
            this.Controls.Add(this.rbtn_ch);
            this.Controls.Add(this.btn_getReg);
            this.Controls.Add(this.lbl_fps);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximumSize = new System.Drawing.Size(239, 137);
            this.MinimumSize = new System.Drawing.Size(239, 137);
            this.Name = "Form1";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Oled";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbl_fps;
        private System.Windows.Forms.Button btn_getReg;
        private System.Windows.Forms.RadioButton rbtn_ch;
        private System.Windows.Forms.RadioButton rbtn_ft;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbl_status;
    }
}

