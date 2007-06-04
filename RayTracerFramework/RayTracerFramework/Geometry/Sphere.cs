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
        protected BSphere boundingSphere;

        protected Sphere(Vec3 center, float radius) {           
            this.radius = radius;
            this.radiusSq = radius * radius;
            this.transform = Matrix.GetTranslation(center);
            this.invTransform = Matrix.GetTranslation(-center);
            this.boundingSphere = new BSphere(center, radius, radiusSq);
        }

        protected Sphere(float radius, Matrix transform, Matrix invTransform, BSphere bSphere)
        {
            this.radius = radius;
            this.radiusSq = radius * radius;
            this.transform = transform;
            this.invTransform = invTransform;
            this.boundingSphere = bSphere;
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
        public bool Intersect(Ray ray, out RayIntersectionPoint firstIntersection) {
            // o: (ray-)origin
            // c: center
            // x: point on ray-line for which o_x (= distance center-ray) has a right angle to ray
            // i: intersection point
            
            // Transform ray to object space
            Ray rayOS = ray.Transform(invTransform);
            Vec3 o_cVec = Vec3.Zero - rayOS.position; // Center at (0,0,0)
            float tOS = 0.0f, t = 0.0f;
            float o_cSq = o_cVec.LengthSq;
            if (o_cSq < radiusSq) { // ray starts inside sphere (exactly one intersection)
                float o_x = Vec3.Dot(o_cVec, rayOS.direction); // negative if ray points away from center
                //                       (      c_xSq        )
                float x_iSq = radiusSq - (o_cSq - (o_x * o_x));
                tOS = o_x + (float)Math.Sqrt(x_iSq);
                Vec3 iPos = rayOS.GetPoint(tOS);
                Vec3 c_iVec = iPos - Vec3.Zero; // Center at (0,0,0)
                Vec3 intersectionPoint = Vec3.TransformPosition3(iPos, transform);
                Vec3 normal = Vec3.TransformNormal3n(-c_iVec, transform);
                t = Vec3.GetLength(intersectionPoint - ray.position);
                firstIntersection = new RayIntersectionPoint(intersectionPoint, normal, t, this);
                return true;
            }
            else {
                float o_x = Vec3.Dot(o_cVec, rayOS.direction); // negative if ray points away from center
                if (o_x < 0.0f) {
                    tOS = 0.0f;
                    firstIntersection = null;
                    return false;
                }
                //                       (      c_xSq        )
                float x_iSq = radiusSq - (o_cSq - (o_x * o_x));
                if (x_iSq < 0.0f) {
                    tOS = 0.0f;
                    firstIntersection = null;
                    return false;
                }
                // Sphere is hit
                tOS = o_x - (float)Math.Sqrt(x_iSq);
                Vec3 iPos = rayOS.GetPoint(tOS);
                Vec3 c_iVec = iPos - Vec3.Zero; // Center at (0,0,0)
                Vec3 intersectionPoint = Vec3.TransformPosition3(iPos, transform);
                Vec3 normal = Vec3.TransformNormal3n(c_iVec, transform);
                t = Vec3.GetLength(intersectionPoint - ray.position);
                firstIntersection = new RayIntersectionPoint(intersectionPoint, normal, t, this);
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

            // Transform ray to object space
            Ray rayOS = ray.Transform(invTransform);
            Vec3 o_cVec = Vec3.Zero - rayOS.position; // Center at (0,0,0)
            float t1OS = 0f, t2OS = 0f, t1 = 0f, t2 = 0f;
            float o_cSq = o_cVec.LengthSq;
            if (o_cSq < radiusSq) { // ray starts inside sphere (exactly one intersection)
                float o_x = Vec3.Dot(o_cVec, rayOS.direction); // negative if ray points away from center
                //                       (      c_xSq        )
                float x_iSq = radiusSq - (o_cSq - (o_x * o_x));
                t1OS = o_x + (float)Math.Sqrt(x_iSq);
                Vec3 iPos = rayOS.GetPoint(t1OS);
                Vec3 c_iVec = iPos - Vec3.Zero; // Center at (0,0,0)
                Vec3 intersectionPoint = Vec3.TransformPosition3(iPos, transform);
                Vec3 normal = Vec3.TransformNormal3n(c_iVec, transform);
                t1 = Vec3.GetLength(intersectionPoint - ray.position);
                intersections.Add(t1OS, new RayIntersectionPoint(intersectionPoint, normal, t1, this));
                return 1;
            }
            else {
                float o_x = Vec3.Dot(o_cVec, ray.direction); // negative if ray points away from center
                if (o_x < 0.0f) {
                    t1OS = t2OS = 0.0f;
                    intersections = null;
                    return 0;
                }
                //                       (      c_xSq        )
                float x_iSq = radiusSq - (o_cSq - (o_x * o_x));
                if (x_iSq < 0.0f) {
                    t1OS = t2OS = 0.0f;
                    intersections = null;
                    return 0;
                }
                float x_i = (float)Math.Sqrt(x_iSq);
                t1OS = o_x - x_i;
                t2OS = o_x + x_i;

                Vec3 i1Pos = rayOS.GetPoint(t1OS);
                Vec3 i2Pos = rayOS.GetPoint(t2OS);
                Vec3 c_i1Vec = i1Pos - Vec3.Zero; // Center at (0,0,0)
                Vec3 c_i2Vec = i2Pos - Vec3.Zero; // Center at (0,0,0)
                Vec3 intersectionPoint1 = Vec3.TransformPosition3(i1Pos, transform);
                Vec3 intersectionPoint2 = Vec3.TransformPosition3(i2Pos, transform);
                Vec3 normal1 = Vec3.TransformNormal3n(c_i1Vec, transform);
                Vec3 normal2 = Vec3.TransformNormal3n(c_i2Vec, transform);
                t1 = Vec3.GetLength(intersectionPoint1 - ray.position);
                t2 = Vec3.GetLength(intersectionPoint2 - ray.position);
                intersections.Add(t1OS, new RayIntersectionPoint(intersectionPoint1, normal1, t1, this));
                intersections.Add(t2OS, new RayIntersectionPoint(intersectionPoint2, normal2, t2, this));
                return 2;
            }
        }

        public void Transform(Matrix transformation) {
            this.transform *= transformation;
            this.invTransform = Matrix.Invert(this.transform);
            Setup();
        }

        public void Transform(Matrix transformation, Matrix invTransformation) {
            this.transform *= transformation;
            this.invTransform = invTransform * this.invTransform;
            Setup();
        }

        protected void Setup() {
            boundingSphere.center = Vec3.TransformPosition3(Vec3.Zero, transform);
            Vec3 onSphereX = Vec3.TransformPosition3(new Vec3(radius, 0f, 0f), transform);
            Vec3 onSphereY = Vec3.TransformPosition3(new Vec3(0f, radius, 0f), transform);
            Vec3 onSphereZ = Vec3.TransformPosition3(new Vec3(0f, 0f, radius), transform);
            float transformedRadiusXSq = Vec3.GetLengthSq(onSphereX - boundingSphere.center);
            float transformedRadiusYSq = Vec3.GetLengthSq(onSphereY - boundingSphere.center);
            float transformedRadiusZSq = Vec3.GetLengthSq(onSphereZ - boundingSphere.center);
            boundingSphere.radiusSq = Math.Max(transformedRadiusZSq, Math.Max(transformedRadiusXSq, transformedRadiusYSq));
            boundingSphere.radius = (float)Math.Sqrt(boundingSphere.radiusSq);
        }

        public BSphere BSphere {
            get { return boundingSphere; }
        }

    }
}
