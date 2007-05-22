using System;
using System.Collections.Generic;
using System.Text;
using RayTracerFramework.Geometry;
using RayTracerFramework.RayTracer;

namespace RayTracerFramework.Shading {
    // Standard recursive raytracing
    class StdShading {
        private readonly static float localThreshold = 0.02f;
        private readonly static float reflectionThreshold = 0.02f;
        private readonly static float refractionThreshold = 0.02f;        
        private readonly static float contributionThreshold = 0.05f;        

        public static Color RecursiveShade(
                Ray ray, 
                RayIntersectionPoint intersection, 
                Scene scene,
                Material material, 
                float contribution) {

            Color resultColor = new Color();
            float localPart = 1.0f - (material.reflectionPart + material.refractionPart);

            // Local color
            if (localPart > localThreshold) {
                Color localColor = scene.lightingModel.calculateColor(
                        ray,
                        intersection,
                        material,
                        scene);
                resultColor += (localColor * localPart);
            }

            // Reflection color
            float reflectionContribution = material.reflectionPart * contribution;
            if (material.reflectionPart > reflectionThreshold
                    && reflectionContribution > contributionThreshold
                    && ray.recursionDepth + 1 <= Renderer.MaxRecursionDepth) { 
                // Calculate reflection ray
                // R = 2N(N*V)-V   (V = -ray.direction)
                float NV = Vec3.Dot(intersection.normal, -ray.direction);
                // assert if(NV < 0) throw new Execption("NV < 0")

                Vec3 reflectionDir = Vec3.Normalize(2.0f * NV * intersection.normal + ray.direction);
                Vec3 reflectionPos = intersection.position + Ray.positionEpsilon * reflectionDir;
                Ray reflectionRay = new Ray(reflectionPos, reflectionDir, ray.recursionDepth + 1);

                // Find nearest object intersection and shade reflection  
                RayIntersectionPoint firstIntersection;
                Color reflectionColor;
                if (scene.Intersect(reflectionRay, out firstIntersection)) {
                    IObject firstHitObject = (IObject)firstIntersection.hitObject;
                    reflectionColor = firstHitObject.Shade(reflectionRay, firstIntersection, scene, reflectionContribution);
                }
                else {
                    //reflectionColor = scene.backgroundColor;//GetBackgroundColor(reflectionRay);
                    reflectionColor = scene.GetBackgroundColor(reflectionRay);
                }
                // Add reflection color to resultColor
                resultColor += (reflectionColor * material.reflectionPart);
            }


            // Refraction color
            float refractionContribution = material.refractionPart * contribution;
            if (material.refractionPart > refractionThreshold
                    && refractionContribution > contributionThreshold
                    && ray.recursionDepth + 1 <= Renderer.MaxRecursionDepth)
            {
                // Calculate refraction ray
                // ((ni/nr)*(N*V) - sqrt(1 - (ni/nr)^2*(1.0f-(N*V)^2))) * N - (ni/nr) * V
                float NV = Vec3.Dot(intersection.normal, -ray.direction);
                // assert if(NV < 0) throw new Execption("NV < 0")

                float refractionRatio = scene.refractionIndex / material.refractionIndex;                
                float cosThetaR = (float)Math.Sqrt(1f - refractionRatio * refractionRatio * (1f - NV * NV));
                // Clamp cosine
                if (float.IsNaN(cosThetaR))
                    cosThetaR = 1f;
                float beforeNTerm = (float)(refractionRatio * NV - cosThetaR);
                Vec3 refractionDir = Vec3.Normalize( beforeNTerm * intersection.normal + refractionRatio * ray.direction);

                // assert if(Vec3.Dot(refractionDir, intersection.normal) > 0) throw new Exception("Vec3.Dot(refractionDir, intersection.normal) > 0");
                // assert if (Vec3.Dot(refractionDir, ray.direction) < 0) throw new Exception("Vec3.Dot(refractionDir, ray.direction) < 0");
                
                Vec3 refractionPos = intersection.position - Ray.positionEpsilon * intersection.normal;//refractionDir;
                // assert if (Vec3.Dot(refractionPos - intersection.position, intersection.normal) > 0) throw new Exception("Vec3.Dot(refractionPos - intersection.position, intersection.normal) < 0");                    
                Ray refractionRay = new Ray(refractionPos, refractionDir, ray.recursionDepth + 1);

                // Get refraction-ray intersection with the object
                RayIntersectionPoint refractionIntersection;
                // Optical assertion: ^^
                if (!intersection.hitObject.Intersect(refractionRay, out refractionIntersection))
                    return Color.Red;//material.refractionPart * scene.GetBackgroundColor(refractionRay);

                // Calculate second (outside) refraction ray                    
                NV = Vec3.Dot(refractionIntersection.normal, -refractionDir);
                // assert if(NV < 0) throw new Execption("NV < 0")
                refractionRatio = 1f / refractionRatio;

                cosThetaR = (float)Math.Sqrt(1f - refractionRatio * refractionRatio * (1f - NV * NV));
                // Clamp cosine
                if (float.IsNaN(cosThetaR))
                    cosThetaR = 1f;
                beforeNTerm = (float)(refractionRatio * NV - cosThetaR);
                refractionDir = Vec3.Normalize(beforeNTerm * refractionIntersection.normal + refractionRatio * refractionDir);
                if (float.IsNaN(refractionDir.x))
                    throw new Exception("refraction dir is NaN");
                refractionPos = refractionIntersection.position - Ray.positionEpsilon * refractionIntersection.normal;
                refractionRay = new Ray(refractionPos, refractionDir, ray.recursionDepth + 2);

                // Test refracted ray against szene and calculate color
                RayIntersectionPoint firstIntersection;
                Color refractionColor;
                if (scene.Intersect(refractionRay, out firstIntersection)) {
                    IObject firstHitObject = (IObject)firstIntersection.hitObject;
                    refractionColor = firstHitObject.Shade(refractionRay, firstIntersection, scene, refractionContribution);
                } else
                    refractionColor = scene.GetBackgroundColor(refractionRay);
                // Add refraction color to resultColor
                resultColor += (refractionColor * material.refractionPart);                 
            }

            return resultColor;
        }
    }
}
