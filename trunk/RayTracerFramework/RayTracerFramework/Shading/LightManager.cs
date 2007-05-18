using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracerFramework.Shading {
    
    class LightManager {
        public List<Light> lights;

        public LightManager() {
            lights = new List<Light>();
        }

        public void AddLight(Light light) {
            lights.Add(light);
        }
    }
}
