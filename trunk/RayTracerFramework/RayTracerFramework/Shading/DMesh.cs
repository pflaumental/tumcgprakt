using System;
using System.Collections.Generic;
using System.Text;
using RayTracerFramework.Geometry;
using RayTracerFramework.RayTracer;

namespace RayTracerFramework.Shading {
    public class DMesh : Mesh, IObject {

        public DMesh(string meshFilename) : base(meshFilename) { }

        private DMesh(string meshFilename, List<MeshSubset> subsets, BSphere boungingSphere,
                      List<Vec3> vertices, List<Vec3> normals, List<Vec3> missingNormals)
            : base(meshFilename, subsets, boungingSphere, vertices, normals, missingNormals) { }      

        public Color Shade(Ray ray, RayIntersectionPoint intersection, Scene scene, float contribution) {
            // DTriangle hitTriangle = (DTriangle)intersection.hitObject;
            RayMeshIntersectionPoint subsetIntersection = (RayMeshIntersectionPoint)intersection;
            DMeshSubset hitSubset = (DMeshSubset)subsetIntersection.hitSubset;
            return StdShading.RecursiveShade(ray, intersection, scene, hitSubset.material, contribution);
        }

        public Material Material {
            get {
                throw new Exception("The method or operation is not implemented.");
            }
            set {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        public override string ToString() {
            return "Mesh";
        }

    }
}
