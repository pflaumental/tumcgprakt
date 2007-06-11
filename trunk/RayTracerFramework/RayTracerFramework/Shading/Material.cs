using System;
using System.Collections.Generic;
using System.Text;
using RayTracerFramework.Utility;
using RayTracerFramework.Geometry;

namespace RayTracerFramework.Shading {
    class Material {
        public Color emissive;
        public Color ambient;
        public Color diffuse;
        public Color specular;

        public float specularPower;

        public bool reflective;
        public bool refractive;

        public float refractionRatio; // world_refractionIndex / material_refractionIndex
                                      // between 0 and 1
        public float r0; // fresnel term for rays which hit the surface in right angle
        public float oneMinusR0; // 1 - r0
        public float refractionRate; // amount of non-reflected light that becomes refracted
                                     // between 0 and 1

        public FastBitmap diffuseTexture;

        public string name = "default";

        public static readonly Material WhiteMaterial = new Material();
        public static readonly Material RedMaterial = new Material(Color.Red, Color.Red, Color.Red, Color.Red, 10, false, false, 0, 0, null);
        public static readonly Material GreenMaterial = new Material(Color.Green, Color.Green, Color.Green, Color.Green, 10, false, false, 0, 0, null);
        public static readonly Material BlueMaterial = new Material(Color.Blue, Color.Blue, Color.Blue, Color.Blue, 10, false, false, 0, 0, null);

        public Material() : this(Color.White, new Color(64, 45, 26), new Color(147, 103, 55), new Color(209, 167, 143), 10, false, false, 0, 0, null) { }
        public Material(Material other) {
            this.emissive = other.emissive;
            this.ambient = other.ambient;
            this.diffuse = other.diffuse;
            this.specular = other.specular;
            this.specularPower = other.specularPower;
            this.r0 = other.r0;
            this.oneMinusR0 = other.oneMinusR0;
            this.reflective = other.reflective;
            this.refractionRate = other.refractionRate;
            this.refractionRatio = other.refractionRatio;
            this.refractive = other.refractive;
            this.diffuseTexture = other.diffuseTexture;
        }

        public Material(Color emissive, Color ambient, Color diffuse,
                        Color specular, float specularPower,
                        bool reflective, bool refractive,
                        float refractionRatio,
                        float refractionRate,
                        FastBitmap diffuseTexture) {
            this.emissive = emissive;
            this.ambient = ambient;
            this.diffuse = diffuse;
            this.specular = specular;
            this.specularPower = specularPower;
            this.reflective = reflective;
            this.refractive = refractive;
            this.refractionRatio = refractionRatio;
            float oneMinusRefractionRatio = 1f - refractionRatio;
            float onePlusRefractionRatio = 1f + refractionRatio;
            this.r0 = (oneMinusRefractionRatio * oneMinusRefractionRatio)
                    / (onePlusRefractionRatio * onePlusRefractionRatio);
            this.oneMinusR0 = 1f - r0;
            this.refractionRate = refractionRate;
            this.diffuseTexture = diffuseTexture;
        }

        public Color GetDiffuse(Vec2 textureCoordinates) {
            if (diffuseTexture == null)
                return diffuse;
            else
                return diffuseTexture.GetPixel(
                        textureCoordinates.x * (diffuseTexture.Width - 1), 
                        textureCoordinates.y * (diffuseTexture.Height - 1));
        }
    }
}
