using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using RayTracerFramework.Geometry;

namespace RayTracerFramework.Shading {
    class PointLight : Light {

        public Vec3 position;

        public PointLight(Vec3 position) {
            this.position = position;
        }

        

    }
}
