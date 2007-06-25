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
                        storedPhotons += TracePhotons(ray, pointLight.diffuse, storedPhotons);
                        break;
                    case LightType.Area:
                        PhotonMapping.AreaLight areaLight = (AreaLight)light;
                        areaLight.GetRandomSample(out position, out direction);
                        ray = new Ray(position, direction, 1);
                        storedPhotons += TracePhotons(ray, areaLight.diffuse, storedPhotons);
                        break;
                }
                
            } while (storedPhotons < desiredStoredPhotons);

            return new PhotonMap(photons);

        }

        private int TracePhotons(Ray ray, Color power, int arrayIndex) {
            if (arrayIndex >= photons.Length || ray.recursionDepth >= recursionDepth)
                return 0;
            RayIntersectionPoint intersection;
            scene.Intersect(ray, out intersection);
            if (intersection == null)
                return 0;
            IObject obj = (IObject) intersection.hitObject;
            Material mat;
            if (obj is DMesh) {
                RayMeshIntersectionPoint meshIntersection = (RayMeshIntersectionPoint)intersection;
                mat = ((DMeshSubset)meshIntersection.hitSubset).material;
            } else 
                mat = obj.Material;

            float pDiffuse;
            Color matDiffuse = mat.diffuseTexture == null
                    ? mat.diffuse
                    : mat.diffuseTexture.GetPixel(
                            intersection.textureCoordinates.x, 
                            intersection.textureCoordinates.y);
            Color powerXdiffuse = power * matDiffuse;
            float maxPower = Math.Max(power.RedFloat, Math.Max(power.GreenFloat, power.BlueFloat));
            pDiffuse = Math.Max(powerXdiffuse.RedFloat, Math.Max(powerXdiffuse.GreenFloat, powerXdiffuse.BlueFloat)) 
                    / maxPower;
            pDiffuse = pDiffuse > 1f ? 1f : pDiffuse; // clamp

            Color powerXglossy = power * mat.specular;
            float pGlossy = Math.Max(powerXglossy.RedFloat, Math.Max(powerXglossy.GreenFloat, powerXglossy.BlueFloat)) / maxPower;
            pGlossy = pGlossy > 1f ? 1f : pGlossy; // clamp

            float pMirror = mat.reflective 
                    ? LightHelper.Fresnel(mat, ray.direction, intersection.normal) 
                    : 0f;

            float pRefraction = mat.refractive
                    ? (1f - pMirror) * mat.refractionRate
                    : 0f;
            float rndVal = (float) rnd.NextDouble();

            Vec3 newRayDirection;
            Color newPower = power;
            float border = pDiffuse;
            int storedPhotonCnt = 0;
            if (rndVal <= border) {
                newRayDirection = LightHelper.GetUniformRndDirection(intersection.normal);
                newPower = powerXdiffuse * (1f / pDiffuse);
                photons[arrayIndex] = new Photon(power - newPower, intersection.position, ray.direction, 0);
                storedPhotonCnt = 1;
            } else if (rndVal <= (border += pGlossy)) {
                // TODO: calculate GLOSSY direction instead
                newRayDirection = LightHelper.GetUniformRndDirection(intersection.normal);
                newPower = powerXglossy * (1f / pGlossy);
            } else if (rndVal <= (border += pMirror)) {
                float NV = Vec3.Dot(intersection.normal, -ray.direction);
                newRayDirection = Vec3.Normalize(2.0f * NV * intersection.normal + ray.direction);
            } else if (rndVal <= (border += pRefraction)) {
                float NV = Vec3.Dot(intersection.normal, -ray.direction);
                float cosThetaR = (float)Math.Sqrt(1f - mat.refractionRatio * mat.refractionRatio * (1f - NV * NV));
                // Do nothing if formula can not be hold
                if (float.IsNaN(cosThetaR))
                    return 0;
                float beforeNTerm = (float)(mat.refractionRatio * NV - cosThetaR);
                newRayDirection = beforeNTerm * intersection.normal + mat.refractionRatio * ray.direction;
            } else {
                // absorption
                photons[arrayIndex] = new Photon(power, intersection.position, ray.direction, 0);
                return 1;
            }
            Ray newRay = new Ray(intersection.position, newRayDirection, ray.recursionDepth + 1);
            return storedPhotonCnt + TracePhotons(newRay, newPower, arrayIndex + storedPhotonCnt);
        }
    }
}