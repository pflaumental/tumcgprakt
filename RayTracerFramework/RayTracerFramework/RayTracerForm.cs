using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using RayTracerFramework.RayTracer;
using RayTracerFramework.Geometry;
using System.Drawing.Imaging;
using RayTracerFramework.Shading;
using Color = RayTracerFramework.Shading.Color;
using RayTracerFramework.Utility;
using RayTracerFramework.Loading;

namespace RayTracerFramework {
    public partial class RayTracerForm : Form {
        private SettingsDialog settingsDialog; 

        private Scene scene;
        private SceneManager sceneManager;
        private Renderer renderer;
        //private Camera cam;

        private Bitmap renderBitmap;
        private byte[] rgbValues;
        private int rgbValuesLength;

        //private Vec3 camPos;
        //private Vec3 camLookAt;

        private bool sceneReady;
        private bool isRendering;
        private bool userCanceled;

        private int startMillis;
        private int lastMillis;
        int elapsedTime;

        private class RenderArgs {
            public Scene scene;
            public byte[] rgbValues;
            public int rgbValuesLength;
            public int stride;
            public int targetWidth;
            public int targetHeight;

            public RenderArgs(
                    Scene scene, 
                    byte[] rgbValues,
                    int rgbValuesLength,
                    int stride,
                    int targetWidth,
                    int targetHeight) {
                this.scene = scene;
                this.rgbValues = rgbValues;
                this.rgbValuesLength = rgbValuesLength;
                this.stride = stride;
                this.targetWidth = targetWidth;
                this.targetHeight = targetHeight;
            }
        }

        public RayTracerForm() {
            InitializeComponent();
            sceneManager = new SceneManager();
            string stdSceneFile = Settings.Setup.Loading.DefaultStandardSceneDirectory
                    + Settings.Setup.Loading.DefaultSceneName;
            scene = sceneManager.LoadScene(stdSceneFile);
            LoadCameraControlValues();

            settingsDialog = new SettingsDialog();
            Vec3 camPos = new Vec3(0, 0, -5);
            Vec3 camLookAt = Vec3.Zero;
            sceneReady = false;
            isRendering = false;
            userCanceled = false;
            renderer = new Renderer(renderBackgroundWorker);
        }

        private void LoadCameraControlValues() {
            tbCamPosX.Text = scene.cam.eyePos.x.ToString();
            tbCamPosY.Text = scene.cam.eyePos.y.ToString();
            tbCamPosZ.Text = scene.cam.eyePos.z.ToString();
            tbCamLookAtX.Text = scene.cam.lookAtPos.x.ToString();
            tbCamLookAtY.Text = scene.cam.lookAtPos.y.ToString();
            tbCamLookAtZ.Text = scene.cam.lookAtPos.z.ToString();
        }

        private void Render() {            
            pictureBox.Image = null;
            renderBitmap = new Bitmap(pictureBox.Size.Width, pictureBox.Size.Height, PixelFormat.Format24bppRgb);

            // Parse CamPos and CamLookAt
            try {
                scene.cam.eyePos.x = float.Parse(tbCamPosX.Text);
                scene.cam.eyePos.y = float.Parse(tbCamPosY.Text);
                scene.cam.eyePos.z = float.Parse(tbCamPosZ.Text);
                scene.cam.lookAtPos.x = float.Parse(tbCamLookAtX.Text);
                scene.cam.lookAtPos.y = float.Parse(tbCamLookAtY.Text);
                scene.cam.lookAtPos.z = float.Parse(tbCamLookAtZ.Text);
            } catch {
                MessageBox.Show("EyePos/LookAt specified in invalid format. "
                        + "Using standard values (EyePos = (0,0,-5), LookAt = (0,0,0)",
                        "Input error");
                scene.cam.eyePos = new Vec3(0, 0, -5);
                scene.cam.lookAtPos = Vec3.Zero;
                LoadCameraControlValues();
            }

            scene.cam.aspectRatio = ((float)renderBitmap.Width) / renderBitmap.Height;

            // Generate rgbValues
            BitmapData bitmapData = renderBitmap.LockBits(new Rectangle(0, 0, renderBitmap.Width, renderBitmap.Height),
                    ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);            
            int stride = bitmapData.Stride;
            renderBitmap.UnlockBits(bitmapData);
            rgbValuesLength = stride * renderBitmap.Height;
            rgbValues = new byte[rgbValuesLength];

            elapsedTime = 0;
            lastMillis = startMillis = Environment.TickCount;

            renderBackgroundWorker.RunWorkerAsync(new RenderArgs(scene, rgbValues, rgbValuesLength, stride, renderBitmap.Width, renderBitmap.Height));
        }

