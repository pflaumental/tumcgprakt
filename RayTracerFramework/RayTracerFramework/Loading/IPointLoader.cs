using System;
using System.Collections.Generic;
using System.Text;
using RayTracerFramework.Shading;
using RayTracerFramework.Geometry;

namespace RayTracerFramework.Loading {
    interface IPointLoader {
        List<IIntersectable> LoadFromFile(string filename);
    }
}
