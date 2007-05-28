using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using RayTracerFramework.Geometry;
using System.Drawing.Imaging;
using RayTracerFramework.Shading;
using Color = RayTracerFramework.Shading.Color;

namespace RayTracerFramework.RayTracer {
    class Renderer {
        public static readonly int MaxRecursionDepth = 10;

        private System.Windows.Forms.ProgressBar progressBar;

        public Renderer(System.Windows.Forms.ProgressBar progressBar) {
            this.progressBar = progressBar;
        }

        public void Render(Scene scene, Bitmap target) {
            int targetWidth = target.Width;
            int targetHeight = target.Height;

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
            Vec3 xOffset = pixelWidth * camX;
            Vec3 yOffset = -pixelHeight * camY;

            // Go to center of first line
            Vec3 eyePos = scene.cam.eyePos;
            Vec3 rowStartPos = eyePos + camZ + camY * ((viewPlaneHeight - pixelHeight) * 0.5f);
            rowStartPos -= camX * ((viewPlaneWidth - pixelWidth) * 0.5f);
            Vec3 pixelCenterPos = new Vec3(rowStartPos);

            Matrix inverseView = scene.cam.GetInverseViewMatrix();
            Ray rayWS = new Ray(
                    Vec3.TransformPosition3(Vec3.Zero, inverseView),
                    Vec3.TransformNormal3n(pixelCenterPos, inverseView),
                    0);

            // Setup bitmap
            BitmapData bitmapData = target.LockBits(new Rectangle(0, 0, targetWidth, targetHeight),
                    ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            IntPtr bitmapDataAddress = bitmapData.Scan0;
            int stride = bitmapData.Stride;
            int rgbValuesLength = stride * targetHeight;            
            int waste = stride - targetWidth * 3;
            byte[] rgbValues = new byte[rgbValuesLength];
            int rgbValuesPos = 0;

            RayIntersectionPoint firstIntersection;

            scene.Setup();

            progressBar.Minimum = 0;
            progressBar.Maximum = targetHeight;

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
                    rgbValues[rgbValuesPos]     = color.BlueInt;
                    rgbValues[rgbValuesPos + 1] = color.GreenInt;
                    rgbValues[rgbValuesPos + 2] = color.RedInt;                    

                    rgbValuesPos += 3;

                    // Set next ray direction and pixelCenterPos
                    pixelCenterPos += xOffset;
                    rayWS.direction = Vec3.Normalize(pixelCenterPos - eyePos);
                }
                // Reset next ray direction, pixelCenterPos and rowStartPos
                rowStartPos += yOffset;
                rayWS.direction = Vec3.Normalize(rowStartPos -eyePos);
                pixelCenterPos.x = rowStartPos.x;
                pixelCenterPos.y = rowStartPos.y;
                pixelCenterPos.z = rowStartPos.z;

                rgbValuesPos += waste;

                progressBar.Value = y;
            }
            System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, bitmapDataAddress, rgbValuesLength);
            target.UnlockBits(bitmapData);            
        }
        
    }
}
