using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracerFramework.Shading {
    class Material {
        public Color emissive;
        public Color ambient;
        public Color diffuse;
        public Color specular;
        public float specularPower;

        public float reflectionPart;
        public float refractionPart;

        public static readonly Material WhiteMaterial = new Material();
        public static readonly Material RedMaterial = new Material(Color.Red, Color.Red, Color.Red, Color.Red, 10, 0.5f, 0.5f);
        public static readonly Material GreenMaterial = new Material(Color.Green, Color.Green, Color.Green, Color.Green, 10, 0.5f, 0.5f);
        public static readonly Material BlueMaterial = new Material(Color.Blue, Color.Blue, Color.Blue, Color.Blue, 10, 0.5f, 0.5f);


        public Material() : this(Color.White, Color.White, Color.White, Color.White, 10, 0.5f, 0.5f) { }

        public Material(Color emissive, Color ambient, Color diffuse,
                        Color specular, float specularPower,
                        float reflectionPart, float refractionPart) {
            this.emissive = emissive;
            this.ambient = ambient;
            this.diffuse = diffuse;
            this.specular = specular;
            this.specularPower = specularPower;
            this.reflectionPart = reflectionPart;
            this.refractionPart = refractionPart;
        }
    }
}
