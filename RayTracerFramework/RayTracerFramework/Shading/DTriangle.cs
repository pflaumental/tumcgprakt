using System;
using System.Collections.Generic;
using System.Text;
using RayTracerFramework.Geometry;

namespace RayTracerFramework.Shading {
    class DTriangle : Triangle, IObject {

        public DTriangle() { }

        public DTriangle(Vec3 p1, Vec3 p2, Vec3 p3,
                         Vec3 n1, Vec3 n2, Vec3 n3,
                         Vec3 t1, Vec3 t2, Vec3 t3) : base(p1, p2, p3,
                                                           n1, n2, n3,
                                                           t1, t2, t3) { }

        public IObject Clone() {
            return new DTriangle(p1, p2, p3, n1, n2, n3, t1, t2, t3);
        }


        public Color Shade(Ray ray, RayIntersectionPoint intersection, RayTracerFramework.RayTracer.Scene scene, float contribution) {
            return Color.Blue;
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
