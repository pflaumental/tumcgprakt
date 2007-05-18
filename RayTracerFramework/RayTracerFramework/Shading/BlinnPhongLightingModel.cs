using System;
using System.Collections.Generic;
using System.Text;
using RayTracerFramework.Geometry;

namespace RayTracerFramework.Shading {

    class BlinnPhongLightingModel : ILightingModel {


        public Color calculateColor(Ray ray, IntersectionPoint intersection,
                                             Material material, LightManager lightManager) {
            Color iTotal = new Color();
            foreach (Light light in lightManager.lights) {
                switch (light.lightType) {
                    case LightType.Point:
                        PointLight pointLight = (PointLight)light;
                        Vec3 N = intersection.normal;
                        Vec3 L = Vec3.Normalize(pointLight.position - intersection.position);
                        float diffuse = Vec3.Dot(L, N);
                        if (diffuse < 0)
                            diffuse = 0;

                        Vec3 V = -ray.direction;
                        Vec3 H = Vec3.Normalize(L + V);
                        float specular = (float)Math.Pow(Vec3.Dot(H, N), material.specularPower);
                        Color diffuseColor = material.diffuse * pointLight.diffuse * diffuse;
                        Color specularColor = material.specular * pointLight.specular * specular;
                        Color ambientColor = material.ambient * pointLight.ambient;
                        iTotal = iTotal + specularColor + diffuseColor + ambientColor;

                        break;
                }
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
