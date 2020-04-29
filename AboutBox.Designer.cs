namespace Half_Life_Background_Changer
{
    partial class AboutBox
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutBox));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.memberLink = new System.Windows.Forms.LinkLabel();
            this.divider1 = new CSCSCH.Divider();
            this.uLabel3 = new Half_Life_Background_Changer.ULabel();
            this.uLabel1 = new Half_Life_Background_Changer.ULabel();
            this.uLabel2 = new Half_Life_Background_Changer.ULabel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(333, 96);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(39, 38);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            // 
            // memberLink
            // 
            this.memberLink.AutoSize = true;
            this.memberLink.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.memberLink.LinkColor = System.Drawing.Color.RoyalBlue;
            this.memberLink.Location = new System.Drawing.Point(55, 120);
            this.memberLink.Name = "memberLink";
            this.memberLink.Size = new System.Drawing.Size(268, 13);
            this.memberLink.TabIndex = 5;
            this.memberLink.TabStop = true;
            this.memberLink.Text = "https://gamebanana.com/members/1454821";
            // 
            // divider1
            // 
            this.divider1.BackColor = System.Drawing.Color.Transparent;
            this.divider1.DarkBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.divider1.LightBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.divider1.Location = new System.Drawing.Point(12, 88);
            this.divider1.Name = "divider1";
            this.divider1.Size = new System.Drawing.Size(360, 2);
            this.divider1.TabIndex = 0;
            this.divider1.Text = "divider1";
            // 
            // uLabel3
            // 
            this.uLabel3.BackColor = System.Drawing.Color.Transparent;
            this.uLabel3.ClickedColor = System.Drawing.Color.Orange;
            this.uLabel3.ContentAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.uLabel3.Enabled = false;
            this.uLabel3.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uLabel3.ForeColor = System.Drawing.Color.Orange;
            this.uLabel3.Location = new System.Drawing.Point(207, 96);
            this.uLabel3.Name = "uLabel3";
            this.uLabel3.Padding = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.uLabel3.Size = new System.Drawing.Size(116, 21);
            this.uLabel3.TabIndex = 7;
            this.uLabel3.Text = "Created by Meowth";
            // 
            // uLabel1
            // 
            this.uLabel1.BackColor = System.Drawing.Color.Transparent;
            this.uLabel1.ClickedColor = System.Drawing.Color.Orange;
            this.uLabel1.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.uLabel1.Enabled = false;
            this.uLabel1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uLabel1.ForeColor = System.Drawing.Color.Orange;
            this.uLabel1.Location = new System.Drawing.Point(12, 12);
            this.uLabel1.Name = "uLabel1";
            this.uLabel1.Padding = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.uLabel1.Size = new System.Drawing.Size(256, 30);
            this.uLabel1.TabIndex = 8;
            this.uLabel1.Text = "CS+ Background Changer";
            // 
            // uLabel2
            // 
            this.uLabel2.BackColor = System.Drawing.Color.Transparent;
            this.uLabel2.ClickedColor = System.Drawing.Color.Orange;
            this.uLabel2.ContentAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.uLabel2.Enabled = false;
            this.uLabel2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uLabel2.ForeColor = System.Drawing.Color.DarkOrange;
            this.uLabel2.Location = new System.Drawing.Point(116, 48);
            this.uLabel2.Name = "uLabel2";
            this.uLabel2.Padding = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.uLabel2.Size = new System.Drawing.Size(256, 30);
            this.uLabel2.TabIndex = 9;
            this.uLabel2.Text = "Easily change your CS 1.6 backgrounds and mods beyond!";
            // 
            // AboutBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(23)))), ((int)(((byte)(34)))));
            this.ClientSize = new System.Drawing.Size(384, 147);
            this.Controls.Add(this.uLabel2);
            this.Controls.Add(this.uLabel1);
            this.Controls.Add(this.uLabel3);
            this.Controls.Add(this.memberLink);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.divider1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximumSize = new System.Drawing.Size(400, 186);
            this.Name = "AboutBox";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CSCSCH.Divider divider1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.LinkLabel memberLink;
        private ULabel uLabel3;
        private ULabel uLabel1;
        private ULabel uLabel2;
    }
}