        private void btnRender_Click(object sender, EventArgs e) {
            if (isRendering) {
                userCanceled = true;
                renderBackgroundWorker.CancelAsync();
                return;
            }
            btnRender.Enabled = false;
            btnSetup.Enabled = false;
            mainMenu.Enabled = false;
            if (!sceneReady) {
                statusBar.Items.Clear();
                statusBar.Items.Add("Setting up scene. This may take some time...");
                btnRender.Text = "Render";
                this.Update();
                scene.Setup(progressBar, statusBar);
                sceneReady = true;
            }
            btnRender.Enabled = true;
            btnRender.Text = "Cancel";
            isRendering = true;
            statusBar.Items.Clear();
            statusBar.Items.Add("Rendering...");
            progressBar.Value = 0;
            Render();
        }

        private void btnSetup_Click(object sender, EventArgs e) {            
            statusBar.Items.Clear();
            statusBar.Items.Add("Setting up scene. This may take some time...");
            btnRender.Text = "Render";
            this.Update();
            sceneReady = true;
            scene.Setup(progressBar, statusBar);
            statusBar.Items.Clear();
            statusBar.Items.Add("Setup done.");
        }

        private void loadMenuItem_Click(object sender, EventArgs e) {
            LoadSceneForm loadForm = new LoadSceneForm();
            if (loadForm.ShowDialog() == DialogResult.OK) {
                scene = loadForm.scene;
                if (sceneReady) {
                    sceneReady = false;
                    btnRender.Text = "Setup + R.";
                }
                LoadCameraControlValues();
            }
            statusBar.Items.Clear();
            statusBar.Items.Add("Scene has been loaded from \"" + loadForm.selectedScene + "\".");
        }

        private void settingsMenuItem_Click(object sender, EventArgs e) {
            DialogResult dialogResult = settingsDialog.ShowDialog();
            if (dialogResult == DialogResult.OK) {
                if (sceneReady == true && settingsDialog.setupSettingsChanged) {
                    settingsDialog.setupSettingsChanged = false;
                    sceneReady = false;
                    btnRender.Text = "Setup + R.";
                }
            }
        }

        private void renderBackgroundWorker_DoWork(object sender, DoWorkEventArgs e) {
            BackgroundWorker worker = sender as BackgroundWorker;
            RenderArgs renderArgs = e.Argument as RenderArgs;
            renderer.Render(
                    renderArgs.scene, 
                    renderArgs.rgbValues, 
                    renderArgs.rgbValuesLength,
                    renderArgs.stride, 
                    renderArgs.targetWidth, 
                    renderArgs.targetHeight);
        }

        private void renderBackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e) {            
            // Show image progress
            BitmapData bitmapData = renderBitmap.LockBits(new Rectangle(0, 0, renderBitmap.Width, renderBitmap.Height),
                                ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            IntPtr bitmapDataAddress = bitmapData.Scan0;
            System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, bitmapDataAddress, rgbValuesLength);
            renderBitmap.UnlockBits(bitmapData);
            pictureBox.Image = renderBitmap;            
            // Show progress in numbers
            int progress = 0;
            int currentMillis = 0;
            progress = e.ProgressPercentage < 1 ? 1 : e.ProgressPercentage;
            progress = progress > 100 ? 100 : progress;
            progressBar.Value = progress;
            currentMillis = Environment.TickCount;
            int remainingSeconds = 0;
            elapsedTime += (currentMillis - lastMillis);
            remainingSeconds = ((100 - progress) * elapsedTime) / (progress * 1000);
            statusBar.Items.Clear();
            statusBar.Items.Add("Rendering... Elapsed time: " + (int)(elapsedTime / 1000f) + "s. Estimated remaining time: " + remainingSeconds + "s.");
            lastMillis = currentMillis;
        }

        private void renderBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
            BitmapData bitmapData = renderBitmap.LockBits(new Rectangle(0, 0, renderBitmap.Width, renderBitmap.Height),
                                ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            IntPtr bitmapDataAddress = bitmapData.Scan0;
            System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, bitmapDataAddress, rgbValuesLength);
            renderBitmap.UnlockBits(bitmapData);
            pictureBox.Image = renderBitmap;

            btnRender.Text = "Render";
            btnSetup.Enabled = true;
            mainMenu.Enabled = true;
            isRendering = false;

            if (userCanceled) {
                progressBar.Value = 0;
                statusBar.Items.Clear();
                statusBar.Items.Add("User canceled.");
            } else {
                progressBar.Value = 100;
                float elapsedTime = (Environment.TickCount - startMillis) / 1000.0f;

                string elapsedTimeString = "Picture (" + renderBitmap.Width + "x" + renderBitmap.Height + ") computed in " +
                                           elapsedTime.ToString("F") + "s. Time per pixel: " +
                                           (1000000.0 * elapsedTime / (renderBitmap.Width * renderBitmap.Height)).ToString("F")
                                           + "\u00B5s.";
                statusBar.Items.Clear();
                statusBar.Items.Add(elapsedTimeString);
            }
        }
    }
}
