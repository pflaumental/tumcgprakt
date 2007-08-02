using System;
using System.Collections.Generic;
using System.Text;
using RayTracerFramework.Shading;
using RayTracerFramework.RayTracer;

namespace RayTracerFramework.Geometry {
    public interface IShadable {
        Color Shade(Ray ray, RayIntersectionPoint intersection, Scene scene, float contribution);
        Material Material { get; set; }
    }
}
