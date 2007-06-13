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
            
            foreach (Light light in scene.lightManager.LightsWorldSpace) {
          
                switch (light.lightType) {
                    case LightType.Point:
                        PointLight pointLight = (PointLight)light;
                        Vec3 N = intersection.normal;
                        Vec3 posToLight = pointLight.position - intersection.position;
                        Vec3 L = Vec3.Normalize(posToLight);
                        float distanceToLight = posToLight.Length;

                        // Points normal away from light?
                        float diffuse = Vec3.Dot(L, N);
                        if (diffuse < 0) {
                            iTotal = iTotal + material.ambient * pointLight.ambient;
                            continue;
                        }
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
                        iTotal = iTotal + (material.ambient * pointLight.ambient) +
                                          (material.GetDiffuse(intersection.textureCoordinates) * pointLight.diffuse * diffuse) +
                                          (material.specular * pointLight.specular * specular);
                          
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
