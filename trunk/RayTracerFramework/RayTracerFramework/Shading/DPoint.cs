using System;
using System.Collections.Generic;
using System.Text;
using RayTracerFramework.Geometry;

namespace RayTracerFramework.Shading {
    class DPoint : IObject {
        public Vec3 position;

        public static float EPSILON = 0.1f;
        public static float EPSILON_SQ = EPSILON * EPSILON;

        public DPoint(Vec3 position) {
            this.position = position;
        }

        public void Transform(Matrix transformation) {
            position = Vec3.TransformPosition3(position, transformation);
        }

        public void Transform(Matrix transformation, Matrix invTransformation) {
            position = Vec3.TransformPosition3(position, transformation);
        }

        public bool Intersect(Ray ray) {
            Vec3 o_pVec = position - ray.position;
            float o_x = Vec3.Dot(o_pVec, ray.direction);
            if (o_x < 0f)
                return false;
            //   |                p_xSq                |
            //   |       o_pSq          |
            if ((Vec3.GetLengthSq(o_pVec) - (o_x * o_x)) < EPSILON_SQ)
                return true;
            else
                return false;
        }

        public bool Intersect(Ray ray, out RayIntersectionPoint firstIntersection) {
            Vec3 o_pVec = position - ray.position;
            float o_x = Vec3.Dot(o_pVec, ray.direction);
            if (o_x < 0f) {
                firstIntersection = null;
                return false;
            }
            float o_pSq = Vec3.GetLengthSq(o_pVec);
            //   |       p_xSq      |            
            if ((o_pSq - (o_x * o_x)) < EPSILON_SQ) {
                firstIntersection = new RayIntersectionPoint(position, -ray.direction, (float)Math.Sqrt(o_pSq), this);
                return true;
            } else {
                firstIntersection = null;
                return false;
            }
        }

        public int Intersect(Ray ray, ref SortedList<float, RayIntersectionPoint> intersections) {
            Vec3 o_pVec = position - ray.position;
            float o_x = Vec3.Dot(o_pVec, ray.direction);
            if (o_x < 0f)
                return 0;
            float o_pSq = Vec3.GetLengthSq(o_pVec);
            //   |       p_xSq      |            
            if ((o_pSq - (o_x * o_x)) < EPSILON_SQ) {
                float t = (float)Math.Sqrt(o_pSq);
                intersections.Add(t, new RayIntersectionPoint(position, -ray.direction, t, this));
                return 1;
            } else
                return 0;
        }

        public BSphere BSphere {
            get { return new BSphere(position, EPSILON); }
        }

        public Color Shade(Ray ray, RayIntersectionPoint intersection, RayTracerFramework.RayTracer.Scene scene, float contribution) {
            float factor = (25f - intersection.t) / 25f;
            if (factor < 0f)
                factor = 0f;
            return Color.White * factor;
        }

        public Material Material {
            get {
                throw new Exception("The method or operation is not implemented.");
            }
            set {
                throw new Exception("The method or operation is not implemented.");
            }
        }
    }
}
