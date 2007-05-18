using System;
using System.Collections.Generic;
using System.Text;
using RayTracerFramework.Geometry;

namespace RayTracerFramework.Shading {
    interface IObject : IGeometricObject, IShadable {
        IObject Clone();
    }
}
