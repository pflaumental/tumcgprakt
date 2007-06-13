#region Using Statements
using System;
using System.Threading;
using XNAColor = Microsoft.Xna.Framework.Graphics.Color;
using GameServiceContainer = Microsoft.Xna.Framework.GameServiceContainer;
using RayTracerFramework.Geometry;
using RayTracerFramework.Utility;
using RayTracerFramework.Shading;
using RayTracerFramework.Loading;
#endregion

namespace RayTracerFramework.RayTracer
{
    class Raytracer
    {
        private Scene scene;
        private Camera cam;       

        /// <summary>
        /// The constructor is private: loading screens should
        /// be activated via the static GenerateImage method instead.
        /// </summary>
        public Raytracer()
        {
        }

        public void Setup(GameServiceContainer gameServiceContainer) {
            #region InitRayTracer
            Microsoft.Xna.Framework.Content.ContentManager content 
                    = new Microsoft.Xna.Framework.Content.ContentManager(gameServiceContainer);
            Vec3 camPos = new Vec3(0, 0, -5);
            Vec3 camLookAt = Vec3.Zero;
            cam = new Camera(camPos, camLookAt, Vec3.StdYAxis, Trigonometric.PI_QUARTER, 4f / 3f);

            scene = new Scene(cam, gameServiceContainer);

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

            scene.useCubeMap = true;

            scene.lightManager.AddWorldSpaceLight(l);
            scene.lightManager.AddWorldSpaceLight(l2);

            //scene.lightManager.AddWorldSpaceLight(l3);
            //scene.lightManager.AddWorldSpaceLight(l4);

            OBJLoader loader = new OBJLoader();
            DMesh mesh = loader.LoadFromFile("bunny_t4046.obj");

            scene.AddDMesh(mesh, Matrix.GetTranslation(-2, 0, 0));
            FastBitmap earthTexture = new FastBitmap(content.Load<Microsoft.Xna.Framework.Graphics.Texture2D>("Content/Textures/earth"));
            scene.AddDSphere(new Vec3(4.0f, 1.0f, -0.5f), 1.5f, new Material(Color.Blue, Color.Red, Color.Blue, Color.White, 10, true, true, 0.95f, 0.85f, null));
            scene.AddDSphere(new Vec3(6.0f, 2.5f, 5.0f), 4, new Material(Color.White, Color.White, Color.White, Color.White, 15, true, false, 0.6f, 0f, earthTexture));

            FastBitmap wallTexture = new FastBitmap(content.Load<Microsoft.Xna.Framework.Graphics.Texture2D>("Content/Textures/env2"));
            Matrix boxTransform = Matrix.GetRotationY(Trigonometric.PI_QUARTER);
            boxTransform *= Matrix.GetRotationX(Trigonometric.PI_QUARTER);
            boxTransform *= Matrix.GetTranslation(0.5f, -0.1f, 0f);
            scene.AddDBox(boxTransform, 1.5f, 1.5f, 1.5f, true, new Material(Color.White, Color.White, Color.White, Color.White, 30, false, false, 0.1f, 0f, wallTexture));
            scene.AddDBox(Matrix.GetTranslation(0f, -1.5f, 0f), 20f, 0.3f, 20f, false, new Material(Color.White, Color.White, Color.White, Color.White, 30, true, false, 0.1f, 0f, null));

            // Do not forget:
            scene.Setup();
            #endregion
        }

        public void GenerateImage(ref XNAColor[] resultImage, int iWidth, int iHeight)
        {
            cam.aspectRatio = ((float)iWidth) / iHeight;
            Renderer.Render(scene, ref resultImage, iWidth, iHeight);
        }
    }
}
