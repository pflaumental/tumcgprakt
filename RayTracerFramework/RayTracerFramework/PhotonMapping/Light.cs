using System;
using System.Collections.Generic;
using System.Text;
using RayTracerFramework.Shading;

namespace RayTracerFramework.PhotonMapping {
    enum LightType { Point, Area };

    // Abstract base class for photon mapping
    abstract class Light {
        public LightType lightType;

        public Color diffuse;
        public float power;

        protected Light(LightType lightType, Color diffuse) {
            this.lightType = lightType;
            this.diffuse = diffuse;
        }

    }
}
