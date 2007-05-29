using System;
using System.Collections.Generic;
using System.Text;
using RayTracerFramework.Geometry;
using RayTracerFramework.RayTracer;

namespace RayTracerFramework.Shading {
    class DMesh : Mesh, IObject {

        public DMesh() { }

        private DMesh(Matrix transform, Matrix transformInv, 
                      List<MaterialGroup> materialGroups) : base(transform, transformInv, materialGroups) { }


        public IObject Clone() {
            return new DMesh(transform, invTransform, materialGroups);
        }    


        public Color Shade(Ray ray, RayIntersectionPoint intersection, Scene scene, float contribution) {
            // DTriangle hitTriangle = (DTriangle)intersection.hitObject;
            RayIntersectionPointTriangle intersectionTriangle = (RayIntersectionPointTriangle)intersection;
            foreach (MaterialGroup mg in materialGroups) {
                if (mg.triangles.Contains(intersectionTriangle.hitTriangle)) {
                   
                    return scene.lightingModel.calculateColor(ray, intersection, new Material(Color.White, Color.White, Color.White, Color.White, 15, 0.1f, 0f, 1.4f), scene);
                    //return StdShading.RecursiveShade(ray, intersection, scene, Material.RedMaterial, contribution);
                }
            }
            throw new Exception("The hit triangle does not belong to this mesh.");
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
