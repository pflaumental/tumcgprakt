using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using RayTracerFramework.Geometry;
using RayTracerFramework.RayTracer;

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

        public Color Shade(Ray ray, RayIntersectionPoint intersection, Scene scene, float contribution) {
            return StdShading.RecursiveShade(ray, intersection, scene, material, contribution);
        }

        public Vec2 GetTextureCoordinates(Vec3 localPoint) {
            throw new NotImplementedException("GetTextureCoordinates not implemeted.");
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

        public Material Material { get { return material; } set { material = value; } }
    }
}
