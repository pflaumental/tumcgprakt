using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracerFramework.Geometry {
    abstract class Sphere : IGeometricObject {
        //protected Vec3 center;
        // Center is implicitly at (0,0,0)
        protected float radius, radiusSq;
        protected Matrix transform;
        protected Matrix invTransform;

        protected Sphere(Vec3 center, float radius) {
            this.radius = radius;
            this.radiusSq = radius * radius;
            this.transform = Matrix.GetTranslation(center);
            this.invTransform = Matrix.GetTranslation(-center);
        }

        protected Sphere(float radius, Matrix transform, Matrix invTransform)
        {
            this.radius = radius;
            this.radiusSq = radius * radius;
            this.transform = transform;
            this.invTransform =invTransform;
        }

        public bool Intersect(Ray ray) {
            // o: (ray-)origin
            // c: center
            // x: point on ray-line for which o_x (= distance center-ray) has a right angle to ray
            // i: intersection point

            // Transform ray to object space
            Ray rayOS = ray.Transform(invTransform);
            Vec3 o_cVec = Vec3.Zero - rayOS.position; // Center at (0,0,0)
            float o_cSq = o_cVec.LengthSq;
            if (o_cSq < radiusSq)// ray starts inside sphere (exactly one intersection)
                return true;
            else
            {
                float o_x = Vec3.Dot(o_cVec, rayOS.direction); // negative if ray points away from center
                if (o_x < 0.0f)
                    return false;
                //                       (      c_xSq        )
                float x_iSq = radiusSq - (o_cSq - (o_x * o_x));
                if (x_iSq < 0.0f)
                    return false;
                return true;
            }
        }

        // Ray direction must be normalized
        public bool Intersect(Ray ray, out IntersectionPoint firstIntersection, out float t) {
            // o: (ray-)origin
            // c: center
            // x: point on ray-line for which o_x (= distance center-ray) has a right angle to ray
            // i: intersection point
            
            // Transform ray to object space
            Ray rayOS = ray.Transform(invTransform);
            Vec3 o_cVec = Vec3.Zero - rayOS.position; // Center at (0,0,0)
            float o_cSq = o_cVec.LengthSq;
            if (o_cSq < radiusSq) { // ray starts inside sphere (exactly one intersection)
                float o_x = Vec3.Dot(o_cVec, rayOS.direction); // negative if ray points away from center
                //                       (      c_xSq        )
                float x_iSq = radiusSq - (o_cSq - (o_x * o_x));
                t = o_x + (float)Math.Sqrt(x_iSq);
                Vec3 iPos = rayOS.GetPoint(t);
                Vec3 c_iVec = iPos - Vec3.Zero; // Center at (0,0,0)
                Vec3 intersectionPoint = Vec3.TransformPosition3(iPos, transform);
                Vec3 normal = Vec3.TransformNormal3n(c_iVec, transform);
                firstIntersection = new IntersectionPoint(intersectionPoint, normal);
                return true;
            }
            else {
                float o_x = Vec3.Dot(o_cVec, rayOS.direction); // negative if ray points away from center
                if (o_x < 0.0f) {
                    t = 0.0f;
                    firstIntersection = null;
                    return false;
                }
                //                       (      c_xSq        )
                float x_iSq = radiusSq - (o_cSq - (o_x * o_x));
                if (x_iSq < 0.0f) {
                    t = 0.0f;
                    firstIntersection = null;
                    return false;
                }
                // Sphere is hit
                t = o_x - (float)Math.Sqrt(x_iSq);
                Vec3 iPos = rayOS.GetPoint(t);
                Vec3 c_iVec = iPos - Vec3.Zero; // Center at (0,0,0)
                Vec3 intersectionPoint = Vec3.TransformPosition3(iPos, transform);
                Vec3 normal = Vec3.TransformNormal3n(c_iVec, transform);
                firstIntersection = new IntersectionPoint(intersectionPoint, normal);
                return true;
            }
        }

        // Returns the number of intersections
        // Ray direction must be normalized
        public int Intersect(Ray ray, out IntersectionPoint[] intersections, out float t1, out float t2) {
            // o: (ray-)origin
            // c: center
            // x: point on ray-line for which o_x (= distance center-ray) has a right angle to ray
            // i: intersection point

            // Transform ray to object space
            Ray rayOS = ray.Transform(invTransform);
            Vec3 o_cVec = Vec3.Zero - rayOS.position; // Center at (0,0,0)
            float o_cSq = o_cVec.LengthSq;
            if (o_cSq < radiusSq) { // ray starts inside sphere (exactly one intersection)
                float o_x = Vec3.Dot(o_cVec, rayOS.direction); // negative if ray points away from center
                //                       (      c_xSq        )
                float x_iSq = radiusSq - (o_cSq - (o_x * o_x));
                t1 = o_x + (float)Math.Sqrt(x_iSq);
                t2 = 0.0f;
                Vec3 iPos = rayOS.GetPoint(t1);
                Vec3 c_iVec = iPos - Vec3.Zero; // Center at (0,0,0)
                Vec3 intersectionPoint = Vec3.TransformPosition3(iPos, transform);
                Vec3 normal = Vec3.TransformNormal3n(c_iVec, transform);                
                intersections = new IntersectionPoint[]{ new IntersectionPoint(intersectionPoint, normal) };
                return 1;
            }
            else {
                float o_x = Vec3.Dot(o_cVec, ray.direction); // negative if ray points away from center
                if (o_x < 0.0f) {
                    t1 = t2 = 0.0f;
                    intersections = null;
                    return 0;
                }
                //                       (      c_xSq        )
                float x_iSq = radiusSq - (o_cSq - (o_x * o_x));
                if (x_iSq < 0.0f) {
                    t1 = t2 = 0.0f;
                    intersections = null;
                    return 0;
                }
                float x_i = (float)Math.Sqrt(x_iSq);
                t1 = o_x - x_i;
                t2 = o_x + x_i;

                Vec3 i1Pos = rayOS.GetPoint(t1);
                Vec3 i2Pos = rayOS.GetPoint(t2);
                Vec3 c_i1Vec = i1Pos - Vec3.Zero; // Center at (0,0,0)
                Vec3 c_i2Vec = i2Pos - Vec3.Zero; // Center at (0,0,0)
                Vec3 intersectionPoint1 = Vec3.TransformPosition3(i1Pos, transform);
                Vec3 intersectionPoint2 = Vec3.TransformPosition3(i2Pos, transform);
                Vec3 normal1 = Vec3.TransformNormal3n(c_i1Vec, transform);
                Vec3 normal2 = Vec3.TransformNormal3n(c_i2Vec, transform);
                intersections = new IntersectionPoint[] {new IntersectionPoint(intersectionPoint1, normal1),
                                                         new IntersectionPoint(intersectionPoint2, normal2) };
                return 2;
            }
        }

        public void Transform(Matrix transformation) {
            this.transform *= transformation;
            this.invTransform = Matrix.Invert(this.transform);
        }

        public void Transform(Matrix transformation, Matrix invTransformation)
        {
            this.transform *= transformation;
            this.invTransform = invTransformation * this.invTransform;
        }
    }
}
