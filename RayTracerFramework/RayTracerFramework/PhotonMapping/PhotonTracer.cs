using System;
using System.Collections.Generic;
using System.Text;
using RayTracerFramework.RayTracer;
using RayTracerFramework.Geometry;
using RayTracerFramework.Shading;
using RayTracerFramework.Utility;

namespace RayTracerFramework.PhotonMapping {
    class PhotonTracer {
        private Scene scene;
        private Photon[] photons;
        private Random rnd;

        public PhotonTracer(Scene scene, Photon[] photons) {
            this.scene = scene;
            this.photons = photons;
            this.rnd = new Random(0);
        }

        public PhotonMap EmitPhotons(int mapSize, int recursionDepth) {
            photons = new Photon[mapSize];



            return null;
        }

        private void TracePhotons(Ray ray, Color power) {
            RayIntersectionPoint intersection;
            scene.Intersect(ray, out intersection);
            if (intersection == null)
                return;
            IObject obj = (IObject) intersection.hitObject;
            Material mat = obj.Material;            
            Color tmp;

            float pDiffuse;
            if (mat.diffuseTexture == null)
                tmp = power * mat.diffuse;
            else
                tmp = power * mat.diffuseTexture.GetPixel(
                        intersection.textureCoordinates.x, 
                        intersection.textureCoordinates.y);
            float maxPower = Math.Max(power.RedFloat, Math.Max(power.GreenFloat, power.BlueFloat));
            pDiffuse = Math.Max(tmp.RedFloat, Math.Max(tmp.GreenFloat, tmp.BlueFloat)) 
                    / maxPower;
            pDiffuse = pDiffuse > 1f ? 1f : pDiffuse; // clamp

            tmp = power * mat.specular;
            float pGlossy = Math.Max(tmp.RedFloat, Math.Max(tmp.GreenFloat, tmp.BlueFloat)) / maxPower;
            pGlossy = pGlossy > 1f ? 1f : pGlossy; // clamp

            float pMirror = mat.reflective 
                    ? LightHelper.Fresnel(mat, ray.direction, intersection.normal) 
                    : 0f;

            float pRefraction = mat.refractive
                    ? (1f - pMirror) * mat.refractionRate
                    : 0f;
            float rndVal = (float) rnd.NextDouble();

            Ray newRay = null;
            Color newPower = power;
            float border = pDiffuse;
            if (rndVal <= border) {
                // calculate diffuse ray
                // calculate newPower
                // store photon
            } else if (rndVal <= (border += pGlossy)) {
                // calculate glossy ray
            } else if (rndVal <= (border += pMirror)) {
                // calculate mirror ray
            } else if (rndVal <= (border += pRefraction)) {
                // calculate refraction ray
            } else {
                // absorption
                // store photon (possibility for rndVal == 0 is low, therefore skip check for diffuse surface)
                return;
            }
            TracePhotons(newRay, newPower);
        }
    }
}
