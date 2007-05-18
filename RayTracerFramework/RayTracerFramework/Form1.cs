using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using RayTracerFramework.RayTracer;
using RayTracerFramework.Geometry;
using System.Drawing.Imaging;

namespace RayTracerFramework {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        private void btnRender_Click(object sender, EventArgs e) {
            Scene scene = new Scene();            
            pictureBox.Size = new Size(766, 430);
            Bitmap b = new Bitmap(pictureBox.Size.Width, pictureBox.Size.Height, PixelFormat.Format24bppRgb);// new Bitmap(100, 100);
            float aspectRatio = (float)b.Width / b.Height;
            //scene.cam.hFov /= 2;
            scene.cam.AdjustVerticalFov(aspectRatio);
            scene.cam.eyePos = new Vec3(0.0f, 0.0f, -5.0f);
            scene.cam.lookAtPos = new Vec3(0.0f, 0.0f, 0.0f);
            scene.geoMng.viewMatrix = scene.cam.GetViewMatrix();            

            scene.AddDSphere(new Vec3(0.6f, 0.0f, 2.0f), 1.5f, Color.Red);
            scene.AddDSphere(new Vec3(-1.0f, 0.0f, 6.0f), 3, Color.Yellow);

            scene.geoMng.TransformAll();

            Renderer renderer = new Renderer();
            renderer.Render(scene, b);

            pictureBox.Image = b;

            //float aspectRatio = (float)width / height;
            //scene.cam.AdjustVerticalFov(aspectRatio);
        }
    }
}