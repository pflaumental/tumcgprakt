using System;
using System.Collections.Generic;
using System.Text;
using RayTracerFramework.Shading;
using RayTracerFramework.Geometry;

namespace RayTracerFramework.PhotonMapping {

    
    public class Photon {
        public Color power;
        public Vec3 position;
        public Vec3 direction;
        public byte flag; // 0=x; 1=y; 2=z;

        public Photon(Color power, Vec3 position, Vec3 direction, byte flag) {
            this.power = power;
            this.position = position;
            this.direction = direction;
            this.flag = flag;
        }
    }

    public class PhotonDistanceSqPair {
        public Photon photon;
        public float distanceSq;

        public PhotonDistanceSqPair(Photon photon, float distanceSq) {
            this.photon = photon;
            this.distanceSq = distanceSq;
        }
    }
}
