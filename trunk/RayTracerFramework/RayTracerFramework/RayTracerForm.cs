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
        private Camera cam;

        private Bitmap renderBitmap;
        private byte[] rgbValues;
        private int rgbValuesLength;

        private Vec3 camPos;
        private Vec3 camLookAt;

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

            //scene = sceneManager.LoadScene("standardscene.xml");
            //cam = scene.cam;

            settingsDialog = new SettingsDialog();
            camPos = new Vec3(0, 0, -5);
            camLookAt = Vec3.Zero;
            tbCamPosX.Text = camPos.x.ToString();
            tbCamPosY.Text = camPos.y.ToString();
            tbCamPosZ.Text = camPos.z.ToString();
            tbCamLookAtX.Text = camLookAt.x.ToString();
            tbCamLookAtY.Text = camLookAt.y.ToString();
            tbCamLookAtZ.Text = camLookAt.z.ToString();
            sceneReady = false;
            isRendering = false;
            userCanceled = false;
            renderer = new Renderer();
        }

        private void Setup() {            
            //Camera cam = new Camera(new Vec3(-5f * (float)Math.Sin(pos * Settings.Render.Trigonometric.Pi * 0.1f), 0f, -5 * (float)Math.Cos(pos++ * Settings.Render.Trigonometric.Pi * 0.1f)),
            //                        new Vec3(0, 0, 0f),
            //                        Vec3.StdYAxis, Settings.Render.Trigonometric.PiQuarter, aspectRatio);

            cam = new Camera(camPos, camLookAt, Vec3.StdYAxis, Settings.Render.Trigonometric.PiQuarter, 4f / 3f);
            //cam.aspectRatio = aspectRatio;

            scene = new Scene(cam);

            PointLight l = new PointLight(new Vec3(1, 4, -3));
            l.ambient = new Color(0.05f, 0.05f, 0.05f);
            l.diffuse = new Color(0.7f, 0.7f, 0.7f);
            l.specular = new Color(0.5f, 0.5f, 0.5f);

            PhotonMapping.Light pl = new PhotonMapping.PointLight(l.position);
            pl.diffuse = l.diffuse;
            pl.power = 100;

            PointLight l2 = new PointLight(new Vec3(4.0f, 6.0f, -0.5f));
            l2.ambient = l.ambient;
            l2.diffuse = l.diffuse;
            l2.specular = l.specular;

            PhotonMapping.Light pl2 = new PhotonMapping.PointLight(l2.position);
            pl2.diffuse = l2.diffuse;
            pl2.power = 50;

            PointLight l3 = new PointLight(new Vec3(2, -2, -2));
            l3.ambient = new Color(0.05f, 0.05f, 0.05f);
            l3.diffuse = new Color(0.0f, 0.2f, 0.8f);
            l3.specular = new Color(0.5f, 0.5f, 0.5f);

            PointLight l4 = new PointLight(new Vec3(-2, -2, -2));
            l4.ambient = new Color(0.05f, 0.05f, 0.05f);
            l4.diffuse = new Color(0.2f, 0.3f, 0.2f);
            l4.specular = new Color(0.5f, 0.5f, 0.3f);

            scene.useCubeMap = false;

            DirectionalLight l5 = new DirectionalLight(new Vec3(2, 3, 5));
            l5.ambient = new Color(0.3f, 0.2f, 0.11f);
            l5.diffuse = Color.Red;
            l5.specular = Color.Black;

            scene.lightManager.AddBlinnWorldSpaceLight(l);
            scene.lightManager.AddPhotonWorldSpaceLight(pl);

            scene.lightManager.AddBlinnWorldSpaceLight(l2);
            scene.lightManager.AddPhotonWorldSpaceLight(pl2);

            scene.lightManager.AddBlinnWorldSpaceLight(l5);

            OBJLoader loader = new OBJLoader();
            DMesh mesh = loader.LoadFromFile("bunny_t4046.obj");

            scene.AddDMesh(mesh, Matrix.GetTranslation(-2, 0, 0));
            //FastBitmap earthTexture = new FastBitmap(new Bitmap(Image.FromFile("../../Textures/earth.jpg")));
            scene.AddDSphere(new Vec3(4.0f, 1.0f, -0.5f), 1.5f, new Material(Color.Blue, Color.Red, Color.Blue, Color.White, 10, true, true, 0.70f, 0.85f, null));
            scene.AddDSphere(new Vec3(6.0f, 2.5f, 5.0f), 4, new Material(Color.White, Color.White, Color.White, Color.White, 15, true, false, 0.6f, 0f, "earth.jpg"));

            //FastBitmap wallTexture = new FastBitmap(new Bitmap(Image.FromFile("../../Textures/env2.jpg")));
            Matrix boxTransform = Matrix.GetRotationY(Settings.Render.Trigonometric.PiQuarter);
            boxTransform *= Matrix.GetRotationX(Settings.Render.Trigonometric.PiQuarter);
            boxTransform *= Matrix.GetTranslation(0.5f, -0.1f, 0f);
            scene.AddDBox(boxTransform, 1.5f, 1.5f, 1.5f, true, new Material(Color.White, Color.White, Color.White, Color.White, 30, false, false, 0.1f, 0f, "env2.jpg"));
            scene.AddDBox(Matrix.GetTranslation(0f, -1.5f, 0f), 20f, 0.3f, 20f, false, new Material(Color.White, Color.White, new Color(0.8f, 0.8f, 0.8f), Color.White, 30, false, false, 0.1f, 0f, null));
            scene.AddDBox(Matrix.Identity, 16f, 16f, 16f, false, new Material(Color.White, Color.White, new Color(0.3f, 0.5f, 0.1f), Color.White, 30, false, false, 0f, 0f, null));
            
            // SceneManager sm2 = new SceneManager();
            // sm2.SaveScene("scene.xml", scene, new Vec2(pictureBox.Size.Width, pictureBox.Size.Height));

            
            if (Settings.Setup.PhotonMapping.EmitPhotons)
                scene.ActivatePhotonMapping(
                        Settings.Setup.PhotonMapping.StoredPhotonsCount,
                        progressBar,
                        statusBar);
            // Do not forget:
            scene.Setup();  
        }

        private void Render() {            
            pictureBox.Image = null;
            renderBitmap = new Bitmap(pictureBox.Size.Width, pictureBox.Size.Height, PixelFormat.Format24bppRgb);

            // Parse CamPos and CamLookAt
            cam.eyePos.x = float.Parse(tbCamPosX.Text);
            cam.eyePos.y = float.Parse(tbCamPosY.Text);
            cam.eyePos.z = float.Parse(tbCamPosZ.Text);
            cam.lookAtPos.x = float.Parse(tbCamLookAtX.Text);
            cam.lookAtPos.y = float.Parse(tbCamLookAtY.Text);
            cam.lookAtPos.z = float.Parse(tbCamLookAtZ.Text);
            
            cam.aspectRatio = ((float)renderBitmap.Width) / renderBitmap.Height;

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
                Setup();                
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
            Setup();
            statusBar.Items.Clear();
            statusBar.Items.Add("Setup done.");
        }

        private void loadMenuItem_Click(object sender, EventArgs e) {
            

            LoadSceneForm loadForm = new LoadSceneForm();
            loadForm.ShowDialog();

            scene = sceneManager.LoadScene("standardscene.xml");
            scene.mediumColor = new Color(0.5f, 0.3f, 0.3f);
            cam = scene.cam;

            statusBar.Items.Clear();
            statusBar.Items.Add("Scene has been loaded from \"standardscene.xml\".");
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
                    renderArgs.targetHeight, 
                    worker);
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
