using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using RayTracerFramework.RayTracer;
using RayTracerFramework.Loading;
using System.IO;

namespace RayTracerFramework {
    public partial class LoadSceneForm : Form {
        private SceneManager sceneManager;
        public Scene scene;
        public int resolutionX, resolutionY;

        public string sceneFileBaseDirectory = "../../Scenes/";
        public string standardSceneFilename = "standardscene.xml";

        public LoadSceneForm() {
            InitializeComponent();
            sceneManager = new SceneManager();

            string[] sceneFiles = Directory.GetFiles(sceneFileBaseDirectory, "*.xml");
            foreach(string sceneFile in sceneFiles)
                cmbSceneName.Items.Add(Path.GetFileName(sceneFile));
            cmbSceneName.SelectedItem = standardSceneFilename;
        }
          

        private void FillFormFromSceneFile(string sceneFile) {
            scene = sceneManager.LoadScene(sceneFileBaseDirectory + sceneFile);

            txtEyePosX.Text = scene.cam.eyePos.x.ToString();
            txtEyePosY.Text = scene.cam.eyePos.y.ToString();
            txtEyePosZ.Text = scene.cam.eyePos.z.ToString();

            txtLookAtX.Text = scene.cam.lookAtPos.x.ToString();
            txtLookAtY.Text = scene.cam.lookAtPos.y.ToString();
            txtLookAtZ.Text = scene.cam.lookAtPos.z.ToString();

            txtUpDirX.Text = scene.cam.upDir.x.ToString();
            txtUpDirY.Text = scene.cam.upDir.y.ToString();
            txtUpDirZ.Text = scene.cam.upDir.z.ToString();

            txtHFOV.Text = scene.cam.hFov.ToString();
            txtAspectRatio.Text = scene.cam.aspectRatio.ToString();

            txtTargetResolutionX.Text = sceneManager.resolutionX.ToString();
            txtTargetResolutionY.Text = sceneManager.resolutionY.ToString();



        }

        private void cmbSceneName_SelectedIndexChanged(object sender, EventArgs e) {
            FillFormFromSceneFile((string)cmbSceneName.SelectedItem);
        }
    }
}