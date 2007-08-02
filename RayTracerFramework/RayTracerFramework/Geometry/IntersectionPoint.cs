using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracerFramework.Geometry {
    public class IntersectionPoint {
        public Vec3 position;
        public Vec3 normal;

        public IntersectionPoint(Vec3 position, Vec3 normal) {
            this.position = position;
            this.normal = normal;
        }
    }
}
