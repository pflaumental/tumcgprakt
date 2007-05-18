using System;
using System.Collections.Generic;
using System.Text;
using RayTracerFramework.Geometry;

namespace RayTracerFramework.Shading {

    class BlinnPhongLightingModel : ILightingModel {


        public Color calculateColor(Ray ray, IntersectionPoint intersection,
                                             Material material, LightManager lightManager) {
            Light l = lightManager.lights[0];
            LightType type = l.lightType;
            if (type == LightType.Point) {
                PointLight pointLight = (PointLight)l;
                float factor = Vec3.Dot(Vec3.Normalize(pointLight.position - intersection.position),
                                     intersection.normal);
                if (factor < 0)
                    factor = 0;
                return material.diffuse * factor;
               // return Color.FromArgb((int)(((float)material.diffuse.R) * factor), (int)(((float)material.diffuse.G) * factor), (int)(((float)material.diffuse.B) * factor));
            } else {
                Console.WriteLine("Kein Punktlicht!");
                return Color.Blue;
            }
            //float factor = Vec3.Dot(-Vec3.StdZAxis, intersection.normal);
            //return Color.FromArgb((int)(((float)emissive.R) * factor), (int)(((float)emissive.G) * factor), (int)(((float)emissive.B) * factor));
        }
    }
}
