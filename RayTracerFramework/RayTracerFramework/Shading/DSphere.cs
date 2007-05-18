using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using RayTracerFramework.Geometry;

namespace RayTracerFramework.Shading {
    class DSphere : Sphere, IObject {
        public Color emissive;

        public DSphere(Vec3 center, float radius, Color emissiveColor) : base(center, radius) {
            this.emissive = emissiveColor;
        }

        public System.Drawing.Color Shade(Ray ray, IntersectionPoint intersection) {
            // Use simple diffuse lighing with fixed directional light
            float factor = Vec3.Dot(-Vec3.StdZAxis, intersection.normal);
            return Color.FromArgb((int)(((float)emissive.R) * factor), (int)(((float)emissive.G) * factor), (int)(((float)emissive.B) * factor));
        }

        public IObject Clone() {
            return new DSphere(center, radius, emissive);
        }
    }
}
