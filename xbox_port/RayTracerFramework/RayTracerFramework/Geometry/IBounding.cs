using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracerFramework.Geometry {
    interface IBounding {
        BSphere BSphere { get; }
    }
}
