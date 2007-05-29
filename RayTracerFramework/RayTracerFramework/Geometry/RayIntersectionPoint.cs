using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracerFramework.Geometry {
    class RayIntersectionPoint : IntersectionPoint {
        public float t;
        public IIntersectable hitObject;

        public RayIntersectionPoint(Vec3 position, Vec3 normal, float t, IIntersectable hitObject)
            : base(position, normal) {
            this.t = t;
            this.hitObject = hitObject;
        }
    }
}
