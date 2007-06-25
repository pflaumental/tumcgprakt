using System;
using System.Collections.Generic;
using System.Text;
using RayTracerFramework.Shading;
using RayTracerFramework.Geometry;

namespace RayTracerFramework.PhotonMapping {
    class Photon {
        public Color power;
        public Vec3 position;
        public Vec3 direction;
        public byte flag;

        public Photon(Color power, Vec3 position, Vec3 direction, byte flag) {
            this.power = power;
            this.position = position;
            this.direction = direction;
            this.flag = flag;
        }
    }
}
