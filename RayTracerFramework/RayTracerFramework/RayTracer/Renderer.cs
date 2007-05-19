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


        public Renderer() {
        }

        public void Render(Scene scene, Bitmap target) {
            int targetWidth = target.Width;
            int targetHeight = target.Height;

            //// Prepare Bitmap
            //BitmapData bitmapData;
            //target.UnlockBits(bitmapData);
            //bitmapData.

            // View frustrum starts at 1.0f
            //float viewPlaneWidth = (float)Math.Tan(scene.cam.hFov) * 2;
            //float viewPlaneHeight = (float)Math.Tan(scene.cam.vFov) * 2;
            float viewPlaneWidth = scene.cam.GetViewPlaneWidth();
            float viewPlaneHeight = scene.cam.GetViewPlaneHeight();
            
            float pixelWidth = viewPlaneWidth / targetWidth;
            float pixelHeight = viewPlaneHeight / targetHeight;

            //// Calculate orthonormal basis for the camera
            //Vec3 camZ = scene.cam.ViewDir;
            //Vec3 camX = Vec3.Cross(scene.cam.upDir, camZ);
            //Vec3 camY = Vec3.Cross(camZ, camX);

            // Go to center of first line
            //Vec3 eyePos = scene.cam.eyePos;
            Vec3 rowStartPos = Vec3.StdZAxis + Vec3.StdYAxis * ((viewPlaneHeight - pixelHeight) * 0.5f);
            rowStartPos -= Vec3.StdXAxis * ((viewPlaneWidth - pixelWidth) * 0.5f);
            Vec3 pixelCenterPos = new Vec3(rowStartPos);

            Vec3 rayDir = Vec3.Normalize(pixelCenterPos);
            Ray ray = new Ray(Vec3.Zero, rayDir);            

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
            for (int y = 0; y < targetHeight; y++) { // pixel lines
                for (int x = 0; x < targetWidth; x++) { // pixel columns                     
                    // Find nearest object intersection
                    scene.Intersect(ray, out firstIntersection);

                    // Shade pixel      
                    Color color;
                    if (firstIntersection != null) {
                        IObject hitObject = (IObject)firstIntersection.hitObject;
                        color = hitObject.Shade(ray, firstIntersection, scene, 1.0f);
                    } else
                        color = scene.backgroundColor;

                    rgbValues[rgbValuesPos] = color.BlueInt;
                    rgbValues[rgbValuesPos + 1] = color.GreenInt;
                    rgbValues[rgbValuesPos + 2] = color.RedInt;                    

                    rgbValuesPos += 3;

                    // Set next ray direction and pixelCenterPos
                    pixelCenterPos.x += pixelWidth;
                    ray.direction = Vec3.Normalize(pixelCenterPos);
                }
                // Reset next ray direction, pixelCenterPos and rowStartPos
                rowStartPos.y -= pixelHeight;
                ray.direction = Vec3.Normalize(rowStartPos);
                pixelCenterPos.x = rowStartPos.x;
                pixelCenterPos.y = rowStartPos.y;
                pixelCenterPos.z = rowStartPos.z;

                rgbValuesPos += waste;
            }
            System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, bitmapDataAddress, rgbValuesLength);
            target.UnlockBits(bitmapData);
        }
        


    }



}
