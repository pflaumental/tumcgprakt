using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using RayTracerFramework.Geometry;
using System.Drawing.Imaging;
using RayTracerFramework.Shading;
using Color = RayTracerFramework.Shading.Color;
using System.Windows.Forms;
using System.Threading;

namespace RayTracerFramework.RayTracer {
    public class Renderer {                      
        // Shared MT-Render vars
        private Scene scene;
        private byte[] rgbValues;
        private int rgbValuesLength;
        private int targetWidth;
        private int targetHeight;
        private float viewPlaneWidth;
        private float viewPlaneHeight;
        private float pixelWidth;
        private float pixelHeight;
        private Vec3 camZ;
        private Vec3 camX;
        private Vec3 camY;
        private Vec3 xOffset;
        private Vec3 yOffset;
        private Vec3 eyePos;
        private Vec3 firstPixelPos;
        private int stride;

        private volatile int nextLine;
        private volatile bool renderingFinished;

        public Renderer() {
            renderingFinished = false;
        }

        public void Render(
                Scene scene, 
                byte[] rgbValues,
                int rgbValuesLength,
                int stride,
                int targetWidth,
                int targetHeight,
                System.ComponentModel.BackgroundWorker worker) {
            this.scene = scene;
            this.rgbValues = rgbValues;
            this.rgbValuesLength = rgbValuesLength;
            this.stride = stride;
            this.targetWidth = targetWidth;
            this.targetHeight = targetHeight;
            
            // View frustrum starts at 1.0f
            viewPlaneWidth = scene.cam.GetViewPlaneWidth();
            viewPlaneHeight = scene.cam.GetViewPlaneHeight();
            
            pixelWidth = viewPlaneWidth / targetWidth;
            pixelHeight = viewPlaneHeight / targetHeight;

            // Calculate orthonormal basis for the camera
            camZ = scene.cam.ViewDir;
            camX = Vec3.Cross(scene.cam.upDir, camZ);
            camY = Vec3.Cross(camZ, camX);
            
            // Calculate pixel center offset vectors
            xOffset = pixelWidth * camX;
            yOffset = -pixelHeight * camY;

            eyePos = scene.cam.eyePos;

            firstPixelPos = eyePos + camZ + camY * ((viewPlaneHeight - pixelHeight) * 0.5f);
            firstPixelPos -= camX * ((viewPlaneWidth - pixelWidth) * 0.5f);

            nextLine = 0;
            
            int numThreads = System.Environment.ProcessorCount;
            Thread[] threads = new Thread[numThreads];
            
            for (int i = 0; i < numThreads; i++) {
                threads[i] = new Thread(new ThreadStart(MTRender));
                threads[i].Priority = ThreadPriority.BelowNormal;
            }

            renderingFinished = false;

            foreach (Thread thread in threads) {
                thread.Start();
            }
            
            while (!renderingFinished) {
                Thread.Sleep(1000);
                if (worker.CancellationPending) {
                    foreach (Thread thread in threads) {
                        thread.Abort();
                    }
                    foreach (Thread thread in threads) {
                        thread.Join();
                    }
                    return;
                }
                int progress = (int)((100f * nextLine) / targetHeight);
                worker.ReportProgress(progress);
            }

            foreach (Thread thread in threads) {
                thread.Join();
            }
        }

        private void MTRender() {                        
            Ray rayWS = new Ray(
                    eyePos,
                    null,
                    0);
            RayIntersectionPoint firstIntersection;

            // Setup bitmap
            int rgbValuesPos = 0;
            int y;
            Vec3 rowStartPos;
            Vec3 pixelCenterPos = new Vec3();
            #pragma warning disable 420
            while((y = Interlocked.Increment(ref nextLine)) < targetHeight) { // pixel lines
            #pragma warning restore 420
                // Calculate ray direction, pixelCenterPos and rowStartPos
                rowStartPos = firstPixelPos + y * yOffset;
                rayWS.direction = Vec3.Normalize(rowStartPos - eyePos);
                pixelCenterPos.x = rowStartPos.x;
                pixelCenterPos.y = rowStartPos.y;
                pixelCenterPos.z = rowStartPos.z;

                rgbValuesPos = y * stride;

                for (int x = 0; x < targetWidth; x++) { // pixel columns                     
                    // Find nearest object intersection and Shade pixel      
                    Color color;
                    if (scene.Intersect(rayWS, out firstIntersection)) {
                        IObject hitObject = (IObject)firstIntersection.hitObject;
                        color = hitObject.Shade(rayWS, firstIntersection, scene, 1.0f);
                    } else {
                        color = scene.GetBackgroundColor(rayWS);
                    }
                    rgbValues[rgbValuesPos] = color.BlueInt;
                    rgbValues[rgbValuesPos + 1] = color.GreenInt;
                    rgbValues[rgbValuesPos + 2] = color.RedInt;

                    rgbValuesPos += 3;

                    // Set next ray direction and pixelCenterPos
                    pixelCenterPos += xOffset;
                    rayWS.direction = Vec3.Normalize(pixelCenterPos - eyePos);
                }
            }
            renderingFinished = true;
        }
    }
}
