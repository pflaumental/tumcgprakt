using System;
using System.Collections.Generic;
using System.Text;
using RayTracerFramework.Utility;
using RayTracerFramework.Geometry;

namespace RayTracerFramework.Shading {
    class DMeshSubset : MeshSubset {
        public Material material;
        public FastBitmap colorTexture;

        public DMeshSubset() : base() {
            this.material = new Material();            
            this.colorTexture = null;
        }
    }
}
