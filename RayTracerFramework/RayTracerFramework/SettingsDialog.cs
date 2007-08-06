using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
namespace RayTracerFramework {
    public partial class SettingsDialog : Form {

        public bool setupSettingsChanged;

        public SettingsDialog() {
            setupSettingsChanged = false;
            InitializeComponent();
            LoadControlValues();
        }

        private void LoadControlValues() {
            // Setup
            fogAmbientLightAmplifierTrackBar.Value = (int)Settings.Setup.Scene.fogAmbientLightAmplifier;
            labelFogAmbientLightAmplifierValue.Text = Settings.Setup.Scene.fogAmbientLightAmplifier.ToString();

            emitPhotonsCheckBox.Checked = Settings.Setup.PhotonMapping.EmitPhotons;

            storedPhotonsComboBox.Text = Settings.Setup.PhotonMapping.StoredPhotonsCount.ToString();

            powerLevelTrackBar.Value = (int)Settings.Setup.PhotonMapping.PowerLevel;
            labelPowerLevelValue.Text = Settings.Setup.PhotonMapping.PowerLevel.ToString();

            traceRecursionDepthTrackBar.Value = Settings.Setup.PhotonMapping.TracingMaxRecursionDepth;
            labelTraceRecursionDepthValue.Text = Settings.Setup.PhotonMapping.TracingMaxRecursionDepth.ToString();

            mediumIsPatricipatingCheckBox.Checked = Settings.Setup.PhotonMapping.MediumIsParticipating;

            lightingDensityTrackBar.Value = (int)Math.Round(Settings.Setup.PhotonMapping.MediumLightingDensity * 100f);
            labelLightingDensityValue.Text = Settings.Setup.PhotonMapping.MediumLightingDensity.ToString();

            kdTreeMaxHeightTrackBar.Value = Settings.Setup.KDTree.DefaultMaxHeight;
            labelKDTreeMaxHeightValue.Text = Settings.Setup.KDTree.DefaultMaxHeight.ToString();

            kdTreeObjectsPerLeafTrackBar.Value = Settings.Setup.KDTree.DefaultMaxDesiredObjectsPerLeafCount;
            labelKDTreeObjectsPerLeafValue.Text = Settings.Setup.KDTree.DefaultMaxDesiredObjectsPerLeafCount.ToString();

            // Render
            fogEnableCheckBox.Checked = Settings.Render.StdShading.enableFog;

            maxRecursionDepthTrackBar.Value = Settings.Render.Renderer.MaxRecursionDepth;
            labelMaxRecursionDepthValue.Text = Settings.Render.Renderer.MaxRecursionDepth.ToString();

            fogLevelTrackBar.Value = (int)(Settings.Render.StdShading.FogLevel * 10f);
            labelFogLevelValue.Text = Settings.Render.StdShading.FogLevel.ToString();

            renderPhotonsCheckBox.Checked = Settings.Render.PhotonMapping.RenderSurfacePhotons;

            localScaleDownTrackBar.Value = (int)(Settings.Render.PhotonMapping.LocalScaleDown * 10f);
            labelLocalScaledownValue.Text = Settings.Render.PhotonMapping.LocalScaleDown.ToString();

            coneFilterConstantKtrackBar.Value = (int)((Settings.Render.PhotonMapping.ConeFilterConstantK - 1f) * 10f);
            labelConeFilterConstantKValue.Text = Settings.Render.PhotonMapping.ConeFilterConstantK.ToString();

            collectionSphereRadiusTrackBar.Value = (int)(Settings.Render.PhotonMapping.SphereRadius * 20f);
            labelCollectionSphereRadiusValue.Text = Settings.Render.PhotonMapping.SphereRadius.ToString();

            renderMediumCheckBox.Checked = Settings.Render.PhotonMapping.RenderMediumPhotons;

            collectionCapsuleRadiusTrackBar.Value = (int)(Settings.Render.PhotonMapping.CapsuleRadius * 20f);
            labelCollectionCapsuleRadiusValue.Text = Settings.Render.PhotonMapping.CapsuleRadius.ToString();

            lightingAmplifierTrackBar.Value = (int)Settings.Render.PhotonMapping.MediumEnlightmentAmplifier;
            labelLightingAmplifierValue.Text = Settings.Render.PhotonMapping.MediumEnlightmentAmplifier.ToString();

            mediumConeFilterConstantKTrackBar.Value = (int)((Settings.Render.PhotonMapping.MediumConeFilterConstantK - 1f) * 10);
            labelMediumConeFilterConstantKValue.Text = Settings.Render.PhotonMapping.MediumConeFilterConstantK.ToString();
        }

