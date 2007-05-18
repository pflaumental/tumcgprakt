using System;
using System.Collections.Generic;
using System.Text;
using RayTracerFramework.Geometry;
using System.Drawing;

namespace RayTracerFramework.Shading {
    interface ILightingModel {

        Color calculateColor(Ray ray, IntersectionPoint intersection,
                             Material material, LightManager lightManager);

    }
}
