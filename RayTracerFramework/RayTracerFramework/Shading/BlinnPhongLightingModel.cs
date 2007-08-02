using System;
using System.Collections.Generic;
using System.Text;
using RayTracerFramework.Geometry;
using RayTracerFramework.RayTracer;
using RayTracerFramework.PhotonMapping;

namespace RayTracerFramework.Shading {

    class BlinnPhongLightingModel : ILightingModel {
        public static readonly float coneFilterConstantK = 1.5f;//1.6

        public Color calculateColor(Ray ray, RayIntersectionPoint intersection,
                                             Material material, Scene scene) {
            Color iTotal = new Color();

            if (scene.usePhotonMapping) {
                List<PhotonDistanceSqPair> photons = scene.photonMap.FindPhotonsInSphere(intersection.position);
                Color photonDiffuseColor = new Color();
                //float minDistSq = float.PositiveInfinity;
                foreach (PhotonDistanceSqPair photonDistanceSqPair in photons) {
                    //// Use this code if you want to "see" the photons
                    //if (photonDistanceSqPair.distanceSq < minDistSq) {
                    //    minDistSq = photonDistanceSqPair.distanceSq;
                    //    photonDiffuseColor = Color.Blue * (0.0006f / photonDistanceSqPair.distanceSq);
                    //}
                    photonDiffuseColor = photonDiffuseColor + photonDistanceSqPair.photon.power
                             * (1f - ((float)Math.Sqrt(photonDistanceSqPair.distanceSq)) / (coneFilterConstantK * PhotonMap.sphereRadius));                    
                }
                iTotal = iTotal + photonDiffuseColor * material.GetDiffuse(intersection.textureCoordinates);
            }

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
                        float distanceToLight = posToLight.Length;

                        if(!scene.usePhotonMapping)
                            iTotal = iTotal + material.ambient * pointLight.ambient;
                        
                        // Is light in shadow?
                        Vec3 toLightRayPos = new Vec3(intersection.position + Ray.positionEpsilon * intersection.normal);
                        Ray toLightRay = new Ray(toLightRayPos, L, 0);
                        RayIntersectionPoint firstIntersection;
                        if (scene.Intersect(toLightRay, out firstIntersection) && firstIntersection.t < distanceToLight) {
                            iTotal = iTotal + material.ambient * pointLight.ambient;
                            continue;
                        }
 
                        // Light is seen
                        //Vec3 V = -ray.direction;
                        Vec3 V = Vec3.Normalize(scene.cam.eyePos - intersection.position);
                        Vec3 H = Vec3.Normalize(L + V);
                       
                        float specular = (float)Math.Pow(Vec3.Dot(H, N), material.specularPower);
                        // assert if (material.diffuseTexture != null && (intersection.textureCoordinates.x < 0f || intersection.textureCoordinates.x > 1f || intersection.textureCoordinates.y < 0f || intersection.textureCoordinates.y > 1f)) throw new Exception("Texture coordinates out of bounds");
                        Color diffuseContribution = (material.GetDiffuse(intersection.textureCoordinates) * pointLight.diffuse * diffuse);
                        if (scene.usePhotonMapping)
                            diffuseContribution = diffuseContribution * PhotonMap.diffuseScaleDown;
                        iTotal = iTotal + diffuseContribution;
                        iTotal = iTotal + (material.specular * pointLight.specular * specular);
                          
                        break;
                    case LightType.Directional:

                        break;
                } // end switch
                
            }

            iTotal.Saturate();
            return iTotal;            
        }
    }
}
