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
    public abstract class Light {
        public LightType lightType;

        public Color diffuse;
        public float power;

        public Light() { }

        public Light(LightType lightType, Color diffuse) {
            this.lightType = lightType;
            this.diffuse = diffuse;
        }

    }
}
