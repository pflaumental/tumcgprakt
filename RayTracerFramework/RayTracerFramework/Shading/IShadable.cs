using System;
using System.Collections.Generic;
using System.Text;
using RayTracerFramework.Shading;
using RayTracerFramework.RayTracer;

namespace RayTracerFramework.Geometry {
    interface IShadable {
        Color Shade(Ray ray, RayIntersectionPoint intersection, Scene scene, float contribution);
    }
}
