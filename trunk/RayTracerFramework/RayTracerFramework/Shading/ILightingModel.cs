using System;
using System.Collections.Generic;
using System.Text;
using RayTracerFramework.Geometry;
using System.Drawing;
using RayTracerFramework.RayTracer;

namespace RayTracerFramework.Shading {
    public interface ILightingModel {

        Color calculateColor(Ray ray, RayIntersectionPoint intersection,
                             Material material, Scene scene);

    }
}
