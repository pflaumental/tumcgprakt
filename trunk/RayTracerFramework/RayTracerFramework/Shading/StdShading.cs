using System;
using System.Collections.Generic;
using System.Text;
using RayTracerFramework.Geometry;
using RayTracerFramework.RayTracer;

namespace RayTracerFramework.Shading {
    // Standard recursive raytracing
    class StdShading {
        private readonly static float localThreshold = 0.05f;
        private readonly static float reflectionThreshold = 0.05f;
        private readonly static float refractionThreshold = 0.05f;        
        private readonly static float contributionThreshold = 0.05f;

        private readonly static float positionEpsilon = 0.0001f;

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
                        scene.lightManager,
                        scene.geoMng);
                resultColor += (localColor * localPart);
            }

            // Reflection color
            float reflectionContribution = material.reflectionPart * contribution;
            if (material.reflectionPart > reflectionThreshold
                    && reflectionContribution > contributionThreshold) { 
                // Calculate reflection ray
                // R = 2N(N*V)-V   (V = -ray.direction)
                float NV = Vec3.Dot(intersection.normal, -ray.direction);
                if (NV > 0) {
                    Vec3 reflectionDir = Vec3.Normalize(2.0f * NV * intersection.normal + ray.direction);
                    Vec3 reflectionPos = intersection.position + positionEpsilon * reflectionDir;
                    Ray reflectionRay = new Ray(reflectionPos, reflectionDir);

                    // Find nearest object intersection
                    RayIntersectionPoint firstIntersection = null, nearestIntersection = null;
                    float nearestT = float.PositiveInfinity;
                    IObject nearestObject = null;
                    foreach (IObject geoObj in scene.geoMng.TransformedObjects) {
                        if (geoObj.Intersect(reflectionRay, out firstIntersection)) {
                            if (firstIntersection.t < nearestT) {
                                nearestT = firstIntersection.t;
                                nearestObject = geoObj; ;
                                nearestIntersection = firstIntersection;
                            }
                        }
                    }

                    // Shade reflection      
                    Color reflectionColor;
                    if (nearestObject != null)
                        reflectionColor = nearestObject.Shade(ray, nearestIntersection, scene, reflectionContribution);
                    else
                        reflectionColor = scene.backgroundColor;

                    // Add reflection color to resultColor
                    resultColor += (reflectionColor * material.reflectionPart);
                }
            }
            return resultColor;
        }
    }
}
