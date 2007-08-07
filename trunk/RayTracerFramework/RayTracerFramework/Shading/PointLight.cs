using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using RayTracerFramework.Geometry;
using RayTracerFramework.Utility;
using System.Xml.Serialization;

namespace RayTracerFramework.Shading {

    [XmlType("Blinn.PointLight")]
    public class PointLight : Light {

        [XmlElement("Position")]
        public Vec3 position;

        public PointLight() : base(LightType.Point) {
            position = Vec3.Zero;
        }

        public PointLight(Vec3 position) : base(LightType.Point) {
            this.position = new Vec3(position);         
        }

        public override string ToString() {
            return "Point Light";
        }

    }
}
