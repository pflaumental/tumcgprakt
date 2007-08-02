using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracerFramework.Geometry {
    public class MeshSubset {
        public TriangleKDTree kdTree;
        //public List<Triangle> triangles;

        public MeshSubset() {
            //this.triangles = new List<Triangle>();
            this.kdTree = new TriangleKDTree();
        }
    }
}
