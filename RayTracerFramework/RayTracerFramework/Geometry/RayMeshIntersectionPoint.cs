using System;
using System.Collections.Generic;
using System.Text;
using RayTracerFramework.Shading;

namespace RayTracerFramework.Geometry {
    class RayMeshIntersectionPoint : RayIntersectionPoint {
        public MeshSubset hitSubset;
        public float u;
        public float v;

        public RayMeshIntersectionPoint(
                Vec3 position, 
                Vec3 normal, 
                float t, 
                IGeometricObject hitObject,
                MeshSubset hitSubset, 
                float u, 
                float v) : base(position, normal, t, hitObject) {
            this.hitSubset = hitSubset;
            this.u = u;
            this.v = v;
        }
    }
}
