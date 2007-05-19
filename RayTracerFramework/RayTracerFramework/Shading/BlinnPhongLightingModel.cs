using System;
using System.Collections.Generic;
using System.Text;
using RayTracerFramework.Geometry;
using RayTracerFramework.RayTracer;

namespace RayTracerFramework.Shading {

    class BlinnPhongLightingModel : ILightingModel {


        public Color calculateColor(Ray ray, RayIntersectionPoint intersection,
                                             Material material, Scene scene) {
            Color iTotal = new Color();
            //bool lighting = true;
            
            foreach (Light light in scene.lightManager.LightsViewSpace) {
               
                switch (light.lightType) {
                    case LightType.Point:
                        PointLight pointLight = (PointLight)light;
                        Vec3 N = intersection.normal;
                        Vec3 posToLight = pointLight.position - intersection.position;
                        Vec3 L = Vec3.Normalize(posToLight);
                        float distanceToLight = posToLight.Length;

                        // Is light in shadow?
                        Vec3 toLightRayPos = new Vec3(intersection.position + Ray.positionEpsilon * L);
                        Ray toLightRay = new Ray(toLightRayPos, L);
                        RayIntersectionPoint firstIntersection;
                        if (scene.Intersect(toLightRay, out firstIntersection) && firstIntersection.t < distanceToLight) {
                            iTotal = iTotal + material.ambient * pointLight.ambient;
                            continue;
                        }
                        
                        float diffuse = Vec3.Dot(L, N);
                        if (diffuse < 0) { // Point faces away from the point light
                            iTotal = iTotal + material.ambient * pointLight.ambient;
                            
                        
                        } else {
                            Vec3 V = -ray.direction;
                            Vec3 H = Vec3.Normalize(L + V);

                            float specular = (float)Math.Pow(Vec3.Dot(H, N), material.specularPower);
                            iTotal = iTotal + (material.ambient * pointLight.ambient) +
                                              (material.diffuse * pointLight.diffuse * diffuse) +
                                              (material.specular * pointLight.specular * specular);
                        }  
                        break;
                    case LightType.Directional:

                        break;
                } // end switch
                

            }

            iTotal.Saturate();
            return iTotal;
            
            //Light l = lightManager.lights[0];
            //if (l.lightType == LightType.Point) {
            //    PointLight pointLight = (PointLight)l;
               
            //   // return Color.FromArgb((int)(((float)material.diffuse.R) * factor), (int)(((float)material.diffuse.G) * factor), (int)(((float)material.diffuse.B) * factor));
            //} else {
            //    Console.WriteLine("Kein Punktlicht!");
            //    return Color.Blue;
            //}
            //float factor = Vec3.Dot(-Vec3.StdZAxis, intersection.normal);
            //return Color.FromArgb((int)(((float)emissive.R) * factor), (int)(((float)emissive.G) * factor), (int)(((float)emissive.B) * factor));
        }
    }
}
