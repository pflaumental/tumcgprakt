using System;
using System.Collections.Generic;
using System.Text;
using RayTracerFramework.Shading;
using RayTracerFramework.Geometry;

namespace RayTracerFramework.Utility {
    class LightHelper {
        public static float Fresnel(Material material, Vec3 incomingDir, Vec3 normal) {
            return material.r0 + material.oneMinusR0 * (float)Math.Pow(1f - Vec3.Dot(-incomingDir, normal), 5f);
        }

        public static Vec3 GetUniformRndDirection(Vec3 normal) {
            Vec3 direction = Rnd.RandomVec3();
            if (Vec3.Dot(direction, normal) < 0)
                direction = -direction;
            return direction;
        }

        public static Vec3 GetRndDirectionCosWeightedPDF(Vec3 normal) {
            float r1 = Rnd.RandomFloat();
            float r2 = Rnd.RandomFloat();

            // x, y, z relative to standard basis
            float x = (float)(Math.Cos(Trigonometric.TWO_PI * r1) * Math.Sqrt(1 - r2));
            float y = (float)(Math.Sin(Trigonometric.TWO_PI * r1) * Math.Sqrt(1 - r2));
            float z = (float)Math.Sqrt(r2);
            return null;

            
        }
    }
}
