using System;
using System.Collections.Generic;
using System.Text;
using RayTracerFramework.Geometry;
using RayTracerFramework.RayTracer;
using RayTracerFramework.PhotonMapping;

using System.Xml.Serialization;

namespace RayTracerFramework.Shading {

    public class BlinnPhongLightingModel : ILightingModel {

        public Color CalculateColor(Ray ray, RayIntersectionPoint intersection,
                                             Material material, Scene scene) {
            Color result;
            List<PhotonDistanceSqPair> photons;

            // Ambient color
            if (Settings.Render.PhotonMapping.RenderSurfacePhotons) {
                // Global illumination
                Color globalIlluminationAmbientColor = new Color();
                photons = scene.photonMap.FindPhotonsInSphere(intersection.position);
                //float minDistSq = float.PositiveInfinity;
                foreach (PhotonDistanceSqPair photonDistanceSqPair in photons) {
                    //// Use this code if you want to "see" the photons
                    //if (photonDistanceSqPair.distanceSq < minDistSq) {
                    //    minDistSq = photonDistanceSqPair.distanceSq;
                    //    globalIlluminationAmbientColor = Color.Blue * (0.0006f / photonDistanceSqPair.distanceSq);
                    //}
                    float photonDistance = (float)Math.Sqrt(photonDistanceSqPair.distanceSq);
                    globalIlluminationAmbientColor = globalIlluminationAmbientColor + photonDistanceSqPair.photon.power
                             * (1f - photonDistance / (Settings.Render.PhotonMapping.ConeFilterConstantK * Settings.Render.PhotonMapping.SphereRadius));
                }
                result = globalIlluminationAmbientColor * material.GetAmbient(intersection.textureCoordinates);
            } else
                // Local model
                result = scene.surfacesAmbientLightColor * material.GetAmbient(intersection.textureCoordinates);

            // Local lighting model
            Color localContribution = new Color(); // diffuse and specular color
            foreach (Light light in scene.lightManager.BlinnLightsWorldSpace) {
                switch (light.lightType) {
                    case LightType.Point:
                        PointLight pointLight = (PointLight)light;
                        
                        Vec3 N = intersection.normal;
                        Vec3 posToLight = pointLight.position - intersection.position;
                        Vec3 L = Vec3.Normalize(posToLight);
                        // Points normal away from light?
                        float diffuse = Vec3.Dot(L, N);
                        if (diffuse < 0)
                            continue;                        
                        
                        // Is light in shadow?
                        Vec3 toLightRayPos = new Vec3(intersection.position + Settings.Render.Ray.PositionEpsilon * intersection.normal);
                        Ray toLightRay = new Ray(toLightRayPos, L, 0);
                        float distanceToLight = posToLight.Length;
                        RayIntersectionPoint firstIntersection;
                        if (scene.Intersect(toLightRay, out firstIntersection) && firstIntersection.t < distanceToLight) {                            
                            continue;
                        }
 
                        // Light is seen
                        //Vec3 V = -ray.direction;
                        Vec3 V = Vec3.Normalize(scene.cam.eyePos - intersection.position);
                        Vec3 H = Vec3.Normalize(L + V);
                       
                        float specular = (float)Math.Pow(Vec3.Dot(H, N), material.specularPower);
                        // assert if (material.diffuseTexture != null && (intersection.textureCoordinates.x < 0f || intersection.textureCoordinates.x > 1f || intersection.textureCoordinates.y < 0f || intersection.textureCoordinates.y > 1f)) throw new Exception("Texture coordinates out of bounds");
                        
                        // Diffuse color
                        localContribution = localContribution + (material.GetDiffuse(intersection.textureCoordinates) * pointLight.diffuse * diffuse);

                        // Specular color
                        localContribution = localContribution + (material.specular * pointLight.specular * specular);

                        break;
                    case LightType.Directional:
                        break;
                } // end switch                
            } // end foreach
            if (Settings.Render.PhotonMapping.RenderSurfacePhotons)
                result = result + Settings.Render.PhotonMapping.LocalScaleDown * localContribution;
            else
                result = result + localContribution;
          
            result.Saturate();
            return result;
        }
    }
}
