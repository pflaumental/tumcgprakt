using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracerFramework.Geometry {
    public class RayIntersectionPoint : IntersectionPoint {
        public float t;
        public IIntersectable hitObject;
        public Vec2 textureCoordinates;

        public RayIntersectionPoint(
                Vec3 position, 
                Vec3 normal, 
                float t, 
                IIntersectable hitObject,
                Vec2 textureCoordinates) : base(position, normal) {
            this.t = t;
            this.hitObject = hitObject;
            this.textureCoordinates = textureCoordinates;
        }
    }
}
