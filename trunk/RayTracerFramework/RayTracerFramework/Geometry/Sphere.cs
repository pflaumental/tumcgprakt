using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracerFramework.Geometry {
    abstract class Sphere : IGeometricObject {
        protected Vec3 center; // world space
        protected float radius, radiusSq; // world space

        protected Sphere(Vec3 center, float radius) {
            this.center = center;
            this.radius = radius;
            this.radiusSq = radius * radius;
        }

        public bool Intersect(Ray ray) {
            Vec3 originToCenter = (center - ray.position);
            float t = Vec3.Dot(originToCenter, ray.direction);
            float distanceSq = originToCenter.LengthSq - t*t;
            return distanceSq <= radiusSq;
        }

        // Ray direction must be normalized
        public bool Intersect(Ray ray, out IntersectionPoint firstIntersection, out float t) {
            // o: (ray-)origin
            // c: center
            // x: point on ray-line for which o_x (= distance center-ray) has a right angle to ray
            // i: intersection point
            Vec3 o_cVec = center - ray.position;
            float o_cSq = o_cVec.LengthSq;
            if (o_cSq < radiusSq) { // ray starts inside sphere (exactly one intersection)
                float o_x = Vec3.Dot(o_cVec, ray.direction); // negative if ray points away from center
                //                       (      c_xSq        )
                float x_iSq = radiusSq - (o_cSq - (o_x * o_x));
                t = o_x + (float)Math.Sqrt(x_iSq);
                Vec3 intersectionPoint = ray.GetPoint(t);
                Vec3 normal = Vec3.Normalize(intersectionPoint - center);
                firstIntersection = new IntersectionPoint(intersectionPoint, normal);
                return true;
            }
            else {
                float o_x = Vec3.Dot(o_cVec, ray.direction); // negative if ray points away from center
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
                Vec3 intersectionPoint = ray.GetPoint(t);
                Vec3 normal = Vec3.Normalize(intersectionPoint - center);
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
            Vec3 o_cVec = center - ray.position;
            float o_cSq = o_cVec.LengthSq;
            if (o_cSq < radiusSq) { // ray starts inside sphere (exactly one intersection)
                float o_x = Vec3.Dot(o_cVec, ray.direction); // negative if ray points away from center
                //                       (      c_xSq        )
                float x_iSq = radiusSq - (o_cSq - (o_x * o_x));
                t1 = o_x + (float)Math.Sqrt(x_iSq);
                t2 = 0.0f;
                Vec3 intersectionPoint = ray.GetPoint(t1);
                Vec3 normal = Vec3.Normalize(intersectionPoint - center);
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
                Vec3 intersectionPoint1 = ray.GetPoint(t1);
                Vec3 intersectionPoint2 = ray.GetPoint(t2);
                Vec3 normal1 = Vec3.Normalize(intersectionPoint1 - center);
                Vec3 normal2 = Vec3.Normalize(intersectionPoint2 - center);
                intersections = new IntersectionPoint[] {new IntersectionPoint(intersectionPoint1, normal1),
                                                         new IntersectionPoint(intersectionPoint2, normal2) };
                return 2;
            }
        }

        // Only Translation and uniform scaling are handled correctly
        public void Transform(Matrix transformation) {
            Vec3 pointOnSphere = new Vec3(center.x + radius, center.y, center.z);
            Vec3 transformedPointOnSphere = new Vec3(Vec3.TransformPosition(pointOnSphere, transformation));

            center = new Vec3(Vec3.TransformPosition(center, transformation));                        
            radius = Vec3.GetLength(transformedPointOnSphere - center);
        }
    }
}
