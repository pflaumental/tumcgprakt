using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using RayTracerFramework.Geometry;
using System.Drawing.Imaging;
using RayTracerFramework.Shading;

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
            float viewPlaneWidth = (float)Math.Tan(scene.cam.hFov) * 2;
            float viewPlaneHeight = (float)Math.Tan(scene.cam.vFov) * 2;
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

            IntersectionPoint firstIntersection;
            float nearestT = float.PositiveInfinity;
            float currentT;
            IObject nearestObject = null;
            IntersectionPoint nearestIntersection = null;

            // Setup bitmap
            BitmapData bitmapData = target.LockBits(new Rectangle(0, 0, targetWidth, targetHeight),
                    ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            IntPtr bitmapDataAddress = bitmapData.Scan0;
            int stride = bitmapData.Stride;
            int rgbValuesLength = stride * targetHeight;            
            int waste = stride - targetWidth * 3;
            byte[] rgbValues = new byte[rgbValuesLength];
            int rgbValuesPos = 0;

            for (int y = 0; y < targetHeight; y++) { // pixel lines
                for (int x = 0; x < targetWidth; x++) { // pixel columns                     
                    // Find nearest object intersection
                    foreach (IObject geoObj in scene.geoMng.TransformedObjects) {
                        if (geoObj.Intersect(ray, out firstIntersection, out currentT)) {
                            if (currentT < nearestT) {
                                nearestT = currentT;
                                nearestObject = geoObj; ;
                                nearestIntersection = firstIntersection;
                            }
                        }
                    }
                    // Shade pixel      
                    Color color;
                    if (nearestObject != null)
                        color = nearestObject.Shade(ray, nearestIntersection);
                    else
                        color = scene.backgroundColor;

                    rgbValues[rgbValuesPos] = color.B;
                    rgbValues[rgbValuesPos + 1] = color.G;
                    rgbValues[rgbValuesPos + 2] = color.R;                    

                    rgbValuesPos += 3;

                    nearestIntersection = null;
                    nearestObject = null;
                    nearestT = float.PositiveInfinity;

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
