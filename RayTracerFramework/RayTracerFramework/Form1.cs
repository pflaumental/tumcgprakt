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

namespace RayTracerFramework {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        private void btnRender_Click(object sender, EventArgs e) {
            Scene scene = new Scene();

            System.Drawing.Color c = System.Drawing.Color.LightSlateGray;
            Console.WriteLine(c.R + "," + c.G + "," + c.B);

            //pictureBox.Size = new Size(766, 430);
            Bitmap b = new Bitmap(pictureBox.Size.Width, pictureBox.Size.Height, PixelFormat.Format24bppRgb);// new Bitmap(100, 100);
            float aspectRatio = (float)b.Width / b.Height;
            //scene.cam.hFov /= 2;
            //scene.cam.AdjustVerticalFov(aspectRatio);
            scene.cam.aspectRatio = aspectRatio;
            scene.cam.eyePos = new Vec3(0.0f, 0.0f, -5.0f);
            scene.cam.lookAtPos = new Vec3(0.0f, 0.0f, 0.0f);
            scene.geoMng.viewMatrix = scene.cam.GetViewMatrix();

            Light l = new PointLight(new Vec3(-10, 0, -2));
            l.ambient = new Color(0.05f, 0.05f, 0.05f);
            l.diffuse = new Color(0.2f, 0.3f, 0.5f);
            l.specular = new Color(0.8f, 0.8f, 0.8f);

            Light l2 = new PointLight(new Vec3(-5, 10, -2));
            l2.ambient = new Color(0.05f, 0.05f, 0.05f);
            l2.diffuse = new Color(0.2f, 0.3f, 0.6f);
            l2.specular = new Color(0.6f, 0.5f, 0.5f);

            Light l3 = new PointLight(new Vec3(5, 5, 5));
            l3.ambient = new Color(0.05f, 0.05f, 0.05f);
            l3.diffuse = new Color(0.0f, 0.2f, 0.8f);
            l3.specular = new Color(0.5f, 0.5f, 0.5f);

            Light l4 = new PointLight(new Vec3(5, -5, 5));
            l4.ambient = new Color(0.05f, 0.05f, 0.05f);
            l4.diffuse = new Color(0.2f, 0.2f, 0.2f);
            l4.specular = new Color(0.5f, 0.5f, 0.5f);

            scene.lightManager.AddLight(l);
            scene.lightManager.AddLight(l2);

            //scene.lightManager.AddLight(l3);
            //scene.lightManager.AddLight(l4);


            //scene.AddDSphere(new Vec3(-2.0f, 0.0f, 2.0f), 2.5f, new Material(Color.White, new Color(200, 100, 200), Color.Blue, new Color(0, 255, 0), 20, 0, 0, 0));
            //scene.AddDSphere(new Vec3(0.0f, 0.0f, 0.0f), 1, Material.GreenMaterial);
            //scene.AddDSphere(new Vec3(3.0f, 0.0f, 3.0f), 3, new Material(Color.Blue, Color.White, Color.White, Color.White, 30, 0, 0, 0));

            //DSphere sphere1 = scene.AddDSphere(new Vec3(0.0f, 0.0f, -1.0f), 1.5f, new Material(Color.White, Color.Red, Color.Red, Color.Red, 10, 0, 0, 0));
            //sphere1.Transform(Matrix.GetScale(1.0f, 0.5f, 1.0f));
            //sphere1.Transform(Matrix.GetRotationX((float)-Math.PI * 0.25f));
            //sphere1.Transform(Matrix.GetRotationZ((float)Math.PI * 0.25f));
            scene.AddDSphere(new Vec3(-2.0f, 0.0f, 5.0f), 3, Material.GreenMaterial);
            scene.AddDSphere(new Vec3(4.0f, 0.0f, 5.0f), 4, new Material(Color.Blue, Color.White, Color.White, Color.White, 30, 0, 0, 0));

            DBox box1 = scene.AddDBox(new Vec3(-1.0f, -0.5f, -1.0f), 2.0f, 1.0f, 2.0f, new Material(Color.Green, Color.Green, Color.Green, Color.Green, 30, 0.7f, 0, 0));
            //box1.Transform(Matrix.GetRotationX((float)Math.PI * 0.125f));
            //box1.Transform(Matrix.GetRotationY((float)Math.PI * 0.125f));
            box1.Transform(Matrix.GetTranslation(0.0f, -1.2f, -2.8f));

            scene.geoMng.TransformAll();

            Renderer renderer = new Renderer();
            renderer.Render(scene, b);

            pictureBox.Image = b;

            //float aspectRatio = (float)width / height;
            //scene.cam.AdjustVerticalFov(aspectRatio);
        }
    }
}
