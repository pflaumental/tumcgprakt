using System;
using System.Collections.Generic;
using System.Text;
using RayTracerFramework.Geometry;
using RayTracerFramework.RayTracer;

namespace RayTracerFramework.Shading {
    /*
    class DTriangle : Triangle, IObject {
        public MaterialGroup materialGroup;

        public DTriangle(MaterialGroup materialGroup) { }

        public DTriangle(Vec3 p1, Vec3 p2, Vec3 p3,
                         Vec3 n1, Vec3 n2, Vec3 n3,
                         Vec3 t1, Vec3 t2, Vec3 t3, 
                         MaterialGroup materialGroup) : 
                         base(p1, p2, p3,
                              n1, n2, n3,
                              t1, t2, t3) {
            this.materialGroup = materialGroup;
        }

        public IObject Clone() {
            return new DTriangle(p1, p2, p3, n1, n2, n3, t1, t2, t3, materialGroup);
        }


        public Color Shade(Ray ray, RayIntersectionPoint intersection, Scene scene, float contribution) {
            return Color.Blue;
            // return StdShading.RecursiveShade(ray, intersection, scene, materialGroup.material, contribution);
            
        }

        public Material Material {
            get {
                return materialGroup.material;
            }
            set {
                materialGroup.material = value;
            }
        }

    }*/
}
