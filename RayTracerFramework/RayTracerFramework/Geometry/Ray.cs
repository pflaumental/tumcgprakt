using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracerFramework.Geometry {
    class Ray {
        public readonly static float positionEpsilon = 0.0001f;

        public Vec3 position;
        public Vec3 direction;  // Must be normalized
        public int recursionDepth;

        public Ray(Vec3 position, Vec3 direction, int recursionDepth) {
            this.position = position;
            this.direction = direction;
            this.recursionDepth = recursionDepth;
        }

        public Vec3 GetPoint(float t) {
            return position + direction * t;
        }

        public Ray Transform(Matrix transformation) {
            return new Ray(Vec3.TransformPosition3(position, transformation), Vec3.TransformNormal3n(direction, transformation), recursionDepth);
        }
    }
}
