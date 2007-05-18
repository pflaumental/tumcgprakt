using System;
using System.Collections.Generic;
using System.Text;
using RayTracerFramework.Geometry;
using RayTracerFramework.Utility;
using RayTracerFramework.Shading;

namespace RayTracerFramework.RayTracer {
    class Scene {
        public GeometryManager geoMng;
        public ILightingModel lightingModel;
        public LightManager lightManager;
        public Camera cam;
        public Color backgroundColor;

        public Scene() {
            geoMng = new GeometryManager();
            lightingModel = new BlinnPhongLightingModel();
            lightManager = new LightManager();
            cam = new Camera();
            backgroundColor = Color.LightSlateGray;
        }

        public void AddInstance(IObject geoObj, Matrix worldMatrix) {
            geoMng.AddInstance(geoObj, worldMatrix);
        }

        public void AddInstance(IObject geoObj, Vec3 worldPos) {
            geoMng.AddInstance(geoObj, Matrix.GetTranslation(worldPos));
        }

        public DSphere AddDSphere(Vec3 worldPos, float radius, Material material) {
            DSphere sphere = new DSphere(Vec3.Zero, radius, material);
            geoMng.AddInstance(sphere, Matrix.GetTranslation(worldPos));
            return sphere;
        }

        public DBox AddDBox(Vec3 worldPos, float width, float height, float depth, Material material) {
            DBox box = new DBox(Vec3.Zero, width, height, depth, material);
            geoMng.AddInstance(box, Matrix.GetTranslation(worldPos));
            return box;
        }        
    }
}
