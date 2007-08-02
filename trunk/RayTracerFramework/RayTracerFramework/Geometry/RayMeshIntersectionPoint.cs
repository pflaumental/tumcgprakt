using System;
using System.Collections.Generic;
using System.Text;
using RayTracerFramework.Shading;

namespace RayTracerFramework.Geometry {
    public class RayMeshIntersectionPoint : RayIntersectionPoint {
        public MeshSubset hitSubset;

        public RayMeshIntersectionPoint(
                Vec3 position, 
                Vec3 normal, 
                float t, 
                IGeometricObject hitObject,
                Vec2 textureCoordinates,
                MeshSubset hitSubset) : base(position, normal, t, hitObject, textureCoordinates) {
            this.hitSubset = hitSubset;
        }
    }
}
