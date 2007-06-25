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
    }
}
