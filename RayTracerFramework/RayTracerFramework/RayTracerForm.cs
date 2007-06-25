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
        private Scene scene;
        private Renderer renderer;
        private Camera cam;

        private int pos = 0;
        private Matrix camTransform = Matrix.GetRotationY(Trigonometric.PI_QUARTER);

        public RayTracerForm() {
            InitializeComponent();
            Setup();
        }

        private void Setup() {
            renderer = new Renderer(progressBar, statusBar);

            //Camera cam = new Camera(new Vec3(-5f * (float)Math.Sin(pos * Trigonometric.PI * 0.1f), 0f, -5 * (float)Math.Cos(pos++ * Trigonometric.PI * 0.1f)),
            //                        new Vec3(0, 0, 0f),
            //                        Vec3.StdYAxis, Trigonometric.PI_QUARTER, aspectRatio);

            Vec3 camPos = new Vec3(0, 0, -5);
            Vec3 camLookAt = Vec3.Zero;
            cam = new Camera(camPos, camLookAt, Vec3.StdYAxis, Trigonometric.PI_QUARTER, 4f / 3f);
            //cam.aspectRatio = aspectRatio;

            scene = new Scene(cam);

            Light l = new PointLight(new Vec3(2, 8, -12));
            l.ambient = new Color(0.05f, 0.05f, 0.05f);
            l.diffuse = new Color(0.6f, 0.4f, 0.4f);
            l.specular = new Color(0.8f, 0.5f, 0.5f);

            Light l2 = new PointLight(new Vec3(4, 6, -6));
            l2.ambient = new Color(0.05f, 0.05f, 0.05f);
            l2.diffuse = new Color(0.3f, 0.4f, 0.5f);
            l2.specular = new Color(0.5f, 0.5f, 0.6f);

            Light l3 = new PointLight(new Vec3(2, -2, -2));
            l3.ambient = new Color(0.05f, 0.05f, 0.05f);
            l3.diffuse = new Color(0.0f, 0.2f, 0.8f);
            l3.specular = new Color(0.5f, 0.5f, 0.5f);

            Light l4 = new PointLight(new Vec3(-2, -2, -2));
            l4.ambient = new Color(0.05f, 0.05f, 0.05f);
            l4.diffuse = new Color(0.2f, 0.3f, 0.2f);
            l4.specular = new Color(0.5f, 0.5f, 0.3f);

            scene.useCubeMap = false;

            scene.lightManager.AddBlinnWorldSpaceLight(l);
            scene.lightManager.AddBlinnWorldSpaceLight(l2);

            //scene.lightManager.AddWorldSpaceLight(l3);
            //scene.lightManager.AddWorldSpaceLight(l4);

            OBJLoader loader = new OBJLoader();
            DMesh mesh = loader.LoadFromFile("bunny_t4046.obj");

            scene.AddDMesh(mesh, Matrix.GetTranslation(-2, 0, 0));
            FastBitmap earthTexture = new FastBitmap(new Bitmap(Image.FromFile("../../Textures/earth.jpg")));
            scene.AddDSphere(new Vec3(4.0f, 1.0f, -0.5f), 1.5f, new Material(Color.Blue, Color.Red, Color.Blue, Color.White, 10, true, true, 0.95f, 0.85f, null));
            scene.AddDSphere(new Vec3(6.0f, 2.5f, 5.0f), 4, new Material(Color.White, Color.White, Color.White, Color.White, 15, true, false, 0.6f, 0f, earthTexture));

            FastBitmap wallTexture = new FastBitmap(new Bitmap(Image.FromFile("../../Textures/env2.jpg")));
            Matrix boxTransform = Matrix.GetRotationY(Trigonometric.PI_QUARTER);
            boxTransform *= Matrix.GetRotationX(Trigonometric.PI_QUARTER);
            boxTransform *= Matrix.GetTranslation(0.5f, -0.1f, 0f);
            scene.AddDBox(boxTransform, 1.5f, 1.5f, 1.5f, true, new Material(Color.White, Color.White, Color.White, Color.White, 30, false, false, 0.1f, 0f, wallTexture));
            scene.AddDBox(Matrix.GetTranslation(0f, -1.5f, 0f), 20f, 0.3f, 20f, false, new Material(Color.White, Color.White, Color.White, Color.White, 30, true, false, 0.1f, 0f, null));
            scene.AddDBox(Matrix.Identity, 30f, 30f, 30f, false, new Material(Color.White, Color.White, Color.White, Color.White, 30, false , false, 0f, 0f, null));

            // Do not forget:
            scene.Setup();

        }

        private void Render() {
            float startMillis = Environment.TickCount;

            Bitmap bitmap = new Bitmap(pictureBox.Size.Width, pictureBox.Size.Height, PixelFormat.Format24bppRgb);// new Bitmap(100, 100);
            cam.aspectRatio = ((float)bitmap.Width) / bitmap.Height;

            cam.eyePos = Vec3.TransformNormal3(cam.eyePos, camTransform);

            renderer.Render(scene, bitmap);            

            float elapsedTime = (Environment.TickCount - startMillis) / 1000.0f;

            string elapsedTimeString = "Picture (" + bitmap.Width + "x" + bitmap.Height + ") computed in " +
                                       elapsedTime.ToString("F") + "s. Time per pixel: " +
                                       (1000000.0 * elapsedTime / (bitmap.Width * bitmap.Height)).ToString("F")
                                       + "\u00B5s.";
            statusBar.Items.Clear();
            statusBar.Items.Add(elapsedTimeString);

            pictureBox.Image = bitmap;
        }

        private void btnRender_Click(object sender, EventArgs e) {
            Render();
        }
    }
}
