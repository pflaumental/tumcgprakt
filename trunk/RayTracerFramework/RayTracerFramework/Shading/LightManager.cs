using System;
using System.Collections.Generic;
using System.Text;
using RayTracerFramework.Geometry;

namespace RayTracerFramework.Shading {
    
    class LightManager {
        private List<Light> lightsWorldSpace;

        public LightManager() {
            this.lightsWorldSpace = new List<Light>();
        }

        public void AddWorldSpaceLight(Light lightWorldSpace) {
            this.lightsWorldSpace.Add(lightWorldSpace);
        }

        public List<Light> LightsWorldSpace {
            get { return lightsWorldSpace; }
        }
    }
}
