using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using RayTracerFramework.Geometry;
using System.Drawing.Imaging;
using RayTracerFramework.Shading;
using Color = RayTracerFramework.Shading.Color;
using System.Windows.Forms;

namespace RayTracerFramework.RayTracer {
    public class Renderer {        

        private ProgressBar progressBar;
        private StatusStrip statusBar;
        private PictureBox pictureBox;
        public bool cancelRendering;
        public bool renderingFinished;
        
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
        private int stride;
        private int waste;

        private volatile int nextLine;

        public Renderer(ProgressBar progressBar, StatusStrip statusBar, PictureBox pictureBox) {
            this.progressBar = progressBar;
            this.statusBar = statusBar;
            this.pictureBox = pictureBox;
            cancelRendering = false;
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

            waste = stride - targetWidth * 3;

            nextLine = 0;

            System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ThreadStart(MTRender));
            renderingFinished = false;
            thread.Start();
            while (!renderingFinished) {
                //if (cancelRendering)
                //    break;
                System.Threading.Thread.Sleep(1000);
                if (worker.CancellationPending) {
                    thread.Abort();
                    thread.Join();
                    return;
                }
                int progress = (int)((100f * nextLine) / targetHeight);
                worker.ReportProgress(progress);
            }
            thread.Join();
        }

        private void MTRender() {
            // Go to center of first line
            eyePos = scene.cam.eyePos;
            Vec3 rowStartPos = eyePos + camZ + camY * ((viewPlaneHeight - pixelHeight) * 0.5f);
            rowStartPos -= camX * ((viewPlaneWidth - pixelWidth) * 0.5f);
            Vec3 pixelCenterPos = new Vec3(rowStartPos);

            Ray rayWS = new Ray(
                    eyePos,
                    Vec3.Normalize(pixelCenterPos - eyePos),
                    0);
            
            RayIntersectionPoint firstIntersection;

            // Setup bitmap
            int rgbValuesPos = 0;            
            for (int y = 0; y < targetHeight; y++) { // pixel lines
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

                // Reset next ray direction, pixelCenterPos and rowStartPos
                rowStartPos += yOffset;
                rayWS.direction = Vec3.Normalize(rowStartPos - eyePos);
                pixelCenterPos.x = rowStartPos.x;
                pixelCenterPos.y = rowStartPos.y;
                pixelCenterPos.z = rowStartPos.z;

                rgbValuesPos += waste;

                nextLine++;
            }
            renderingFinished = true;
        }
    }
}
