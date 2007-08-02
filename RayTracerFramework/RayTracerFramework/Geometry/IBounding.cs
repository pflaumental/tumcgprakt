using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracerFramework.Geometry {
    public interface IBounding {
        BSphere BSphere { get; }
    }
}
