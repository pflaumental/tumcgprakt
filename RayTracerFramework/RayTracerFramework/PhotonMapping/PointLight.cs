using System;
using System.Collections.Generic;
using System.Text;
using RayTracerFramework.Utility;
using RayTracerFramework.Geometry;
using RayTracerFramework.Shading;
using System.Xml.Serialization;

namespace RayTracerFramework.PhotonMapping {

    [XmlType("Photon.PointLight")]
    public class PointLight : Light {

        public Vec3 position;

        public PointLight() : this(Vec3.Zero) { }

        public PointLight(Vec3 position) : base(LightType.Point, Color.White) {
            this.position = new Vec3(position);
        }

        public PointLight(Vec3 position, Color diffuse) : base(LightType.Point, diffuse) {
            this.position = position;
        }

        public void GetRandomSample(out Vec3 direction) {
            direction = Rnd.RandomVec3();
        }

    }
}
