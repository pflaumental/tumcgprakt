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

            //return scene.lightingModel.CalculateColor(ray, intersection, /*hitSubset.material*/Material.WhiteMaterial, scene);
            //foreach (DMeshSubset subset in subsets) {
            //    if (subset.triangles.Contains(subsetIntersection.hitSubset)) {
                   
            //        return scene.lightingModel.CalculateColor(ray, intersection, Material.WhiteMaterial, scene);
            //        //return StdShading.RecursiveShade(ray, intersection, scene, Material.RedMaterial, contribution);
            //    }
            //}
            //throw new Exception("The hit triangle does not belong to this mesh.");
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
