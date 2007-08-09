using System;
using System.Collections.Generic;
using System.Text;
using RayTracerFramework.Geometry;

namespace RayTracerFramework.Utility {
    class Rnd {
        private static Random random = new Random(1);

        public static Vec3 RandomVec3() {
            #region alternative
            /*float f1 = (float)(random.NextDouble() * 2 -1);
            float f2 = (float)(random.NextDouble() * 2 - 1);
            float f3 = (float)(random.NextDouble() * 2 -1);
            return Vec3.Normalize(new Vec3(f1, f2, f3));*/
            /*
            double phi = random.NextDouble() * Settings.Render.Trigonometric.TwoPi;
            double theta = random.NextDouble() * Settings.Render.Trigonometric.Pi;

            float x = (float)(Math.Cos(phi) * Math.Sin(theta));
            float y = (float)(Math.Sin(phi) * Math.Cos(theta));
            float z = (float)Math.Cos(theta);
            return new Vec3(x, y, z);*/
            #endregion

            float r1 = (float)Rnd.RandomFloat();
            float r2 = (float)Rnd.RandomFloat();
            float twoPIr1 = 2 * Settings.Render.Trigonometric.Pi * r1;
            float oneMinusR2 = 1 - r2;
            float x = (float)(2 * Math.Cos(twoPIr1) * Math.Sqrt(r2 * oneMinusR2));
            float y = (float)(2 * Math.Sin(twoPIr1) * Math.Sqrt(r2 * oneMinusR2));
            float z = 1 - 2 * r2;
            return new Vec3(x, y, z);
        }

        public static float RandomFloat() {
            return (float)random.NextDouble();
        }

    }
}
