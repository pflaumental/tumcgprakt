using System;
using System.Collections.Generic;
using System.Text;
using RayTracerFramework.Shading;
using System.Xml.Serialization;

namespace RayTracerFramework.PhotonMapping {
    public enum LightType { Point, Area };

    // Abstract base class for photon mapping
    [XmlType("Photon.Light")]
    [XmlInclude(typeof(RayTracerFramework.PhotonMapping.PointLight))]
    [XmlInclude(typeof(RayTracerFramework.PhotonMapping.AreaLight))]
    public abstract class Light {
        [XmlIgnore()]
        public LightType lightType;

        [XmlElement("DiffuseColor")]
        public Color diffuse;

        [XmlElement("Power")]
        public float power;

        public Light() { }

        public Light(LightType lightType, Color diffuse) {
            this.lightType = lightType;
            this.diffuse = diffuse;
        }

    }
}
