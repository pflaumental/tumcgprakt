using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracerFramework.Geometry {

    // A box with lower left for corner centered at the object space origin (0,0,0) and
    // the upper right back corner at (dx, dy, dz)
    abstract class Box : IGeometricObject {        
        protected float dx, dy, dz;

        protected Matrix transform;
        protected Matrix invTransform;


        protected Box(Vec3 position, float width, float height, float depth) {            
            this.dx = width;
            this.dy = height;
            this.dz = depth;
        }

        public void Transform(Matrix transformation)
        {
            this.transform *= transformation;
            this.invTransform = Matrix.Invert(this.transform);
        }

        public void Transform(Matrix transformation, Matrix invTransformation)
        {
            this.transform *= transformation;
            this.invTransform = invTransformation * this.invTransform;
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
