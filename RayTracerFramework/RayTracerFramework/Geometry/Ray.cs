using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracerFramework.Geometry {
    class Ray {        
        public Vec3 position;
        public Vec3 direction;  // Must be normalized

        public Ray(Vec3 position, Vec3 direction) {
            this.position = position;
            this.direction = direction;
        }

        public Vec3 GetPoint(float t) {
            return position + direction * t;
        }
    }
}
