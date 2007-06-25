using System;
using System.Collections.Generic;
using System.Text;
using RayTracerFramework.Geometry;

namespace RayTracerFramework.Utility {
    class Rnd {
        private static Random random = new Random(1);

        public static Vec3 RandomVec3() {
            float f1 = (float)(random.NextDouble() * 2 -1);
            float f2 = (float)(random.NextDouble() * 2 - 1);
            float f3 = (float)(random.NextDouble() * 2 -1);
            return Vec3.Normalize(new Vec3(f1, f2, f3));
        }

        public static float RandomFloat() {
            return (float)random.NextDouble();
        }

    }
}