        private void buttonOK_Click(object sender, EventArgs e) {
            if(
                    (int)Settings.Setup.Scene.fogAmbientLightAmplifier != fogAmbientLightAmplifierTrackBar.Value ||
                    Settings.Setup.PhotonMapping.EmitPhotons != emitPhotonsCheckBox.Checked ||
                    Settings.Setup.PhotonMapping.StoredPhotonsCount != int.Parse(storedPhotonsComboBox.Text) ||
                    (int)Settings.Setup.PhotonMapping.PowerLevel != powerLevelTrackBar.Value ||
                    Settings.Setup.PhotonMapping.TracingMaxRecursionDepth != traceRecursionDepthTrackBar.Value ||
                    Settings.Setup.PhotonMapping.MediumIsParticipating != mediumIsPatricipatingCheckBox.Checked ||
                    Math.Round(Settings.Setup.PhotonMapping.MediumLightingDensity * 100f) != lightingDensityTrackBar.Value ||
                    Settings.Setup.KDTree.DefaultMaxHeight != kdTreeMaxHeightTrackBar.Value ||
                    Settings.Setup.KDTree.DefaultMaxDesiredObjectsPerLeafCount != kdTreeObjectsPerLeafTrackBar.Value)
                setupSettingsChanged = true;

            // Setup
            Settings.Setup.Scene.fogAmbientLightAmplifier = fogAmbientLightAmplifierTrackBar.Value;

            Settings.Setup.PhotonMapping.EmitPhotons = emitPhotonsCheckBox.Checked;

            Settings.Setup.PhotonMapping.StoredPhotonsCount = int.Parse(storedPhotonsComboBox.Text);

            Settings.Setup.PhotonMapping.PowerLevel = powerLevelTrackBar.Value;

            Settings.Setup.PhotonMapping.TracingMaxRecursionDepth = traceRecursionDepthTrackBar.Value;

            Settings.Setup.PhotonMapping.MediumIsParticipating = mediumIsPatricipatingCheckBox.Checked;

            Settings.Setup.PhotonMapping.MediumLightingDensity = lightingDensityTrackBar.Value / 100f;

            Settings.Setup.KDTree.DefaultMaxHeight = kdTreeMaxHeightTrackBar.Value;

            Settings.Setup.KDTree.DefaultMaxDesiredObjectsPerLeafCount = kdTreeObjectsPerLeafTrackBar.Value;

            //// Render
            Settings.Render.StdShading.enableFog = fogEnableCheckBox.Checked;

            Settings.Render.Renderer.MaxRecursionDepth = maxRecursionDepthTrackBar.Value;

            Settings.Render.StdShading.FogLevel = fogLevelTrackBar.Value / 10f;

            Settings.Render.PhotonMapping.RenderSurfacePhotons = renderPhotonsCheckBox.Checked;

            Settings.Render.PhotonMapping.LocalScaleDown = localScaleDownTrackBar.Value / 10f;

            Settings.Render.PhotonMapping.ConeFilterConstantK = (10f + coneFilterConstantKtrackBar.Value) / 10f;

            Settings.Render.PhotonMapping.SphereRadius = collectionSphereRadiusTrackBar.Value / 20f;

            Settings.Render.PhotonMapping.RenderMediumPhotons = renderMediumCheckBox.Checked;

            Settings.Render.PhotonMapping.CapsuleRadius = collectionCapsuleRadiusTrackBar.Value / 20f;

            Settings.Render.PhotonMapping.MediumEnlightmentAmplifier = lightingAmplifierTrackBar.Value;

            Settings.Render.PhotonMapping.MediumConeFilterConstantK = (10f + mediumConeFilterConstantKTrackBar.Value) / 10f;
        }

        private void buttonCancel_Click(object sender, EventArgs e) {
            LoadControlValues();
        }

        private void emitPhotonsCheckBox_CheckedChanged(object sender, EventArgs e) {
            storedPhotonsComboBox.Enabled = ((CheckBox)sender).Checked;
            powerLevelTrackBar.Enabled = ((CheckBox)sender).Checked;
            traceRecursionDepthTrackBar.Enabled = ((CheckBox)sender).Checked;
            if (!((CheckBox)sender).Checked) {
                mediumIsPatricipatingCheckBox.Checked = false;
                renderMediumCheckBox.Checked = false;
                renderPhotonsCheckBox.Checked = false;
            }
        }

