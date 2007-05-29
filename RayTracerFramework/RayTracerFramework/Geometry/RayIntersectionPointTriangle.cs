using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracerFramework.Geometry {
    class RayIntersectionPointTriangle : RayIntersectionPoint {
        public Triangle hitTriangle;
        public float u, v;

        public RayIntersectionPointTriangle(Vec3 position, Vec3 normal, float t, IGeometricObject hitObject,
                                            Triangle hitTriangle, float u, float v)
            : base(position, normal, t, hitObject) {
            this.hitTriangle = hitTriangle;
            this.u = u;
            this.v = v;
        }
    }
}
