using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracerFramework.Geometry {
    class RayIntersectionPoint : IntersectionPoint {
        public float t;

        public RayIntersectionPoint(Vec3 position, Vec3 normal, float t)
            : base(position, normal) {
            this.t = t;
        }
    }
}
