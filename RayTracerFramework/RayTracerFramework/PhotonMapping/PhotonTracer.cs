using System;
using System.Collections.Generic;
using System.Text;

using RayTracerFramework.RayTracer;
using RayTracerFramework.Utility;
using RayTracerFramework.Geometry;
using RayTracerFramework.Shading;


namespace RayTracerFramework.PhotonMapping {

    public class PhotonTracer {

        // Related assignement: 8.1.a

        private Photon[] photons;
        private int maxStoredPhotons;

        private Scene scene;
        private int recursionDepth;

        private Random rnd;

        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.StatusStrip statusBar;

        public PhotonTracer(
                Scene scene, 
                int maxStoredPhotons,
                int recursionDepth,
                System.Windows.Forms.ProgressBar progressBar,
                System.Windows.Forms.StatusStrip statusBar) {
            this.scene = scene;
            this.maxStoredPhotons = maxStoredPhotons;
            this.photons = new Photon[maxStoredPhotons];
            this.recursionDepth = recursionDepth;
            this.progressBar = progressBar;
            this.statusBar = statusBar;
            rnd = new Random(0);
        }
     
        public PhotonMap EmitPhotons() {

            // Related assignement: 8.1.d

            if (scene.lightManager.PhotonLightsWorldSpace.Count < 1)
                throw new Exception("No photon emitting lights in the scene.");
            
            int storedPhotons = 0;
            int emittedPhotons = 0;
            int lastStoredPhotons = 0;

            

            List<PhotonMapping.Light> lights = scene.lightManager.PhotonLightsWorldSpace;
            float totalPower = scene.GetTotalPhotonLightPower();
            float[] intervals = new float[lights.Count];
            intervals[0] = lights[0].power / totalPower;

            for (int i = 1; i < lights.Count; i++)
                intervals[i] = lights[i].power / totalPower + intervals[i-1];
            
            PhotonMapping.Light light;
            float random;
            Vec3 position, direction;
            Ray ray;

            progressBar.Minimum = 0;
            progressBar.Maximum = maxStoredPhotons;

            // Emit photons
            do {
                emittedPhotons++;
                if((storedPhotons - lastStoredPhotons) > 1000) {
                    lastStoredPhotons = storedPhotons;
                    statusBar.Items.Clear();
                    statusBar.Items.Add("Emitting photons... Photons stored: " + storedPhotons + " / " + maxStoredPhotons);
                    statusBar.Refresh();
                    progressBar.Value = storedPhotons;
                    progressBar.Parent.Update();
                }
                
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
                
            } while (storedPhotons < maxStoredPhotons);

            float powerScale = (Settings.Setup.PhotonMapping.PowerLevel * totalPower) / emittedPhotons;

            foreach (Photon photon in photons) {
                photon.power *= powerScale;
            }
            statusBar.Items.Clear();
            statusBar.Items.Add("Building photon kd-tree. This may take some time...");
            statusBar.Refresh();
            return new PhotonMap(photons);

        }