        private void mediumIsPatricipatingCheckBox_CheckedChanged(object sender, EventArgs e) {
            lightingDensityTrackBar.Enabled = ((CheckBox)sender).Checked;
            if (((CheckBox)sender).Checked)
                emitPhotonsCheckBox.Checked = true;
            else
                renderMediumCheckBox.Checked = false;
        }

        private void fogEnableCheckBox_CheckedChanged(object sender, EventArgs e) {
            fogLevelTrackBar.Enabled = ((CheckBox)sender).Checked;
            if (!((CheckBox)sender).Checked)
                renderMediumCheckBox.Checked = false;
        }

        private void renderPhotonsCheckBox_CheckedChanged(object sender, EventArgs e) {
            localScaleDownTrackBar.Enabled = ((CheckBox)sender).Checked;
            coneFilterConstantKtrackBar.Enabled = ((CheckBox)sender).Checked;
            collectionSphereRadiusTrackBar.Enabled = ((CheckBox)sender).Checked;
            if (((CheckBox)sender).Checked)
                emitPhotonsCheckBox.Checked = true;
        }

        private void renderMediumCheckBox_CheckedChanged(object sender, EventArgs e) {
            collectionCapsuleRadiusTrackBar.Enabled = ((CheckBox)sender).Checked;
            lightingAmplifierTrackBar.Enabled = ((CheckBox)sender).Checked;
            mediumConeFilterConstantKTrackBar.Enabled = ((CheckBox)sender).Checked;
            if (((CheckBox)sender).Checked) {
                emitPhotonsCheckBox.Checked = true;
                mediumIsPatricipatingCheckBox.Checked = true;
                fogEnableCheckBox.Checked = true;
            }
        }

        private void fogAmbientLightAmplifierTrackBar_Scroll(object sender, EventArgs e) {
            labelFogAmbientLightAmplifierValue.Text = ((TrackBar)sender).Value.ToString();
        }

        private void powerLevelTrackBar_Scroll(object sender, EventArgs e) {
            labelPowerLevelValue.Text = ((TrackBar)sender).Value.ToString();
        }

        private void traceRecursionDepthTrackBar_Scroll(object sender, EventArgs e) {
            labelTraceRecursionDepthValue.Text = ((TrackBar)sender).Value.ToString();
        }

        private void lightingDensityTrackBar_Scroll(object sender, EventArgs e) {
            labelLightingDensityValue.Text = (((TrackBar)sender).Value / 100f).ToString();
        }

        private void kdTreeMaxHeightTrackBar_Scroll(object sender, EventArgs e) {
            labelKDTreeMaxHeightValue.Text = ((TrackBar)sender).Value.ToString();
        }

        private void kdTreeObjectsPerLeafTrackBar_Scroll(object sender, EventArgs e) {
            labelKDTreeObjectsPerLeafValue.Text = ((TrackBar)sender).Value.ToString();
        }

        private void maxRecursionDepthTrackBar_Scroll(object sender, EventArgs e) {
            labelMaxRecursionDepthValue.Text = ((TrackBar)sender).Value.ToString();
        }

        private void fogLevelTrackBar_Scroll(object sender, EventArgs e) {
            labelFogLevelValue.Text = (((TrackBar)sender).Value / 10f).ToString();
        }

        private void localScaleDownTrackBar_Scroll(object sender, EventArgs e) {
            labelLocalScaledownValue.Text = (((TrackBar)sender).Value / 10f).ToString();
        }

        private void coneFilterConstantKtrackBar_Scroll(object sender, EventArgs e) {
            labelConeFilterConstantKValue.Text = ((((TrackBar)sender).Value + 10) / 10f).ToString();
        }

        private void collectionSphereRadiusTrackBar_Scroll(object sender, EventArgs e) {
            labelCollectionSphereRadiusValue.Text = (((TrackBar)sender).Value / 20f).ToString();
        }

        private void collectionCapsuleRadiusTrackBar_Scroll(object sender, EventArgs e) {
            labelCollectionCapsuleRadiusValue.Text = (((TrackBar)sender).Value / 20f).ToString();
        }

        private void lightingAmplifierTrackBar_Scroll(object sender, EventArgs e) {
            labelLightingAmplifierValue.Text = ((TrackBar)sender).Value.ToString();
        }

        private void mediumConeFilterConstantKTrackBar_Scroll(object sender, EventArgs e) {
            labelMediumConeFilterConstantKValue.Text = (((TrackBar)sender).Value / 20f).ToString();
        }
    }
}