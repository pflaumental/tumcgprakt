using System;
using System.Collections.Generic;
using System.Text;

using RayTracerFramework.RayTracer;
using RayTracerFramework.Utility;
using RayTracerFramework.Geometry;
using RayTracerFramework.Shading;


namespace RayTracerFramework.PhotonMapping {

    class PhotonTracer {        
        private Photon[] photons;
        private int desiredStoredPhotons;
        private Scene scene;
        private int recursionDepth;

        private Random rnd;


        public PhotonTracer(Scene scene, int desiredStoredPhotons, int recursionDepth) {
            this.scene = scene;
            this.desiredStoredPhotons = desiredStoredPhotons;
            this.photons = new Photon[desiredStoredPhotons];
            this.recursionDepth = recursionDepth;
            rnd = new Random(0);
        }


     
        public PhotonMap EmitPhotons() {
            if (scene.lightManager.PhotonLightsWorldSpace.Count < 1)
                throw new Exception("No photon emitting lights in the scene.");
            
            int storedPhotons = 0;

            List<PhotonMapping.Light> lights = scene.lightManager.PhotonLightsWorldSpace;
            float totalPower = scene.GetTotalPhotonLightPower();
            float[] intervals = new float[lights.Count];
            intervals[0] = lights[0].power;

            for (int i = 1; i < lights.Count; i++)
                intervals[i] = lights[i].power / totalPower + intervals[i-1];
            
            PhotonMapping.Light light;
            float random;
            Vec3 position, direction;
            Ray ray;

            // Emit photons
            do {
                random = Rnd.RandomFloat();
                light = lights[0];
                for (int i = 0; i < intervals.Length; i++) {
                    if (random <= intervals[i])
                        light = scene.lightManager.PhotonLightsWorldSpace[i];
                }

                switch (light.lightType) {
                    case LightType.Point:
                        PhotonMapping.PointLight pointLight = (PointLight)light;
                       
                        pointLight.GetRandomSample(out direction);
                        ray = new Ray(pointLight.position, direction, 1);
                        //storedPhotons += TracePhotons(ray, pointLight.diffuse);
                        break;
                    case LightType.Area:
                        PhotonMapping.AreaLight areaLight = (AreaLight)light;
                        areaLight.GetRandomSample(out position, out direction);
                        ray = new Ray(position, direction, 1);
                        //storedPhotons += TracePhotons(ray, areaLight.diffuse);
                        break;
                }
                
            } while (storedPhotons < desiredStoredPhotons);


            return null; // new PhotonMap(photons);

        }

        private int TracePhotons(Ray ray, Color power) {
            RayIntersectionPoint intersection;
            scene.Intersect(ray, out intersection);
            if (intersection == null)
                return 0;
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
            int storedPhotons = 0;
            if (rndVal <= border) {
                // calculate diffuse ray
                // calculate newPower
                // store photon
                storedPhotons = 1;
            } else if (rndVal <= (border += pGlossy)) {
                // calculate glossy ray
            } else if (rndVal <= (border += pMirror)) {
                // calculate mirror ray
            } else if (rndVal <= (border += pRefraction)) {
                // calculate refraction ray
            } else {
                // absorption
                // store photon (possibility for rndVal == 0 is low, therefore skip check for diffuse surface)
                return 1;
            }
            return storedPhotons + TracePhotons(newRay, newPower);
        }
    }
}
