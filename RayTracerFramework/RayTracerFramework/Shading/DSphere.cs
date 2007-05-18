using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using RayTracerFramework.Geometry;

namespace RayTracerFramework.Shading {
    class DSphere : Sphere, IObject {
        private Material material;

        public DSphere(Vec3 center, float radius, Material material) : base(center, radius) {
            this.material = material;
        }

	    private DSphere(float radius, Matrix transform, 
                    Matrix invTransform, Material material):base(radius, transform, invTransform) {
            this.material = material;
	    }

        public Color Shade(Ray ray, IntersectionPoint intersection, ILightingModel lightingModel,
                           LightManager lightManager) {
            return lightingModel.calculateColor(ray, intersection, material, lightManager);
            
            //float factor = Vec3.Dot(-Vec3.StdZAxis, intersection.normal);
            //return Color.FromArgb((int)(((float)emissive.R) * factor), (int)(((float)emissive.G) * factor), (int)(((float)emissive.B) * factor));
        }


        public IObject Clone() {
            return new DSphere(radius, transform, invTransform, material);
        }

        public Color Emissive {
            get { return material.emissive; }
            set { material.emissive = value; }
        }

        public Color Ambient {
            get { return material.ambient; }
            set { material.ambient = value; }
        }

        public Color Diffuse {
            get { return material.diffuse; }
            set { material.diffuse = value; }
        }

        public Color Specular {
            get { return material.specular; }
            set { material.specular = value; }
        }

        public float SpecularPower {
            get { return material.specularPower; }
            set { material.specularPower = value; }
        }
    }
}
