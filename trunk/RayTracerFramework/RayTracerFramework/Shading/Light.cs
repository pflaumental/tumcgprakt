using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using RayTracerFramework.Geometry;
using System.Xml.Serialization;

namespace RayTracerFramework.Shading {

    public enum LightType { Point, Directional, Spot };

    // Abstract base class for blinn-phong lighting
    [XmlType("Blinn.Light")]
    [XmlInclude(typeof(PointLight))]
    [XmlInclude(typeof(DirectionalLight))]
    public abstract class Light {
        // Common light properties which are valid for all light types
        [XmlElement("AmbientColor")]
        public Color ambient;
        
        [XmlElement("DiffuseColor")]
        public Color diffuse;

        [XmlElement("SpecularColor")]
        public Color specular;

        [XmlIgnore()]
        public LightType lightType;

        public Light() {
            ambient = diffuse = specular = Color.White;
        }

        public Light(LightType lightType) {
            ambient = diffuse = specular = Color.White;
            this.lightType = lightType;
        }
    }


}
