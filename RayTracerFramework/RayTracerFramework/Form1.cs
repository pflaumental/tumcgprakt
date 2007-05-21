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

namespace RayTracerFramework {
    public partial class Form1 : Form {

        float pos = -10;

        public Form1() {
            InitializeComponent();
        }

        private void btnRender_Click(object sender, EventArgs e) {
             
            Bitmap b = new Bitmap(pictureBox.Size.Width, pictureBox.Size.Height, PixelFormat.Format24bppRgb);// new Bitmap(100, 100);
            float aspectRatio = (float)b.Width / b.Height;

            Camera cam = new Camera(new Vec3(0.0f, 0.0f, -4.5f),
                                    new Vec3(1.0f, -1.0f, 0.0f),
                                    Vec3.StdYAxis, Trigonometric.PI_QUARTER, 1);
            cam.aspectRatio = aspectRatio;
            Scene scene = new Scene(cam);

            //pictureBox.Size = new Size(766, 430);
           
            //scene.cam.hFov /= 2;
            //scene.cam.AdjustVerticalFov(aspectRatio);
           // scene.cam.aspectRatio = aspectRatio;
           // scene.cam.eyePos = new Vec3(0.0f, 0.0f, -5.0f);
           // scene.cam.lookAtPos = new Vec3(0.0f, 0.0f, 0.0f);
          //  scene.geoMng.viewMatrix = scene.cam.GetViewMatrix();

            Light l = new PointLight(new Vec3(-2, 2, -2));
            l.ambient = new Color(0.05f, 0.05f, 0.05f);
            l.diffuse = new Color(0.3f, 0.6f, 0.6f);
            l.specular = new Color(0.6f, 0.5f, 0.9f);

            Light l2 = new PointLight(new Vec3(2, 2, -2));
            l2.ambient = new Color(0.05f, 0.05f, 0.05f);
            l2.diffuse = new Color(0.7f, 0.3f, 0.3f);
            l2.specular = new Color(0.7f, 0.5f, 0.5f);

            Light l3 = new PointLight(new Vec3(2, -2, -2));
            l3.ambient = new Color(0.05f, 0.05f, 0.05f);
            l3.diffuse = new Color(0.0f, 0.2f, 0.8f);
            l3.specular = new Color(0.5f, 0.5f, 0.5f);

            Light l4 = new PointLight(new Vec3(-2, -2, -2));
            l4.ambient = new Color(0.05f, 0.05f, 0.05f);
            l4.diffuse = new Color(0.2f, 0.3f, 0.2f);
            l4.specular = new Color(0.5f, 0.5f, 0.3f);

            Console.WriteLine(((PointLight)l).position);

            scene.lightManager.AddWorldSpaceLight(l);
            scene.lightManager.AddWorldSpaceLight(l2);

            //scene.lightManager.AddWorldSpaceLight(l3);
            //scene.lightManager.AddWorldSpaceLight(l4);


            //scene.AddDSphere(new Vec3(-2.0f, 0.0f, 2.0f), 2.5f, new Material(Color.White, new Color(200, 100, 200), Color.Blue, new Color(0, 255, 0), 20, 0, 0, 0));
            //scene.AddDSphere(new Vec3(3.0f, 0.0f, 2.0f), 2, Material.WhiteMaterial);
            //scene.AddDSphere(new Vec3(3.0f, 0.0f, 3.0f), 3, new Material(Color.Blue, Color.White, Color.White, Color.White, 30, 0, 0, 0));

            //DSphere sphere1 = scene.AddDSphere(new Vec3(0.0f, 0.0f, -1.0f), 1.5f, new Material(Color.White, Color.Red, Color.Red, Color.Red, 10, 0, 0, 0));
            //sphere1.Transform(Matrix.GetScale(1.0f, 0.5f, 1.0f));
            //sphere1.Transform(Matrix.GetRotationX((float)-Math.PI * 0.25f));
            //sphere1.Transform(Matrix.GetRotationZ((float)Math.PI * 0.25f));
            scene.AddDSphere(Vec3.Zero, 1.5f, new Material(Color.White, Color.White, Color.White, Color.White, 30, 0, 0, 5));
            scene.AddDSphere(new Vec3(4.0f, 0.0f, 5.0f), 4, new Material(Color.Blue, Color.White, Color.White, Color.White, 15, 0.2f, 0.6f, 5));

            DBox box1 = scene.AddDBox(new Vec3(-1.0f, -0.5f, -1.0f), 2.0f, 1.0f, 2.0f, new Material(Color.Green, Color.Green, Color.Green, Color.Green, 30, 0.7f, 0, 5));
            //box1.Transform(Matrix.GetRotationX((float)Math.PI * 0.125f));
            //box1.Transform(Matrix.GetRotationY((float)Math.PI * 0.125f));
            box1.Transform(Matrix.GetTranslation(0.5f, -1.0f, -2.4f));

            scene.geoMng.TransformAll();

            Renderer renderer = new Renderer();
            renderer.Render(scene, b);

            

            pictureBox.Image = b;

            //float aspectRatio = (float)width / height;
            //scene.cam.AdjustVerticalFov(aspectRatio);
        }
    }
}
