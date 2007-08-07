using System;
using System.Collections.Generic;
using System.Text;
using RayTracerFramework.Geometry;
using RayTracerFramework.Shading;
using RayTracerFramework.Utility;
using System.Xml.Serialization;

namespace RayTracerFramework.PhotonMapping {
    [XmlType("Photon.AreaLight")]
    public class AreaLight : Light {
        [XmlElement("TopLeftPosition")]
        public Vec3 topLeftPos;

        [XmlElement("Normal")]
        public Vec3 normal;

        [XmlElement("Tangent")]
        public Vec3 tangent;

        [XmlElement("Binormal")]
        public Vec3 binormal;

        public AreaLight() : this(Vec3.Zero, Vec3.StdYAxis, Vec3.StdXAxis, Vec3.StdZAxis) { }


        public AreaLight(Vec3 topLeftPos, Vec3 normal, Vec3 tangent, Vec3 binormal) : base(LightType.Area, Color.White) {
            this.topLeftPos = topLeftPos;
            this.normal = normal;
            this.tangent = tangent;
            this.binormal = binormal;
        }

        public AreaLight(Vec3 topLeftPos, Vec3 normal, Vec3 tangent, Vec3 binormal, Color diffuse) : base(LightType.Area, diffuse) {
            this.topLeftPos = topLeftPos;
            this.normal = normal;
            this.tangent = tangent;
            this.binormal = binormal;
            this.diffuse = diffuse;
        }


        public void GetRandomSample(out Vec3 position, out Vec3 direction) {
            direction = Rnd.RandomVec3();
            if (Vec3.Dot(direction, normal) < 0)
                direction = -direction;

            Vec3 tangentRandom = Rnd.RandomFloat() * tangent;
            Vec3 binormalRandom = Rnd.RandomFloat() * binormal;
            position = topLeftPos + tangentRandom + binormalRandom;

        }

        public override string ToString() {
            return "Area Light";
        }
    }
}
