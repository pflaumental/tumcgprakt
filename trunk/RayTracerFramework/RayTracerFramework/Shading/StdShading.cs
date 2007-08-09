using System;
using System.Collections.Generic;
using System.Text;
using RayTracerFramework.Geometry;
using RayTracerFramework.RayTracer;
using RayTracerFramework.Utility;
using RayTracerFramework.PhotonMapping;

namespace RayTracerFramework.Shading {
    // Standard recursive raytracing
    public class StdShading {

        // Refraction does not support intersecting objects and objects
        // that do not support rays starting inside of them
        public static Color RecursiveShade(
                Ray ray, 
                RayIntersectionPoint intersection, 
                Scene scene,
                Material material, 
                float contribution) {

            // Related assignement: 4.2.a

            Color resultColor = new Color();

            // Related assignement: 6.2.a

            float fresnelReflectionPart, refractionPart, localPart;
            if (material.reflective) {
                // Approximation from nvidia paper for fresnel term:
                // R(theta) ~~ R_a(theta) = R(0) + (1 - R(0)) * (1 - cos(theta))^5
                // with R(0) = (1 - refractionRatio)^2 / (1 + refreactionRatio)^2
                fresnelReflectionPart = LightHelper.Fresnel(material, ray.direction, intersection.normal);
                if (material.refractive) {
                    refractionPart = (1f - fresnelReflectionPart) * material.refractionRate;
                    localPart = (1f - fresnelReflectionPart) * (1f - material.refractionRate);
                } else {
                    refractionPart = 0f;
                    localPart = 1f - fresnelReflectionPart;
                }
            } else {
                fresnelReflectionPart = 0f;
                if (material.refractive) {
                    refractionPart = material.refractionRate;
                    localPart = 1f - refractionPart;
                } else {
                    refractionPart = 0f;
                    localPart = 1f;
                }
            }

            // "Local" color;
            if (localPart > Settings.Render.StdShading.LocalThreshold) {
                Color localColor = scene.lightingModel.CalculateColor(
                        ray,
                        intersection,
                        material,
                        scene);
                resultColor += (localColor * localPart);
            }

            bool continueRecursion = ray.recursionDepth + 1 <= Settings.Render.Renderer.MaxRecursionDepth;
            float reflectionPartSum = fresnelReflectionPart;            

            // Refraction color
            // Refraction does not support intersecting objects and objects that do not
            // support rays that start inside of them
            float refractionContribution = refractionPart * contribution;
            if (material.refractive
                        && refractionContribution > Settings.Render.StdShading.ContributionThreshold
                        && continueRecursion) {
                // Calculate refraction ray
                // sinThetaR = (ni/nr) * sinThetaI
                // => R = ((ni/nr)*(N*V) - sqrt(1 - (ni/nr)^2*(1.0f-(N*V)^2))) * N - (ni/nr) * V
                float NV = Vec3.Dot(intersection.normal, -ray.direction);
                // assert if(NV < 0) throw new Exception("NV < 0");

                float cosThetaR = (float)Math.Sqrt(1f - material.refractionRatio * material.refractionRatio * (1f - NV * NV));
                // Reflect if formula can not be hold -> use reflection instead of refraction
                if (float.IsNaN(cosThetaR)) {
                    reflectionPartSum += refractionPart;
                    goto endRefraction;
                }
                float beforeNTerm = (float)(material.refractionRatio * NV - cosThetaR);
                Vec3 refractionDir = beforeNTerm * intersection.normal + material.refractionRatio * ray.direction;

                // assert if(Vec3.Dot(refractionDir, intersection.normal) > 0) throw new Exception("Vec3.Dot(refractionDir, intersection.normal) > 0");
                // assert if (Vec3.Dot(refractionDir, ray.direction) < 0) throw new Exception("Vec3.Dot(refractionDir, ray.direction) < 0");

                Vec3 refractionPos = intersection.position - Settings.Render.Ray.PositionEpsilon * intersection.normal;//refractionDir;
                // assert if (Vec3.Dot(refractionPos - intersection.position, intersection.normal) > 0) throw new Exception("Vec3.Dot(refractionPos - intersection.position, intersection.normal) < 0");                    
                Ray refractionRay = new Ray(refractionPos, refractionDir, ray.recursionDepth + 1);

                // Get refraction-ray intersection with the object
                RayIntersectionPoint refractionIntersection;
                // Optical assertion: ^^
                if (!intersection.hitObject.Intersect(refractionRay, out refractionIntersection))
                    return Color.Red;//material.refractionPart * scene.GetBackgroundColor(refractionRay);                

                // Calculate second (outside) refraction ray                    
                NV = Vec3.Dot(refractionIntersection.normal, -refractionDir);
                // assert if (NV < 0) throw new Exception("NV < 0");
                float refractionRatio = 1f / material.refractionRatio;

                cosThetaR = (float)Math.Sqrt(1f - refractionRatio * refractionRatio * (1f - NV * NV));
                // Clamp cosine (not sure what else we could do in this case)
                if (float.IsNaN(cosThetaR)) {
                    // assertreturn Color.Red;
                    cosThetaR = 1f;
                }
                beforeNTerm = (float)(refractionRatio * NV - cosThetaR);
                refractionDir = beforeNTerm * refractionIntersection.normal + refractionRatio * refractionDir;
                // assert if (float.IsNaN(refractionDir.x)) throw new Exception("refraction dir is NaN");
                refractionPos = refractionIntersection.position - Settings.Render.Ray.PositionEpsilon * refractionIntersection.normal;
                refractionRay = new Ray(refractionPos, refractionDir, ray.recursionDepth + 2);

                // Test refracted ray against scene and calculate color
                RayIntersectionPoint firstIntersection;
                Color refractionColor;
                if (scene.Intersect(refractionRay, out firstIntersection)) {
                    // assert if (firstIntersection.hitObject == intersection.hitObject) throw new Exception("shit");

                    IObject firstHitObject = (IObject)firstIntersection.hitObject;
                    refractionColor = firstHitObject.Shade(refractionRay, firstIntersection, scene, refractionContribution);
                } else
                    refractionColor = scene.GetBackgroundColor(refractionRay);
                // Add refraction color to resultColor
                resultColor += (refractionColor * refractionPart);
            } endRefraction:

            // Reflection color
            float reflectionContribution = reflectionPartSum * contribution;
            if (material.reflective
                    && reflectionContribution > Settings.Render.StdShading.ContributionThreshold
                    && continueRecursion) { 
                // Calculate reflection ray
                // R = 2N(N*V)-V   (V = -ray.direction)
                float NV = Vec3.Dot(intersection.normal, -ray.direction);
                // assert if(NV < 0) throw new Exception("NV < 0");

                Vec3 reflectionDir = Vec3.Normalize(2.0f * NV * intersection.normal + ray.direction);
                Vec3 reflectionPos = intersection.position + Settings.Render.Ray.PositionEpsilon * reflectionDir;
                Ray reflectionRay = new Ray(reflectionPos, reflectionDir, ray.recursionDepth + 1);

                // Find nearest object intersection and shade reflection  
                RayIntersectionPoint firstIntersection;
                Color reflectionColor;
                if (scene.Intersect(reflectionRay, out firstIntersection)) {
                    IObject firstHitObject = (IObject)firstIntersection.hitObject;
                    reflectionColor = firstHitObject.Shade(reflectionRay, firstIntersection, scene, reflectionContribution);
                }
                else {
                    reflectionColor = scene.GetBackgroundColor(reflectionRay);
                }
                // Add reflection color to resultColor
                resultColor += (reflectionColor * reflectionPartSum);
            }

            // Fog
            if (Settings.Render.StdShading.enableFog) {
                // Medium illumination
                float surfaceCoefficient = 1f / (1f + intersection.t * Settings.Render.StdShading.FogLevel);
                float mediumCoefficient = 1f - surfaceCoefficient;
                if (Settings.Render.PhotonMapping.RenderMediumPhotons) {
                    Color mediumLightColor = new Color();
                    List<PhotonDistanceSqPair> photons = scene.photonMap.FindPhotonsAlongRay(ray, intersection.t);
                    foreach (PhotonDistanceSqPair photonDistanceSqPair in photons) {
                        float photonDistance = (float)Math.Sqrt(photonDistanceSqPair.distanceSq);
                        mediumLightColor = mediumLightColor + photonDistanceSqPair.photon.power
                                * Settings.Render.PhotonMapping.MediumEnlightmentAmplifier *
                                (1f - photonDistance / (Settings.Render.PhotonMapping.MediumConeFilterConstantK
                                * Settings.Render.PhotonMapping.CapsuleRadius));
                    }
                    resultColor = resultColor * surfaceCoefficient
                            + scene.mediumColor * mediumCoefficient * mediumLightColor;
                } else
                    resultColor = resultColor * surfaceCoefficient
                            + scene.mediumColor * mediumCoefficient * scene.fogAmbientLightColor;
            }

            resultColor.Saturate();
            resultColor.Saturate();
            return resultColor;
        }
    }
}
