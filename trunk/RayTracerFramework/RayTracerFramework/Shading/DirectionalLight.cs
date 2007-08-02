using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using RayTracerFramework.Geometry;

namespace RayTracerFramework.Shading {
    [XmlType("Blinn.DirectionalLight")]
    public class DirectionalLight : Light {

        [XmlElement("Direction")]
        public Vec3 direction;

        public DirectionalLight() : this(Vec3.StdYAxis) { }

        public DirectionalLight(Vec3 direction) : base(LightType.Directional) {
            this.direction = direction;
        }
    }
}
