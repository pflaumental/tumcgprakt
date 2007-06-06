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
        private Matrix camTransform = Matrix.GetRotationY(Trigonometric.PI_QUARTER / 4f);
        
        public RayTracerForm() {            
            InitializeComponent();
            Setup();
        }

        private void Setup() {
            renderer = new Renderer(progressBar, statusBar);

            //Camera cam = new Camera(new Vec3(-5f * (float)Math.Sin(pos * Trigonometric.PI * 0.1f), 0f, -5 * (float)Math.Cos(pos++ * Trigonometric.PI * 0.1f)),
            //                        new Vec3(0, 0, 0f),
            //                        Vec3.StdYAxis, Trigonometric.PI_QUARTER, aspectRatio);

            //Vec3 camPos = new Vec3(253397.460f, 1245810.058f, -21396.996f);
            //Vec3 camLookAt = new Vec3(253397.460f, 1245810.058f, -21395.996f);
            Vec3 camPos = new Vec3(0, 0, -15);
            Vec3 camLookAt = Vec3.Zero;
            cam = new Camera(camPos, camLookAt, Vec3.StdYAxis, Trigonometric.PI_QUARTER, 4f/3f);
            //cam.aspectRatio = aspectRatio;

            scene = new Scene(cam);
            scene.useCubeMap = false;

            Light l = new PointLight(new Vec3(1, 4, -6));
            l.ambient = new Color(0.05f, 0.05f, 0.05f);
            l.diffuse = new Color(0.6f, 0.4f, 0.4f);
            l.specular = new Color(0.8f, 0.5f, 0.5f);

            Light l2 = new PointLight(new Vec3(2, 3, -3));
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

            scene.lightManager.AddWorldSpaceLight(l);
            scene.lightManager.AddWorldSpaceLight(l2);

            //scene.lightManager.AddWorldSpaceLight(l3);
            //scene.lightManager.AddWorldSpaceLight(l4);

            //OBJLoader loader = new OBJLoader();
            //DMesh mesh = loader.LoadFromFile("bunny_t4046.obj");
            //scene.AddDMesh(mesh, Matrix.GetTranslation(-2, 0, 0));

            PointLoader pointLoader = new PointLoader();
            List<IIntersectable> points = pointLoader.LoadFromFile("reduced_7474.point");
            scene.AddDPoints(points);

            //scene.AddDSphere(new Vec3(4.0f, 0.0f, 0.0f), 1.5f, new Material(Color.Blue, Color.Red, Color.Blue, Color.White, 10, 0f, 1f, 1.04f));
            //scene.AddDSphere(new Vec3(6.0f, 3.0f, 5.0f), 4, new Material(Color.White, Color.White, Color.White, Color.White, 15, 0f, 0f, 1.04f));

            //DBox box1 = scene.AddDBox(Matrix.GetRotationY(-Trigonometric.PI_QUARTER) * Matrix.GetTranslation(new Vec3(-4f, -2f, 2f)), 4f, 4f, 2f, new Material(Color.White, Color.White, Color.White, Color.White, 10, 0.7f, 0, 1));
            //scene.AddDBox(new Vec3(-6,-2.02f,0), 0.2f, 6f, 3f, new Material(Color.White, Color.White, Color.White, Color.White, 30, 0.15f, 0.75f, 1.03f));


            //scene.AddDBox(Matrix.GetTranslation(-10.0f, -3f, -10f), 20f, 0.3f, 20f, new Material(Color.White, Color.White, Color.White, Color.White, 30, 0.3f, 0, 1));
            //DBox box1 = scene.AddDBox(new Vec3(0.0f, 0f, 0f),2f, 2f, 2f, new Material(Color.Green, Color.Green, Color.Green, Color.Green, 30, 0.7f, 0, 0));
            //box1.Transform(Matrix.GetRotationX((float)Math.PI * -0.25f));
            //box1.Transform(Matrix.GetRotationY((float)Math.PI * 0.125f));
            //box1.Transform(Matrix.GetTranslation(0f, -0.2f, 0f));

            // Do not forget:
            scene.Setup();
        }

        private void Render() {
            float startMillis = Environment.TickCount;

            Bitmap bitmap = new Bitmap(pictureBox.Size.Width, pictureBox.Size.Height, PixelFormat.Format24bppRgb);// new Bitmap(100, 100);
            cam.aspectRatio = ((float)bitmap.Width) / bitmap.Height;            

            renderer.Render(scene, bitmap);

            cam.eyePos = Vec3.TransformNormal3(cam.eyePos, camTransform);

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
