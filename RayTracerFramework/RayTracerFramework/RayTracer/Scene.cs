using System;
using System.Collections.Generic;
using System.Text;
using RayTracerFramework.Geometry;
using System.Drawing;
using RayTracerFramework.Utility;
using RayTracerFramework.Shading;

namespace RayTracerFramework.RayTracer {
    class Scene {
        public GeometryManager geoMng;
        public Camera cam;
        public Color backgroundColor;

        public Scene() {
            geoMng = new GeometryManager();
            cam = new Camera();
            backgroundColor = Color.LightSlateGray;
        }

        public void AddInstance(IObject geoObj, Matrix worldMatrix) {
            geoMng.AddInstance(geoObj, worldMatrix);
        }

        public void AddInstance(IObject geoObj, Vec3 worldPos) {
            geoMng.AddInstance(geoObj, Matrix.GetTranslation(worldPos));
        }

        public DSphere AddDSphere(Vec3 worldPos, float radius, Color color) {
            DSphere sphere = new DSphere(Vec3.Zero, radius, color);
            geoMng.AddInstance(sphere, Matrix.GetTranslation(worldPos));
            return sphere;
        }        
    }
}
