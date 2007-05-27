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
            return new DMesh(transform, transformInv, materialGroups);
        }


        public void AddMaterialGroup(MaterialGroup materialGroup) {
            materialGroups.Add(materialGroup);
        }


        public Color Shade(Ray ray, RayIntersectionPoint intersection, Scene scene, float contribution) {
            DTriangle hitTriangle = (DTriangle)intersection.hitObject;
            foreach (MaterialGroup mg in materialGroups) {
                if (mg.triangles.Contains(hitTriangle)) {
                    // Currently this will never be killed since only the triangle of the mesh are shaded
                    return StdShading.RecursiveShade(ray, intersection, scene, mg.material, contribution);
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
