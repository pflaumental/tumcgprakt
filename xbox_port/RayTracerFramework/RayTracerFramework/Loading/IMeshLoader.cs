using System;
using System.Collections.Generic;
using System.Text;
using RayTracerFramework.Shading;

namespace RayTracerFramework.Loading {
    interface IMeshLoader {
        DMesh LoadFromFile(string filename);

    }
}
