namespace RayTracerFramework {
    partial class RayTracerForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.btnRender = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.statusBar = new System.Windows.Forms.StatusStrip();
            this.btnSetup = new System.Windows.Forms.Button();
            this.tbCamPosX = new System.Windows.Forms.TextBox();
            this.tbCamPosY = new System.Windows.Forms.TextBox();
            this.tbCamPosZ = new System.Windows.Forms.TextBox();
            this.tbCamLookAtX = new System.Windows.Forms.TextBox();
            this.tbCamLookAtY = new System.Windows.Forms.TextBox();
            this.tbCamLookAtZ = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox
            // 
            this.pictureBox.BackColor = System.Drawing.SystemColors.ControlDark;
            this.pictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox.Location = new System.Drawing.Point(0, 0);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(422, 270);
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
            // 
            // btnRender
            // 
            this.btnRender.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRender.Location = new System.Drawing.Point(359, 313);
            this.btnRender.Name = "btnRender";
            this.btnRender.Size = new System.Drawing.Size(75, 23);
            this.btnRender.TabIndex = 1;
            this.btnRender.Text = "Setup + R.";
            this.btnRender.UseVisualStyleBackColor = true;
            this.btnRender.Click += new System.EventHandler(this.btnRender_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.pictureBox);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(422, 270);
            this.panel1.TabIndex = 2;
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(12, 317);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(341, 16);
            this.progressBar.TabIndex = 3;
            // 
            // statusBar
            // 
            this.statusBar.Location = new System.Drawing.Point(0, 343);
            this.statusBar.Name = "statusBar";
            this.statusBar.Size = new System.Drawing.Size(446, 22);
            this.statusBar.TabIndex = 4;
            // 
            // btnSetup
            // 
            this.btnSetup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSetup.Location = new System.Drawing.Point(360, 288);
            this.btnSetup.Name = "btnSetup";
            this.btnSetup.Size = new System.Drawing.Size(75, 23);
            this.btnSetup.TabIndex = 5;
            this.btnSetup.Text = "Setup";
            this.btnSetup.UseVisualStyleBackColor = true;
            this.btnSetup.Click += new System.EventHandler(this.btnSetup_Click);
            // 
            // tbCamPosX
            // 
            this.tbCamPosX.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.tbCamPosX.Location = new System.Drawing.Point(67, 291);
            this.tbCamPosX.Name = "tbCamPosX";
            this.tbCamPosX.Size = new System.Drawing.Size(32, 20);
            this.tbCamPosX.TabIndex = 6;
            // 
            // tbCamPosY
            // 
            this.tbCamPosY.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.tbCamPosY.Location = new System.Drawing.Point(105, 291);
            this.tbCamPosY.Name = "tbCamPosY";
            this.tbCamPosY.Size = new System.Drawing.Size(32, 20);
            this.tbCamPosY.TabIndex = 6;
            // 
            // tbCamPosZ
            // 
            this.tbCamPosZ.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.tbCamPosZ.Location = new System.Drawing.Point(143, 291);
            this.tbCamPosZ.Name = "tbCamPosZ";
            this.tbCamPosZ.Size = new System.Drawing.Size(32, 20);
            this.tbCamPosZ.TabIndex = 6;
            // 
            // tbCamLookAtX
            // 
            this.tbCamLookAtX.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.tbCamLookAtX.Location = new System.Drawing.Point(244, 291);
            this.tbCamLookAtX.Name = "tbCamLookAtX";
            this.tbCamLookAtX.Size = new System.Drawing.Size(32, 20);
            this.tbCamLookAtX.TabIndex = 6;
            // 
            // tbCamLookAtY
            // 
            this.tbCamLookAtY.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.tbCamLookAtY.Location = new System.Drawing.Point(282, 291);
            this.tbCamLookAtY.Name = "tbCamLookAtY";
            this.tbCamLookAtY.Size = new System.Drawing.Size(32, 20);
            this.tbCamLookAtY.TabIndex = 6;
            // 
            // tbCamLookAtZ
            // 
            this.tbCamLookAtZ.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.tbCamLookAtZ.Location = new System.Drawing.Point(320, 291);
            this.tbCamLookAtZ.Name = "tbCamLookAtZ";
            this.tbCamLookAtZ.Size = new System.Drawing.Size(32, 20);
            this.tbCamLookAtZ.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 295);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Cam Pos";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(179, 295);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Cam LookAt";
            // 
            // RayTracerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(446, 365);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbCamLookAtZ);
            this.Controls.Add(this.tbCamLookAtY);
            this.Controls.Add(this.tbCamLookAtX);
            this.Controls.Add(this.tbCamPosZ);
            this.Controls.Add(this.tbCamPosY);
            this.Controls.Add(this.tbCamPosX);
            this.Controls.Add(this.btnSetup);
            this.Controls.Add(this.statusBar);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnRender);
            this.Name = "RayTracerForm";
            this.Text = "RayTracer";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Button btnRender;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.StatusStrip statusBar;
        private System.Windows.Forms.Button btnSetup;
        private System.Windows.Forms.TextBox tbCamPosX;
        private System.Windows.Forms.TextBox tbCamPosY;
        private System.Windows.Forms.TextBox tbCamPosZ;
        private System.Windows.Forms.TextBox tbCamLookAtX;
        private System.Windows.Forms.TextBox tbCamLookAtY;
        private System.Windows.Forms.TextBox tbCamLookAtZ;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}

