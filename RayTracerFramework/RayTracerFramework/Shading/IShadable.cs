using System;
using System.Collections.Generic;
using System.Text;
using RayTracerFramework.Shading;

namespace RayTracerFramework.Geometry {
    interface IShadable {
        Color Shade(Ray ray, IntersectionPoint intersection, ILightingModel lightingModel,
                    LightManager lightManager, GeometryManager geoMng);
    }
}