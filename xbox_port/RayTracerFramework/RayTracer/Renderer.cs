using System;
using System.Collections.Generic;
using System.Text;
using RayTracerFramework.Geometry;
using XNAColor = Microsoft.Xna.Framework.Graphics.Color;
using RTFColor = RayTracerFramework.Shading.Color;
using RayTracerFramework.Shading;

namespace RayTracerFramework.RayTracer {
    class Renderer {
        public static readonly int MaxRecursionDepth = 10;

        public static void Render(Scene scene, ref XNAColor[] target, int targetWidth, int targetHeight)
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

            RayIntersectionPoint firstIntersection;

            for (int y = 0; y < targetHeight; y++) { // pixel lines
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

                // Reset next ray direction, pixelCenterPos and rowStartPos
                rowStartPos += yOffset;
                rayWS.direction = Vec3.Normalize(rowStartPos -eyePos);
                pixelCenterPos.x = rowStartPos.x;
                pixelCenterPos.y = rowStartPos.y;
                pixelCenterPos.z = rowStartPos.z;
            }

        }
        
    }
}
