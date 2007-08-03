using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace RayTracerFramework {
    public partial class SettingsDialog : Form {
        public bool enablePhotonMapping;
        public int storedPhotonsCount;
        public float diffuseScaleDown;
        public float photonPowerLevel;
        public float photonCollectionRadius;
        public float coneFilterConstantK;

        public bool oldEnablePhotonMapping;
        public int oldStoredPhotonsCount;
        public float oldDiffuseScaleDown;
        public float oldPhotonPowerLevel;
        public float oldPhotonCollectionRadius;
        public float oldConeFilterConstantK;

        public SettingsDialog(
                bool enablePhotonMapping,
                int storedPhotonsCount,
                float diffuseScaleDown,
                float photonPowerLevel,
                float photonCollectionRadius,
                float coneFilterConstantK) {
            oldEnablePhotonMapping = this.enablePhotonMapping = enablePhotonMapping;
            oldStoredPhotonsCount = this.storedPhotonsCount = storedPhotonsCount;
            oldDiffuseScaleDown = this.diffuseScaleDown = diffuseScaleDown;
            oldPhotonPowerLevel = this.photonPowerLevel = photonPowerLevel;
            oldPhotonCollectionRadius = this.photonCollectionRadius = photonCollectionRadius;
            oldConeFilterConstantK = this.coneFilterConstantK = coneFilterConstantK;

            InitializeComponent();

            UpdateControls();
        }

        private void checkBoxEnablePM_CheckedChanged(object sender, EventArgs e) {
            enablePhotonMapping = enablePMcheckBox.Checked;
            storedPhotonsComboBox.Enabled = enablePMcheckBox.Checked;
            diffuseScaleDownTrackBar.Enabled = enablePMcheckBox.Checked;
            diffuseScaleDownTrackBar.Enabled = enablePMcheckBox.Checked;
            powerLevelTrackBar.Enabled = enablePMcheckBox.Checked;
            collectionRadiusTrackBar.Enabled = enablePMcheckBox.Checked;
            coneFilterConstantKtrackBar.Enabled = enablePMcheckBox.Checked;
        }

        private void storedPhotonsComboBox_Changed(object sender, EventArgs e) {
            storedPhotonsCount = int.Parse(storedPhotonsComboBox.Text);
        }

        private void diffuseScaleDownTrackBar_Scroll(object sender, EventArgs e) {
            diffuseScaleDown = diffuseScaleDownTrackBar.Value / 10f;
            UpdateControls();
        }

        private void powerLevelTrackBar_Scroll(object sender, EventArgs e) {
            photonPowerLevel = (float)powerLevelTrackBar.Value;
            UpdateControls();
        }

        private void coneFilterConstantKtrackBar_Scroll(object sender, EventArgs e) {
            coneFilterConstantK = coneFilterConstantKtrackBar.Value / 10f + 1f;
            UpdateControls();
        }

        private void collectionRadiusTrackBar_Scroll(object sender, EventArgs e) {
            photonCollectionRadius = collectionRadiusTrackBar.Value / 10f;
            UpdateControls();
        }

        private void buttonCancel_Click(object sender, EventArgs e) {
            enablePhotonMapping = oldEnablePhotonMapping;
            storedPhotonsCount = oldStoredPhotonsCount;
            diffuseScaleDown = oldDiffuseScaleDown;
            photonPowerLevel = oldPhotonPowerLevel;
            photonCollectionRadius = oldPhotonCollectionRadius;
            coneFilterConstantK = oldConeFilterConstantK;
            UpdateControls();
        }

        private void buttonOK_Click(object sender, EventArgs e) {
            oldEnablePhotonMapping = enablePhotonMapping;
            oldStoredPhotonsCount = storedPhotonsCount;
            oldDiffuseScaleDown = diffuseScaleDown;
            oldPhotonPowerLevel = photonPowerLevel;
            oldPhotonCollectionRadius = photonCollectionRadius;
            oldConeFilterConstantK = coneFilterConstantK;
        }

        private void UpdateControls() {
       
            storedPhotonsComboBox.Text = storedPhotonsCount.ToString();
            diffuseScaleDownTrackBar.Value = (int) (diffuseScaleDown * 10f);
            labelDiffuseScaledownValue.Text = diffuseScaleDown.ToString();
            powerLevelTrackBar.Value = (int) photonPowerLevel;
            labelPowerLevelValue.Text = photonPowerLevel.ToString();
            coneFilterConstantKtrackBar.Value = (int)(10f * (coneFilterConstantK - 1f));
            labelConeFilterConstantKValue.Text = coneFilterConstantK.ToString();
            collectionRadiusTrackBar.Value = (int)(photonCollectionRadius * 10f);
            labelCollectionRadiusValue.Text = photonCollectionRadius.ToString();
        }
    }
}