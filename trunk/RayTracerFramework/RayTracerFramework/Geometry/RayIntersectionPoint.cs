using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracerFramework.Geometry {
    class RayIntersectionPoint : IntersectionPoint {
        public float t;
        public IGeometricObject hitObject;

        public RayIntersectionPoint(Vec3 position, Vec3 normal, float t, IGeometricObject hitObject)
            : base(position, normal) {
            this.t = t;
            this.hitObject = hitObject;
        }
    }
}
