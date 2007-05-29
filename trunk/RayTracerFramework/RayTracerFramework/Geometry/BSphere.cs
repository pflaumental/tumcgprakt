using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracerFramework.Geometry {
    class BSphere : IIntersectable {
        public readonly Vec3 center;
        public readonly float radius, radiusSq;

        public BSphere(Vec3 center, float radius) {
            this.center = center;
            this.radius = radius;
            this.radiusSq = radius * radius;
        }

        public bool Intersect(Ray ray) {
            // o: (ray-)origin
            // c: center
            // x: point on ray-line for which o_x (= distance center-ray) has a right angle to ray
            // i: intersection point

            Vec3 o_cVec = center - ray.position;
            float o_cSq = o_cVec.LengthSq;
            if (o_cSq < radiusSq)// ray starts inside sphere (exactly one intersection)
                return true;
            else {
                float o_x = Vec3.Dot(o_cVec, ray.direction); // negative if ray points away from center
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
        public bool Intersect(Ray ray, out RayIntersectionPoint firstIntersection) {
            // o: (ray-)origin
            // c: center
            // x: point on ray-line for which o_x (= distance center-ray) has a right angle to ray
            // i: intersection point

            Vec3 o_cVec = center - ray.position;
            float t = 0.0f;
            float o_cSq = o_cVec.LengthSq;
            if (o_cSq < radiusSq) { // ray starts inside sphere (exactly one intersection)
                float o_x = Vec3.Dot(o_cVec, ray.direction); // negative if ray points away from center
                //                       (      c_xSq        )
                float x_iSq = radiusSq - (o_cSq - (o_x * o_x));
                t = o_x + (float)Math.Sqrt(x_iSq);
                Vec3 iPos = ray.GetPoint(t);
                Vec3 c_iVec = iPos - center;
                firstIntersection = new RayIntersectionPoint(iPos, Vec3.Normalize(-c_iVec), t, this);
                return true;
            } else {
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
                Vec3 iPos = ray.GetPoint(t);
                Vec3 c_iVec = iPos - center;
                firstIntersection = new RayIntersectionPoint(iPos, Vec3.Normalize(c_iVec), t, this);
                return true;
            }
        }

        // Returns the number of intersections
        // Ray direction must be normalized
        public int Intersect(Ray ray, ref SortedList<float, RayIntersectionPoint> intersections) {
            // o: (ray-)origin
            // c: center
            // x: point on ray-line for which o_x (= distance center-ray) has a right angle to ray
            // i: intersection point

            Vec3 o_cVec = center - ray.position;
            float t1 = 0.0f, t2 = 0.0f;
            float o_cSq = o_cVec.LengthSq;
            if (o_cSq < radiusSq) { // ray starts inside sphere (exactly one intersection)
                float o_x = Vec3.Dot(o_cVec, ray.direction); // negative if ray points away from center
                //                       (      c_xSq        )
                float x_iSq = radiusSq - (o_cSq - (o_x * o_x));
                t1 = o_x + (float)Math.Sqrt(x_iSq);
                Vec3 iPos = ray.GetPoint(t1);
                Vec3 c_iVec = iPos - center;
                intersections.Add(t1, new RayIntersectionPoint(iPos, Vec3.Normalize(c_iVec), t1, this));
                return 1;
            } else {
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

                Vec3 i1Pos = ray.GetPoint(t1);
                Vec3 i2Pos = ray.GetPoint(t2);
                Vec3 c_i1Vec = i1Pos - center;
                Vec3 c_i2Vec = i2Pos - center;
                intersections.Add(t1, new RayIntersectionPoint(i1Pos, Vec3.Normalize(c_i1Vec), t1, this));
                intersections.Add(t2, new RayIntersectionPoint(i1Pos, Vec3.Normalize(c_i2Vec), t2, this));
                return 2;
            }
        }
    }
}
