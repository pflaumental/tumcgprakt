using System;
using System.Collections.Generic;
using System.Text;
using RayTracerFramework.Utility;

namespace RayTracerFramework.Geometry {

    class Triangle : IGeometricObject {
        public Vec3 p1, p2, p3;
        public Vec3 n1, n2, n3;
        public Vec3 t1, t2, t3;

        public Triangle() { }

        public Triangle(Vec3 p1, Vec3 p2, Vec3 p3,
                        Vec3 n1, Vec3 n2, Vec3 n3,
                        Vec3 t1, Vec3 t2, Vec3 t3) {
            this.p1 = p1;
            this.p2 = p2;
            this.p3 = p3;
            this.n1 = n1;
            this.n2 = n2;
            this.n3 = n3;
            this.t1 = t1;
            this.t2 = t2;
            this.t3 = t3;
        }

        // Implementation: the supplied transformation matrix will transform the triangle in place
        public void Transform(Matrix transformation) {
            p1 = Vec3.TransformPosition3(p1, transformation);
            p2 = Vec3.TransformPosition3(p2, transformation);
            p3 = Vec3.TransformPosition3(p3, transformation);

            // Optimization potential: check if transformation is orthogonal
            Matrix invTrans = Matrix.Transpose(Matrix.Invert(transformation));
            n1 = Vec3.TransformNormal3n(n1, invTrans);
            n2 = Vec3.TransformNormal3n(n2, invTrans);
            n3 = Vec3.TransformNormal3n(n3, invTrans);
        }

        public void Transform(Matrix transformation, Matrix invTransformation) {
            p1 = Vec3.TransformPosition3(p1, transformation);
            p2 = Vec3.TransformPosition3(p2, transformation);
            p3 = Vec3.TransformPosition3(p3, transformation);

            // Optimization potential: check if transformation is orthogonal
            Matrix invTrans = Matrix.Transpose(invTransformation);
            n1 = Vec3.TransformNormal3n(n1, invTrans);
            n2 = Vec3.TransformNormal3n(n2, invTrans);
            n3 = Vec3.TransformNormal3n(n3, invTrans);
        }

        public bool Intersect(Ray ray) {
            Vec3 edge1 = p2 - p1;
            Vec3 edge2 = p3 - p1;

            Vec3 pVec = Vec3.Cross(ray.direction, edge2);
            float det = Vec3.Dot(edge1, pVec);

            if (det > -Trigonometric.EPSILON && det < Trigonometric.EPSILON)
                return false;
            float invDet = 1.0f / det;

            Vec3 tVec = ray.position - p1;
            float u = Vec3.Dot(tVec, pVec) * invDet;
            if (u < 0.0f || u > 1.0f)
                return false;

            Vec3 qVec = Vec3.Cross(tVec, edge1);
            float v = Vec3.Dot(ray.direction, qVec) * invDet;
            if (v < 0.0f || u + v > 1.0f)
                return false;

            float t = Vec3.Dot(edge2, qVec) * invDet;
            return true;
        }


        public bool Intersect(Ray ray, out RayIntersectionPoint firstIntersection) {

            Vec3 edge1 = p2 - p1;
            Vec3 edge2 = p3 - p1;

            Vec3 pVec = Vec3.Cross(ray.direction, edge2);
            float det = Vec3.Dot(edge1, pVec);

            if (det < Trigonometric.EPSILON) {
                firstIntersection = null;
                return false;
            }            

            Vec3 tVec = ray.position - p1;
            float u = Vec3.Dot(tVec, pVec);
            if (u < 0.0f || u > det) {
                firstIntersection = null;
                return false;
            }

            Vec3 qVec = Vec3.Cross(tVec, edge1);
            float v = Vec3.Dot(ray.direction, qVec);
            if (v < 0.0f || u + v > det) {
                firstIntersection = null;
                return false;
            }

            float t = Vec3.Dot(edge2, qVec);
            float invDet = 1.0f / det;
            t *= invDet;
            if (t < 0f) {
                firstIntersection = null;
                return false;
            }
            u *= invDet;
            v *= invDet;

            Vec3 normal = (1 - u - v) * n1 + u * n2 + v * n3;

            //if (Vec3.Dot(normal, ray.direction) > 0f) {
            //    firstIntersection = null;
            //    return false;
            //}

            firstIntersection = new RayIntersectionPoint(ray.position + t * ray.direction,
                                                         normal, t, this);
            return true;


            //Vec3 edge1 = p3 - p1;
            //Vec3 edge2 = p2 - p1;
           
            //Vec3 pVec = Vec3.Cross(ray.direction, edge2);
            //float det = Vec3.Dot(edge1, pVec);

            //if (det > -Trigonometric.EPSILON && det < Trigonometric.EPSILON) {
            //    firstIntersection = null;
            //    return false;
            //}
            //float invDet = 1.0f / det;

            //Vec3 tVec = ray.position - p1;
            //float u = Vec3.Dot(tVec, pVec) * invDet;
            //if (u < 0.0f || u > 1.0f) {
            //    firstIntersection = null;
            //    return false;
            //}

            //Vec3 qVec = Vec3.Cross(edge1, tVec);
            //float v = Vec3.Dot(ray.direction, qVec) * invDet;
            //if (v < 0.0f || u + v > 1.0f) {
            //    firstIntersection = null;
            //    return false;
            //}

            //float t = Vec3.Dot(edge2, qVec) * invDet;
            //if (t < 0f) {
            //    firstIntersection = null;
            //    return false;
            //}
            //Vec3 normal = (1 - u - v) * n1 + v * n2 + u * n3;

            //if (Vec3.Dot(normal, ray.direction) > 0f) {
            //    firstIntersection = null;
            //    return false;
            //}
  
            //firstIntersection = new RayIntersectionPoint(ray.position + t * ray.direction,
            //                                             normal, t, this);
            //return true;
        
        }


        public int Intersect(Ray ray, ref SortedList<float, RayIntersectionPoint> intersections) {
            Vec3 edge1 = p2 - p1;
            Vec3 edge2 = p3 - p1;

            Vec3 pVec = Vec3.Cross(ray.direction, edge2);
            float det = Vec3.Dot(edge1, pVec);

            if (det > -Trigonometric.EPSILON && det < Trigonometric.EPSILON) {
                return 0;
            }
            float invDet = 1.0f / det;

            Vec3 tVec = ray.position - p1;
            float u = Vec3.Dot(tVec, pVec) * invDet;
            if (u < 0.0f || u > 1.0f) {
                return 0;
            }

            Vec3 qVec = Vec3.Cross(tVec, edge1);
            float v = Vec3.Dot(ray.direction, qVec) * invDet;
            if (v < 0.0f || u + v > 1.0f) {
                return 0;
            }

            float t = Vec3.Dot(edge2, qVec) * invDet;
            Vec3 normal = (1 - u - v) * n1 + u * n2 + v * n3;

            intersections.Add(t, new RayIntersectionPoint(ray.position + t * ray.direction,
                                                          normal, t, this));
            return 1;    
        }

        public BSphere BSphere {
            get { 
                Vec3 center = (1f/3f) * (p1 + p2 + p3);
                float radiusSq = Vec3.GetLengthSq(p1 - center);
                radiusSq = Math.Max(radiusSq, Vec3.GetLengthSq(p2 - center));
                radiusSq = Math.Max(radiusSq, Vec3.GetLengthSq(p3 - center));
                return new BSphere(center, (float)Math.Sqrt(radiusSq), radiusSq);
            }
        }

    }
}
