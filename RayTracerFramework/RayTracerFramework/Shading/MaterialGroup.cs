using System;
using System.Collections.Generic;
using System.Text;
using RayTracerFramework.Utility;
using RayTracerFramework.Geometry;

namespace RayTracerFramework.Shading {
    class MaterialGroup {
        public Material material;
        public FastBitmap colorTexture;

        public List<DTriangle> triangles;

        public MaterialGroup() {
            this.material = new Material();
            this.triangles = new List<DTriangle>();
            this.colorTexture = null;
        }
    }
}
