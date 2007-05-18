using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracerFramework.Geometry {

    // A box with lower left top corner centered at the object space origin (0,0,0) and
    // the upper right back corner at (dx, dy, dz)
    abstract class Box : IGeometricObject {
        protected Vec3 position; // Position of the lower left front corner
        protected float dx, dy, dz;

        protected Matrix transform;
        protected Matrix transformInv;


        protected Box(Vec3 position, float width, float height, float depth) {
            this.position = position;
            this.dx = width;
            this.dy = height;
            this.dz = depth;
        }

        public void Transform(Matrix transformation) {
            transform = transformation;
            // transformInv = Matrix.Inverse(transformation);
            
        }
       
        public bool Intersect(Ray ray) {
            return true;
            // Transform ray into object space
            //Ray rayTransformed = Vec3.TransformNormal(ray.direction, transformInv);

        }

        public bool Intersect(Ray ray, out IntersectionPoint firstIntersection, out float t) {
            throw new Exception("The method or operation is not implemented.");
        }

        public int Intersect(Ray ray, out IntersectionPoint[] intersections, out float t1, out float t2) {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
