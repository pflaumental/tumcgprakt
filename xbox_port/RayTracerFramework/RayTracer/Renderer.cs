using System;
using System.Collections.Generic;
using System.Text;
using RayTracerFramework.Geometry;
using XNAColor = Microsoft.Xna.Framework.Graphics.Color;
using RTFColor = RayTracerFramework.Shading.Color;
using RayTracerFramework.Shading;
using System.Threading;

namespace RayTracerFramework.RayTracer {
    class Renderer {
        // Per scene and target data
        private Scene scene;
        private XNAColor[] target;
        private int targetWidth;
        private int targetHeight;

        // Per render-process data
        private Vec3 xOffset;
        private Vec3 yOffset;
        private Vec3 eyePos;
        private Vec3 topLeftPixelCenterPos;

        // Synchronized:
        private int nextLine;

        public Renderer(Scene scene, ref XNAColor[] target, int targetWidth, int targetHeight) { 
            this.scene = scene;
            this.target = target;
            this.targetWidth = targetWidth;
            this.targetHeight = targetHeight;
            this.nextLine = 0;
        }

        public static readonly int MaxRecursionDepth = 10;

        public void Render()
        {
            // View frustrum starts at 1.0f
            float viewPlaneWidth = scene.cam.GetViewPlaneWidth();
            float viewPlaneHeight = scene.cam.GetViewPlaneHeight();
            
            float pixelWidth = viewPlaneWidth / targetWidth;
            float pixelHeight = viewPlaneHeight / targetHeight;

            // Calculate orthonormal basis for the camera
            Vec3 camZ = scene.cam.ViewDir;
            Vec3 camX = Vec3.Cross(scene.cam.upDir, camZ);
            Vec3 camY = Vec3.Cross(camZ, camX);
            
            // Calculate pixel center offset vectors
            xOffset = pixelWidth * camX;
            yOffset = -pixelHeight * camY;

            // Go to center of first line
            eyePos = scene.cam.eyePos;
            topLeftPixelCenterPos = eyePos + camZ + camY * ((viewPlaneHeight - pixelHeight) * 0.5f);
            topLeftPixelCenterPos -= camX * ((viewPlaneWidth - pixelWidth) * 0.5f);

            Thread[] freds = new Thread[6];

            // Initialize threads
            for (int i = 0; i < freds.Length; i++) {
                freds[i] = new Thread(new ThreadStart(RenderMT));
                freds[i].Start();
            }

            for (int i = 0; i < freds.Length; i++) {
                freds[i].Join();
            }

        }

        private void RenderMT() {
            while (true) {
                int y;
                lock (this) {
                    if (nextLine == targetHeight)
                        break;
                    y = nextLine++;
                }

                // Reset next ray direction, pixelCenterPos and rowStartPos
                Vec3 pixelCenterPos = topLeftPixelCenterPos + yOffset * nextLine;
                Ray rayWS = new Ray(eyePos, Vec3.Normalize(pixelCenterPos - eyePos), 0);

                RayIntersectionPoint firstIntersection;

                for (int x = 0; x < targetWidth; x++) { // pixel columns                     
                    // Find nearest object intersection and Shade pixel      
                    RTFColor color;
                    if (scene.Intersect(rayWS, out firstIntersection)) {
                        IObject hitObject = (IObject)firstIntersection.hitObject;
                        color = hitObject.Shade(rayWS, firstIntersection, scene, 1.0f);
                    } else {
                        color = scene.GetBackgroundColor(rayWS);
                    }

                    target[y * targetWidth + x] = new XNAColor((byte)color.RedInt, (byte)color.GreenInt, (byte)color.BlueInt);

                    // Set next ray direction and pixelCenterPos
                    pixelCenterPos += xOffset;
                    rayWS.direction = Vec3.Normalize(pixelCenterPos - eyePos);
                }
            }
        }
        
    }
}