        private int TracePhotons(Ray ray, Color power, int arrayIndex) {
            if (arrayIndex >= photons.Length || ray.recursionDepth >= recursionDepth)
                return 0;
            RayIntersectionPoint intersection;
            scene.Intersect(ray, out intersection);
            if (intersection == null)
                return 0;

            if (Settings.Setup.PhotonMapping.MediumIsParticipating) {

                // Related assignement: 10

                int inMediumStoredPhotons = StorePhotonsInMedium(ray, power, intersection, arrayIndex);
                if ((arrayIndex += inMediumStoredPhotons) >= photons.Length)
                    return inMediumStoredPhotons;
            }

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

            float pNotMirrorOrRefraction = (1f - pMirror - pRefraction);
            pDiffuse *= pNotMirrorOrRefraction;
            pGlossy *= pNotMirrorOrRefraction;

            Vec3 newRayPosition = intersection.position;
            Vec3 newRayDirection;
            Color newPower = power;
            float border = pDiffuse * (1f - pMirror - pRefraction);
            int storedPhotonCnt = 0;

            // Related assignement: 8.3.b

            if (rndVal <= border) {
                newRayDirection = LightHelper.GetUniformRndDirection(intersection.normal);
                newPower = powerXdiffuse * (1f / pDiffuse);
                if (ray.recursionDepth > 1) {

                    // Related assignement: 8.3.c

                    photons[arrayIndex] = new Photon(power, intersection.position, ray.direction, 0);
                    storedPhotonCnt = 1;
                }
                newRayPosition = intersection.position + intersection.normal * Settings.Render.Ray.PositionEpsilon;
            } else if (rndVal <= (border += pGlossy)) {
                float NV = Vec3.Dot(intersection.normal, -ray.direction);
                Vec3 mirrorDirection = Vec3.Normalize(2.0f * NV * intersection.normal + ray.direction);
                newRayDirection = LightHelper.GetRndSpecularPhongPDF(mirrorDirection, mat.specularPower);
                newPower = powerXglossy * (1f / pGlossy);
                newRayPosition = intersection.position + intersection.normal * Settings.Render.Ray.PositionEpsilon;
            } else if (rndVal <= (border += pMirror)) {

                // Related assignement: 10

                float NV = Vec3.Dot(intersection.normal, -ray.direction);
                newRayDirection = Vec3.Normalize(2.0f * NV * intersection.normal + ray.direction);
                newRayPosition = intersection.position + intersection.normal * Settings.Render.Ray.PositionEpsilon;
            } else if (rndVal <= (border += pRefraction)) {

                // Related assignement: 10

                float NV = Vec3.Dot(intersection.normal, -ray.direction);
                float cosThetaR = (float)Math.Sqrt(1f - mat.refractionRatio * mat.refractionRatio * (1f - NV * NV));
                // Do nothing if formula can not be hold
                if (float.IsNaN(cosThetaR))
                    return 0;
                float beforeNTerm = (float)(mat.refractionRatio * NV - cosThetaR);
                newRayDirection = beforeNTerm * intersection.normal + mat.refractionRatio * ray.direction;
                newRayPosition = intersection.position - intersection.normal * Settings.Render.Ray.PositionEpsilon;
                Ray innerRay = new Ray(intersection.position, newRayDirection, ray.recursionDepth + 1);
                RayIntersectionPoint outgoingIntersection;
                scene.Intersect(ray, out outgoingIntersection);

                if (outgoingIntersection == null)
                    return 0;

                newRayPosition = outgoingIntersection.position - outgoingIntersection.normal * Settings.Render.Ray.PositionEpsilon;
                NV = Vec3.Dot(outgoingIntersection.normal, -innerRay.direction);
                float invRefractionRatio = 1f - mat.refractionRatio;
                float invRefractionRatioSq = invRefractionRatio * invRefractionRatio;
                cosThetaR = (float)Math.Sqrt(1f - invRefractionRatioSq * (1f - NV * NV));
                // Do nothing if formula can not be hold
                if (float.IsNaN(cosThetaR))
                    return 0;
                beforeNTerm = (float)(invRefractionRatio * NV - cosThetaR);
                newRayDirection = beforeNTerm * outgoingIntersection.normal + invRefractionRatio * innerRay.direction;                
            } else {
                // absorption
                if (ray.recursionDepth > 1) {
                    photons[arrayIndex] = new Photon(power, intersection.position, ray.direction, 0);
                    return 1;
                } else
                    return 0;
            }
            Ray newRay = new Ray(newRayPosition, newRayDirection, ray.recursionDepth + 1);
            return storedPhotonCnt + TracePhotons(newRay, newPower, arrayIndex + storedPhotonCnt);
        }

        private int StorePhotonsInMedium(Ray ray, Color power, RayIntersectionPoint intersection, int arrayIndex) {

            // Related assignement: 10

            float distFromOrigin = 0f;
            int result = 0;
            while (true){
                float rndVal = 0.5f + (float)rnd.NextDouble();
                distFromOrigin += rndVal / Settings.Setup.PhotonMapping.MediumLightingDensity;
                if (distFromOrigin > intersection.t || arrayIndex >= photons.Length )
                    break;
                Vec3 photonPos = intersection.position + intersection.normal * distFromOrigin;
                photons[arrayIndex++] = new Photon(power, photonPos, intersection.normal, 0);
                result++;
            }
            return result;
        }
    }
}
