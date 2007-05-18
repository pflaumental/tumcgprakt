using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using RayTracerFramework.Geometry;

namespace RayTracerFramework.Shading {

    enum LightType { Point, Directional, Spot };

    // Abstract base class for blinn-phong lighting
    abstract class Light {
        // Common light properties which are valid for all light types
        public Color ambient;
        public Color diffuse;
        public Color specular;

        public LightType lightType;
        public bool enabled;   

        protected Light() {
            ambient = diffuse = specular = Color.White;
            lightType = LightType.Point;
            enabled = true;
        }


    }
}
