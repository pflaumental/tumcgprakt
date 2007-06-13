using System;
using System.Collections.Generic;
using System.Text;
using RayTracerFramework.Geometry;
using RayTracerFramework.RayTracer;

namespace RayTracerFramework.Shading {
    interface ILightingModel {

        Color calculateColor(Ray ray, RayIntersectionPoint intersection,
                             Material material, Scene scene);

    }
}
