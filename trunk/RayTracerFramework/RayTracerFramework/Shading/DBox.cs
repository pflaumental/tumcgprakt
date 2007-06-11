using System;
using System.Collections.Generic;
using System.Text;
using RayTracerFramework.Shading;
using RayTracerFramework.RayTracer;

namespace RayTracerFramework.Geometry {
    class DBox : Box, IObject {
        private Material material;

        public DBox(
                Vec3 position,
                float width,
                float height,
                float depth,
                bool textured,
                Material material)
            : base(position, width, height, depth, textured) {
            this.material = material;
        }

        protected DBox(
                float width,
                float height,
                float depth,
                bool textured,
                Matrix transform,
                Matrix invTransform,
                BSphere boundingSphere,
                Material material) 
            : base(transform, invTransform, width, height, depth, textured, boundingSphere) {
            this.material = material;
        }

        public Color Shade(Ray ray, RayIntersectionPoint intersection, Scene scene, float contribution) {
            return StdShading.RecursiveShade(ray, intersection, scene, material, contribution);
        }

        public Vec2 GetTextureCoordinates(Vec3 localPoint) { throw new NotImplementedException("DBox::GetTextureCoordinates not implemented."); }


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
