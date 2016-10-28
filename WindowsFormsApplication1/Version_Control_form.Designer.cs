namespace easygram
{
    partial class Version_Control_form
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
            this.iTalk_ThemeContainer1 = new iTalk.iTalk_ThemeContainer();
            this.iTalk_HeaderLabel1 = new iTalk.iTalk_HeaderLabel();
            this.iTalk_ProgressIndicator1 = new iTalk.iTalk_ProgressIndicator();
            this.iTalk_ThemeContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // iTalk_ThemeContainer1
            // 
            this.iTalk_ThemeContainer1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(246)))), ((int)(((byte)(246)))));
            this.iTalk_ThemeContainer1.Controls.Add(this.iTalk_HeaderLabel1);
            this.iTalk_ThemeContainer1.Controls.Add(this.iTalk_ProgressIndicator1);
            this.iTalk_ThemeContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.iTalk_ThemeContainer1.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.iTalk_ThemeContainer1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(142)))), ((int)(((byte)(142)))));
            this.iTalk_ThemeContainer1.Location = new System.Drawing.Point(0, 0);
            this.iTalk_ThemeContainer1.Name = "iTalk_ThemeContainer1";
            this.iTalk_ThemeContainer1.Padding = new System.Windows.Forms.Padding(3, 28, 3, 28);
            this.iTalk_ThemeContainer1.Sizable = true;
            this.iTalk_ThemeContainer1.Size = new System.Drawing.Size(284, 261);
            this.iTalk_ThemeContainer1.SmartBounds = false;
            this.iTalk_ThemeContainer1.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultLocation;
            this.iTalk_ThemeContainer1.TabIndex = 1;
            this.iTalk_ThemeContainer1.Text = "이지그램 업데이트";
            // 
            // iTalk_HeaderLabel1
            // 
            this.iTalk_HeaderLabel1.AutoSize = true;
            this.iTalk_HeaderLabel1.BackColor = System.Drawing.Color.Transparent;
            this.iTalk_HeaderLabel1.Font = new System.Drawing.Font("Segoe UI", 25F);
            this.iTalk_HeaderLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.iTalk_HeaderLabel1.Location = new System.Drawing.Point(2, 174);
            this.iTalk_HeaderLabel1.Name = "iTalk_HeaderLabel1";
            this.iTalk_HeaderLabel1.Size = new System.Drawing.Size(276, 46);
            this.iTalk_HeaderLabel1.TabIndex = 1;
            this.iTalk_HeaderLabel1.Text = "업데이트 확인 중";
            // 
            // iTalk_ProgressIndicator1
            // 
            this.iTalk_ProgressIndicator1.Location = new System.Drawing.Point(74, 40);
            this.iTalk_ProgressIndicator1.MinimumSize = new System.Drawing.Size(80, 80);
            this.iTalk_ProgressIndicator1.Name = "iTalk_ProgressIndicator1";
            this.iTalk_ProgressIndicator1.P_AnimationColor = System.Drawing.Color.DimGray;
            this.iTalk_ProgressIndicator1.P_AnimationSpeed = 100;
            this.iTalk_ProgressIndicator1.P_BaseColor = System.Drawing.Color.DarkGray;
            this.iTalk_ProgressIndicator1.Size = new System.Drawing.Size(131, 131);
            this.iTalk_ProgressIndicator1.TabIndex = 0;
            this.iTalk_ProgressIndicator1.Text = "iTalk_ProgressIndicator1";
            // 
            // Version_Control_form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.iTalk_ThemeContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MinimumSize = new System.Drawing.Size(126, 39);
            this.Name = "Version_Control_form";
            this.Text = "이지그램 업데이트";
            this.TransparencyKey = System.Drawing.Color.Fuchsia;
            this.iTalk_ThemeContainer1.ResumeLayout(false);
            this.iTalk_ThemeContainer1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private iTalk.iTalk_ProgressIndicator iTalk_ProgressIndicator1;
        private iTalk.iTalk_ThemeContainer iTalk_ThemeContainer1;
        private iTalk.iTalk_HeaderLabel iTalk_HeaderLabel1;
    }
}