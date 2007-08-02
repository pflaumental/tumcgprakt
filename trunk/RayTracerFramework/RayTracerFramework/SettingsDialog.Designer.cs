namespace RayTracerFramework {
    partial class SettingsDialog {
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
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.groupBoxPhotonMapping = new System.Windows.Forms.GroupBox();
            this.labelCollectionRadiusValue = new System.Windows.Forms.Label();
            this.labelCollectionRadius = new System.Windows.Forms.Label();
            this.labelConeFilterConstantKValue = new System.Windows.Forms.Label();
            this.labelConeFilterConstantK = new System.Windows.Forms.Label();
            this.labelPowerLevelValue = new System.Windows.Forms.Label();
            this.labelPowerLevel = new System.Windows.Forms.Label();
            this.labelDiffuseScaledownValue = new System.Windows.Forms.Label();
            this.labelDiffuseScaleDown = new System.Windows.Forms.Label();
            this.collectionRadiusTrackBar = new System.Windows.Forms.TrackBar();
            this.coneFilterConstantKtrackBar = new System.Windows.Forms.TrackBar();
            this.powerLevelTrackBar = new System.Windows.Forms.TrackBar();
            this.diffuseScaleDownTrackBar = new System.Windows.Forms.TrackBar();
            this.labelStoredPhotons = new System.Windows.Forms.Label();
            this.storedPhotonsComboBox = new System.Windows.Forms.ComboBox();
            this.enablePMcheckBox = new System.Windows.Forms.CheckBox();
            this.groupBoxPhotonMapping.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.collectionRadiusTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.coneFilterConstantKtrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.powerLevelTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.diffuseScaleDownTrackBar)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(235, 247);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 0;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(154, 247);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // groupBoxPhotonMapping
            // 
            this.groupBoxPhotonMapping.Controls.Add(this.labelCollectionRadiusValue);
            this.groupBoxPhotonMapping.Controls.Add(this.labelCollectionRadius);
            this.groupBoxPhotonMapping.Controls.Add(this.labelConeFilterConstantKValue);
            this.groupBoxPhotonMapping.Controls.Add(this.labelConeFilterConstantK);
            this.groupBoxPhotonMapping.Controls.Add(this.labelPowerLevelValue);
            this.groupBoxPhotonMapping.Controls.Add(this.labelPowerLevel);
            this.groupBoxPhotonMapping.Controls.Add(this.labelDiffuseScaledownValue);
            this.groupBoxPhotonMapping.Controls.Add(this.labelDiffuseScaleDown);
            this.groupBoxPhotonMapping.Controls.Add(this.collectionRadiusTrackBar);
            this.groupBoxPhotonMapping.Controls.Add(this.coneFilterConstantKtrackBar);
            this.groupBoxPhotonMapping.Controls.Add(this.powerLevelTrackBar);
            this.groupBoxPhotonMapping.Controls.Add(this.diffuseScaleDownTrackBar);
            this.groupBoxPhotonMapping.Controls.Add(this.labelStoredPhotons);
            this.groupBoxPhotonMapping.Controls.Add(this.storedPhotonsComboBox);
            this.groupBoxPhotonMapping.Controls.Add(this.enablePMcheckBox);
            this.groupBoxPhotonMapping.Location = new System.Drawing.Point(12, 12);
            this.groupBoxPhotonMapping.Name = "groupBoxPhotonMapping";
            this.groupBoxPhotonMapping.Size = new System.Drawing.Size(297, 216);
            this.groupBoxPhotonMapping.TabIndex = 2;
            this.groupBoxPhotonMapping.TabStop = false;
            this.groupBoxPhotonMapping.Text = "Photon mapping";
            // 
            // labelCollectionRadiusValue
            // 
            this.labelCollectionRadiusValue.AutoSize = true;
            this.labelCollectionRadiusValue.Location = new System.Drawing.Point(244, 147);
            this.labelCollectionRadiusValue.Name = "labelCollectionRadiusValue";
            this.labelCollectionRadiusValue.Size = new System.Drawing.Size(22, 13);
            this.labelCollectionRadiusValue.TabIndex = 12;
            this.labelCollectionRadiusValue.Text = "0,8";
            // 
            // labelCollectionRadius
            // 
            this.labelCollectionRadius.AutoSize = true;
            this.labelCollectionRadius.Location = new System.Drawing.Point(153, 146);
            this.labelCollectionRadius.Name = "labelCollectionRadius";
            this.labelCollectionRadius.Size = new System.Drawing.Size(84, 13);
            this.labelCollectionRadius.TabIndex = 11;
            this.labelCollectionRadius.Text = "Collection radius";
            // 
            // labelConeFilterConstantKValue
            // 
            this.labelConeFilterConstantKValue.AutoSize = true;
            this.labelConeFilterConstantKValue.Location = new System.Drawing.Point(265, 83);
            this.labelConeFilterConstantKValue.Name = "labelConeFilterConstantKValue";
            this.labelConeFilterConstantKValue.Size = new System.Drawing.Size(22, 13);
            this.labelConeFilterConstantKValue.TabIndex = 10;
            this.labelConeFilterConstantKValue.Text = "1,5";
            // 
            // labelConeFilterConstantK
            // 
            this.labelConeFilterConstantK.AutoSize = true;
            this.labelConeFilterConstantK.Location = new System.Drawing.Point(150, 83);
            this.labelConeFilterConstantK.Name = "labelConeFilterConstantK";
            this.labelConeFilterConstantK.Size = new System.Drawing.Size(108, 13);
            this.labelConeFilterConstantK.TabIndex = 9;
            this.labelConeFilterConstantK.Text = "Cone filter constant K";
            // 
            // labelPowerLevelValue
            // 
            this.labelPowerLevelValue.AutoSize = true;
            this.labelPowerLevelValue.Location = new System.Drawing.Point(111, 147);
            this.labelPowerLevelValue.Name = "labelPowerLevelValue";
            this.labelPowerLevelValue.Size = new System.Drawing.Size(13, 13);
            this.labelPowerLevelValue.TabIndex = 8;
            this.labelPowerLevelValue.Text = "6";
            // 
            // labelPowerLevel
            // 
            this.labelPowerLevel.AutoSize = true;
            this.labelPowerLevel.Location = new System.Drawing.Point(6, 147);
            this.labelPowerLevel.Name = "labelPowerLevel";
            this.labelPowerLevel.Size = new System.Drawing.Size(98, 13);
            this.labelPowerLevel.TabIndex = 7;
            this.labelPowerLevel.Text = "Photon power level";
            // 
            // labelDiffuseScaledownValue
            // 
            this.labelDiffuseScaledownValue.AutoSize = true;
            this.labelDiffuseScaledownValue.Location = new System.Drawing.Point(111, 83);
            this.labelDiffuseScaledownValue.Name = "labelDiffuseScaledownValue";
            this.labelDiffuseScaledownValue.Size = new System.Drawing.Size(22, 13);
            this.labelDiffuseScaledownValue.TabIndex = 6;
            this.labelDiffuseScaledownValue.Text = "0,5";
            // 
            // labelDiffuseScaleDown
            // 
            this.labelDiffuseScaleDown.AutoSize = true;
            this.labelDiffuseScaleDown.Location = new System.Drawing.Point(7, 83);
            this.labelDiffuseScaleDown.Name = "labelDiffuseScaleDown";
            this.labelDiffuseScaleDown.Size = new System.Drawing.Size(97, 13);
            this.labelDiffuseScaleDown.TabIndex = 5;
            this.labelDiffuseScaleDown.Text = "Diffuse scale down";
            // 
            // collectionRadiusTrackBar
            // 
            this.collectionRadiusTrackBar.LargeChange = 1;
            this.collectionRadiusTrackBar.Location = new System.Drawing.Point(149, 162);
            this.collectionRadiusTrackBar.Maximum = 20;
            this.collectionRadiusTrackBar.Minimum = 1;
            this.collectionRadiusTrackBar.Name = "collectionRadiusTrackBar";
            this.collectionRadiusTrackBar.Size = new System.Drawing.Size(138, 45);
            this.collectionRadiusTrackBar.TabIndex = 4;
            this.collectionRadiusTrackBar.Value = 8;
            this.collectionRadiusTrackBar.Scroll += new System.EventHandler(this.collectionRadiusTrackBar_Scroll);
            // 
            // coneFilterConstantKtrackBar
            // 
            this.coneFilterConstantKtrackBar.LargeChange = 1;
            this.coneFilterConstantKtrackBar.Location = new System.Drawing.Point(149, 99);
            this.coneFilterConstantKtrackBar.Maximum = 9;
            this.coneFilterConstantKtrackBar.Minimum = 1;
            this.coneFilterConstantKtrackBar.Name = "coneFilterConstantKtrackBar";
            this.coneFilterConstantKtrackBar.Size = new System.Drawing.Size(138, 45);
            this.coneFilterConstantKtrackBar.TabIndex = 4;
            this.coneFilterConstantKtrackBar.Value = 6;
            this.coneFilterConstantKtrackBar.Scroll += new System.EventHandler(this.coneFilterConstantKtrackBar_Scroll);
            // 
            // powerLevelTrackBar
            // 
            this.powerLevelTrackBar.LargeChange = 1;
            this.powerLevelTrackBar.Location = new System.Drawing.Point(6, 163);
            this.powerLevelTrackBar.Maximum = 9;
            this.powerLevelTrackBar.Minimum = 1;
            this.powerLevelTrackBar.Name = "powerLevelTrackBar";
            this.powerLevelTrackBar.Size = new System.Drawing.Size(138, 45);
            this.powerLevelTrackBar.TabIndex = 4;
            this.powerLevelTrackBar.Value = 6;
            this.powerLevelTrackBar.Scroll += new System.EventHandler(this.powerLevelTrackBar_Scroll);
            // 
            // diffuseScaleDownTrackBar
            // 
            this.diffuseScaleDownTrackBar.LargeChange = 1;
            this.diffuseScaleDownTrackBar.Location = new System.Drawing.Point(6, 99);
            this.diffuseScaleDownTrackBar.Name = "diffuseScaleDownTrackBar";
            this.diffuseScaleDownTrackBar.Size = new System.Drawing.Size(138, 45);
            this.diffuseScaleDownTrackBar.TabIndex = 4;
            this.diffuseScaleDownTrackBar.Value = 5;
            this.diffuseScaleDownTrackBar.Scroll += new System.EventHandler(this.diffuseScaleDownTrackBar_Scroll);
            // 
            // labelStoredPhotons
            // 
            this.labelStoredPhotons.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.labelStoredPhotons.AutoSize = true;
            this.labelStoredPhotons.Location = new System.Drawing.Point(110, 38);
            this.labelStoredPhotons.Name = "labelStoredPhotons";
            this.labelStoredPhotons.Size = new System.Drawing.Size(79, 13);
            this.labelStoredPhotons.TabIndex = 3;
            this.labelStoredPhotons.Text = "Stored photons";
            // 
            // storedPhotonsComboBox
            // 
            this.storedPhotonsComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.storedPhotonsComboBox.DisplayMember = "storedPhotonsCount";
            this.storedPhotonsComboBox.FormattingEnabled = true;
            this.storedPhotonsComboBox.Items.AddRange(new object[] {
            "10000",
            "25000",
            "50000",
            "75000",
            "100000",
            "150000",
            "200000"});
            this.storedPhotonsComboBox.Location = new System.Drawing.Point(110, 54);
            this.storedPhotonsComboBox.Name = "storedPhotonsComboBox";
            this.storedPhotonsComboBox.Size = new System.Drawing.Size(79, 21);
            this.storedPhotonsComboBox.TabIndex = 2;
            this.storedPhotonsComboBox.Text = "150000";
            this.storedPhotonsComboBox.ValueMember = "storedPhotonsCount";
            this.storedPhotonsComboBox.SelectedIndexChanged += new System.EventHandler(this.storedPhotonsComboBox_Changed);
            this.storedPhotonsComboBox.TextUpdate += new System.EventHandler(this.storedPhotonsComboBox_Changed);
            // 
            // enablePMcheckBox
            // 
            this.enablePMcheckBox.AutoSize = true;
            this.enablePMcheckBox.Checked = true;
            this.enablePMcheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.enablePMcheckBox.Location = new System.Drawing.Point(6, 19);
            this.enablePMcheckBox.Name = "enablePMcheckBox";
            this.enablePMcheckBox.Size = new System.Drawing.Size(138, 17);
            this.enablePMcheckBox.TabIndex = 1;
            this.enablePMcheckBox.Text = "Enable photon mapping";
            this.enablePMcheckBox.UseVisualStyleBackColor = true;
            this.enablePMcheckBox.CheckedChanged += new System.EventHandler(this.checkBoxEnablePM_CheckedChanged);
            // 
            // SettingsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(322, 282);
            this.ControlBox = false;
            this.Controls.Add(this.groupBoxPhotonMapping);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsDialog";
            this.ShowInTaskbar = false;
            this.Text = "Settings";
            this.groupBoxPhotonMapping.ResumeLayout(false);
            this.groupBoxPhotonMapping.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.collectionRadiusTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.coneFilterConstantKtrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.powerLevelTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.diffuseScaleDownTrackBar)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.GroupBox groupBoxPhotonMapping;
        private System.Windows.Forms.CheckBox enablePMcheckBox;
        private System.Windows.Forms.Label labelStoredPhotons;
        private System.Windows.Forms.ComboBox storedPhotonsComboBox;
        private System.Windows.Forms.Label labelDiffuseScaleDown;
        private System.Windows.Forms.TrackBar diffuseScaleDownTrackBar;
        private System.Windows.Forms.Label labelDiffuseScaledownValue;
        private System.Windows.Forms.Label labelPowerLevel;
        private System.Windows.Forms.TrackBar powerLevelTrackBar;
        private System.Windows.Forms.Label labelPowerLevelValue;
        private System.Windows.Forms.Label labelConeFilterConstantKValue;
        private System.Windows.Forms.Label labelConeFilterConstantK;
        private System.Windows.Forms.TrackBar coneFilterConstantKtrackBar;
        private System.Windows.Forms.Label labelCollectionRadius;
        private System.Windows.Forms.TrackBar collectionRadiusTrackBar;
        private System.Windows.Forms.Label labelCollectionRadiusValue;
    }
}