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
            Vec3 originToCenter = center - ray.position;
            float distOriginCenterSq = originToCenter.LengthSq;
            if (distOriginCenterSq < radiusSq) { // ray starts inside sphere (exactly one intersection)
                float prjOtcRay = Vec3.Dot(originToCenter, ray.direction);
                float diffRadiusDistCenRaySq = radiusSq - distOriginCenterSq + (prjOtcRay * prjOtcRay) / ray.direction.LengthSq;
                t = prjOtcRay - (float)Math.Sqrt(diffRadiusDistCenRaySq);
                Vec3 intersectionPoint = ray.GetPoint(t);
                Vec3 normal = Vec3.Normalize(intersectionPoint - center);
                firstIntersection = new IntersectionPoint(intersectionPoint, normal);
                return true;
            }
            else {
                float prjOtcRay = Vec3.Dot(originToCenter, ray.direction);
                if (prjOtcRay < 0.0f) {
                    t = 0.0f;
                    firstIntersection = null;
                    return false;
                }
                float distCenRaySq = distOriginCenterSq - (prjOtcRay * prjOtcRay) / ray.direction.LengthSq; // TODO: remove division
                float diffRadiusDistCenRaySq = radiusSq - distCenRaySq;
                if (diffRadiusDistCenRaySq < 0.0f) {
                    t = 0.0f;
                    firstIntersection = null;
                    return false;
                }
                float diffRadiusDistCenRay = (float)Math.Sqrt(diffRadiusDistCenRaySq);
                t = prjOtcRay - diffRadiusDistCenRay;
                Vec3 intersectionPoint = ray.GetPoint(t);
                Vec3 normal = Vec3.Normalize(intersectionPoint - center);
                firstIntersection = new IntersectionPoint(intersectionPoint, normal);
                return true;
            }
        }

        // Returns the number of intersections
        // Ray direction must be normalized
        public int Intersect(Ray ray, out IntersectionPoint[] intersections, out float t1, out float t2) {
            Vec3 originToCenter = center - ray.position;
            float distOriginCenterSq = originToCenter.LengthSq;
            if (distOriginCenterSq < radiusSq) { // ray starts inside sphere (exactly one intersection)
                float prjOtcRay = Vec3.Dot(originToCenter, ray.direction);
                float diffRadiusDistCenRaySq = radiusSq - distOriginCenterSq + (prjOtcRay * prjOtcRay) / ray.direction.LengthSq;
                t1 = prjOtcRay - (float)Math.Sqrt(diffRadiusDistCenRaySq);
                t2 = 0.0f;
                Vec3 intersectionPoint = ray.GetPoint(t1);
                Vec3 normal = Vec3.Normalize(intersectionPoint - center);
                intersections = new IntersectionPoint[]{ new IntersectionPoint(intersectionPoint, normal) };
                return 1;
            }
            else {
                float prjOtcRay = Vec3.Dot(originToCenter, ray.direction);
                if (prjOtcRay < 0.0f) {
                    t1 = t2 = 0.0f;
                    intersections = null;
                    return 0;
                }
                float distCenRaySq = distOriginCenterSq - (prjOtcRay * prjOtcRay) / ray.direction.LengthSq;
                float diffRadiusDistCenRaySq = radiusSq - distCenRaySq;
                if (diffRadiusDistCenRaySq < 0.0f) {
                    t1 = t2 = 0.0f;
                    intersections = null;
                    return 0;
                }
                float diffRadiusDistCenRay = (float)Math.Sqrt(diffRadiusDistCenRaySq);
                t1 = prjOtcRay - diffRadiusDistCenRay;
                t2 = prjOtcRay + diffRadiusDistCenRay;
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
