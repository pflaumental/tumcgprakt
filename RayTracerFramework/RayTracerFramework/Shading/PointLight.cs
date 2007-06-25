using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using RayTracerFramework.Geometry;
using RayTracerFramework.Utility;

namespace RayTracerFramework.Shading {
    class PointLight : Light {

        public Vec3 position;

        public PointLight(Vec3 position) : base(LightType.Point) {
            this.position = new Vec3(position);
           
        }
        

    }
}
