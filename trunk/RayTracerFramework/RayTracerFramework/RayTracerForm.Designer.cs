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
            this.renderPanel = new System.Windows.Forms.Panel();
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
            this.mainMenu = new System.Windows.Forms.MenuStrip();
            this.loadMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.renderPanel.SuspendLayout();
            this.mainMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox
            // 
            this.pictureBox.BackColor = System.Drawing.SystemColors.ControlDark;
            this.pictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox.Location = new System.Drawing.Point(0, 0);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(422, 277);
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
            // 
            // btnRender
            // 
            this.btnRender.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRender.Location = new System.Drawing.Point(359, 335);
            this.btnRender.Name = "btnRender";
            this.btnRender.Size = new System.Drawing.Size(75, 23);
            this.btnRender.TabIndex = 1;
            this.btnRender.Text = "Setup + R.";
            this.btnRender.UseVisualStyleBackColor = true;
            this.btnRender.Click += new System.EventHandler(this.btnRender_Click);
            // 
            // renderPanel
            // 
            this.renderPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.renderPanel.AutoScroll = true;
            this.renderPanel.Controls.Add(this.pictureBox);
            this.renderPanel.Location = new System.Drawing.Point(12, 27);
            this.renderPanel.Name = "renderPanel";
            this.renderPanel.Size = new System.Drawing.Size(422, 277);
            this.renderPanel.TabIndex = 2;
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(12, 339);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(341, 16);
            this.progressBar.TabIndex = 3;
            // 
            // statusBar
            // 
            this.statusBar.Location = new System.Drawing.Point(0, 365);
            this.statusBar.Name = "statusBar";
            this.statusBar.Size = new System.Drawing.Size(446, 22);
            this.statusBar.TabIndex = 4;
            // 
            // btnSetup
            // 
            this.btnSetup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSetup.Location = new System.Drawing.Point(360, 310);
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
            this.tbCamPosX.Location = new System.Drawing.Point(67, 313);
            this.tbCamPosX.Name = "tbCamPosX";
            this.tbCamPosX.Size = new System.Drawing.Size(32, 20);
            this.tbCamPosX.TabIndex = 6;
            // 
            // tbCamPosY
            // 
            this.tbCamPosY.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.tbCamPosY.Location = new System.Drawing.Point(105, 313);
            this.tbCamPosY.Name = "tbCamPosY";
            this.tbCamPosY.Size = new System.Drawing.Size(32, 20);
            this.tbCamPosY.TabIndex = 6;
            // 
            // tbCamPosZ
            // 
            this.tbCamPosZ.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.tbCamPosZ.Location = new System.Drawing.Point(143, 313);
            this.tbCamPosZ.Name = "tbCamPosZ";
            this.tbCamPosZ.Size = new System.Drawing.Size(32, 20);
            this.tbCamPosZ.TabIndex = 6;
            // 
            // tbCamLookAtX
            // 
            this.tbCamLookAtX.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.tbCamLookAtX.Location = new System.Drawing.Point(244, 313);
            this.tbCamLookAtX.Name = "tbCamLookAtX";
            this.tbCamLookAtX.Size = new System.Drawing.Size(32, 20);
            this.tbCamLookAtX.TabIndex = 6;
            // 
            // tbCamLookAtY
            // 
            this.tbCamLookAtY.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.tbCamLookAtY.Location = new System.Drawing.Point(282, 313);
            this.tbCamLookAtY.Name = "tbCamLookAtY";
            this.tbCamLookAtY.Size = new System.Drawing.Size(32, 20);
            this.tbCamLookAtY.TabIndex = 6;
            // 
            // tbCamLookAtZ
            // 
            this.tbCamLookAtZ.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.tbCamLookAtZ.Location = new System.Drawing.Point(320, 313);
            this.tbCamLookAtZ.Name = "tbCamLookAtZ";
            this.tbCamLookAtZ.Size = new System.Drawing.Size(32, 20);
            this.tbCamLookAtZ.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 317);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Cam Pos";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(179, 317);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Cam LookAt";
            // 
            // mainMenu
            // 
            this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadMenuItem,
            this.settingsMenuItem});
            this.mainMenu.Location = new System.Drawing.Point(0, 0);
            this.mainMenu.Name = "mainMenu";
            this.mainMenu.Size = new System.Drawing.Size(446, 24);
            this.mainMenu.TabIndex = 8;
            this.mainMenu.Text = "Main menu";
            // 
            // loadMenuItem
            // 
            this.loadMenuItem.Name = "loadMenuItem";
            this.loadMenuItem.Size = new System.Drawing.Size(73, 20);
            this.loadMenuItem.Text = "Load scene";
            this.loadMenuItem.Click += new System.EventHandler(this.loadMenuItem_Click);
            // 
            // settingsMenuItem
            // 
            this.settingsMenuItem.Name = "settingsMenuItem";
            this.settingsMenuItem.Size = new System.Drawing.Size(58, 20);
            this.settingsMenuItem.Text = "Settings";
            this.settingsMenuItem.Click += new System.EventHandler(this.settingsMenuItem_Click);
            // 
            // RayTracerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(446, 387);
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
            this.Controls.Add(this.mainMenu);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.renderPanel);
            this.Controls.Add(this.btnRender);
            this.MainMenuStrip = this.mainMenu;
            this.Name = "RayTracerForm";
            this.Text = "RayTracer";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.renderPanel.ResumeLayout(false);
            this.mainMenu.ResumeLayout(false);
            this.mainMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Button btnRender;
        private System.Windows.Forms.Panel renderPanel;
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
        private System.Windows.Forms.MenuStrip mainMenu;
        private System.Windows.Forms.ToolStripMenuItem loadMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsMenuItem;
    }
}

