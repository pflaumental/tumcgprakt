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
        public float refractionIndex;

        public string name = "default";

        public static readonly Material WhiteMaterial = new Material();
        public static readonly Material RedMaterial = new Material(Color.Red, Color.Red, Color.Red, Color.Red, 10, 0, 0, 5f);
        public static readonly Material GreenMaterial = new Material(Color.Green, Color.Green, Color.Green, Color.Green, 10, 0, 0, 5);
        public static readonly Material BlueMaterial = new Material(Color.Blue, Color.Blue, Color.Blue, Color.Blue, 10, 0, 0, 5f);


        // public Material() : this(Color.White, Color.White, Color.White, Color.White, 10, 0f, 0.0f, 2f) { }

        public Material() : this(Color.White, new Color(64, 45, 26), new Color(147, 103, 55), new Color(209, 167, 143), 10, 0.5f, 0, 2) { }
        public Material(Material other) {
            this.emissive = other.emissive;
            this.ambient = other.ambient;
            this.diffuse = other.diffuse;
            this.specular = other.specular;
            this.specularPower = other.specularPower;
            this.reflectionPart = other.reflectionPart;
            this.refractionPart = other.refractionPart;
            this.refractionIndex = other.refractionIndex;
        }

        public Material(Color emissive, Color ambient, Color diffuse,
                        Color specular, float specularPower,
                        float reflectionPart, float refractionPart, float refractionIndex) {
            this.emissive = emissive;
            this.ambient = ambient;
            this.diffuse = diffuse;
            this.specular = specular;
            this.specularPower = specularPower;
            this.reflectionPart = reflectionPart;
            this.refractionPart = refractionPart;
            this.refractionIndex = refractionIndex;
        }
    }
}
