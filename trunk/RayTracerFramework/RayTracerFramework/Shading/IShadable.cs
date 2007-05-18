using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace RayTracerFramework.Geometry {
    interface IShadable {
        Color Shade(Ray ray,
                    IntersectionPoint intersection);
    }
}