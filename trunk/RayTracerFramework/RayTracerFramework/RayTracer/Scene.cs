using System;
using System.Collections.Generic;
using System.Text;
using RayTracerFramework.Geometry;
using RayTracerFramework.Utility;
using RayTracerFramework.Shading;
using System.Drawing;
using Color = RayTracerFramework.Shading.Color;
using RayTracerFramework.PhotonMapping;

namespace RayTracerFramework.RayTracer {


    public class Scene : IIntersectable {
        public GeoObjectKDTree kdTree;
        public ILightingModel lightingModel;
        public LightManager lightManager;
        public PhotonMap photonMap;
        public Camera cam;

        public CubeMap cubeMap;
        public Color backgroundColor;
        public bool useCubeMap;
      
        public float mediumRefractionIndex;
        public Color mediumColor;
        public Color surfacesAmbientLightColor;
        public Color fogAmbientLightColor;

        public Scene() { }

        public Scene(Camera cam) {
            photonMap = null;
            this.cam = cam;
            kdTree = new GeoObjectKDTree();
            
            lightingModel = new BlinnPhongLightingModel();
            lightManager = new LightManager();

            backgroundColor = Color.LightSlateGray;
            cubeMap = new CubeMap(100, 100, 100, "stpeters");
            useCubeMap = true;          

            mediumRefractionIndex = 1.0f;
            mediumColor = Settings.Setup.Scene.StdFogColor;
        }

        public float GetTotalPhotonLightPower() {
            float totalPhotonLightPower = 0;
            foreach (PhotonMapping.Light light in this.lightManager.PhotonLightsWorldSpace)
                totalPhotonLightPower += light.power;
            return totalPhotonLightPower;
        }

        public void SetupPhotonMapping(
                int photonMapSize, 
                System.Windows.Forms.ProgressBar progressBar,
                System.Windows.Forms.StatusStrip statusBar) {

        }

        public DPoint AddDPoint(Vec3 position) { 
            DPoint point = new DPoint(position);
            kdTree.content.Add(point);
            return point;
        }

        public void AddDPoints(List<IIntersectable> points) {
            kdTree.content.AddRange(points);
        }

        public DSphere AddDSphere(Vec3 worldPos, float radius, Material material) {
            DSphere sphere = new DSphere(Vec3.Zero, radius, material.diffuseTexture != null, material);
            sphere.Transform(Matrix.GetTranslation(worldPos));
            kdTree.content.Add(sphere);
            return sphere;
        }

        public DMesh AddDMesh(DMesh mesh, Matrix transformation) {
            mesh.Transform(transformation);
            kdTree.content.Add(mesh);
            return mesh;
        }

        public DBox AddDBox(
                Matrix transformation, 
                float width, 
                float height, 
                float depth, 
                bool textured, 
                Material material) {
            DBox box = new DBox(Vec3.Zero, width, height, depth, textured, material);
            box.Transform(Matrix.GetTranslation(-width / 2f, -height / 2f, -depth / 2f));
            box.Transform(transformation);
            kdTree.content.Add(box);
            return box;
        }

        public void Setup(
                System.Windows.Forms.ProgressBar progressBar, 
                System.Windows.Forms.StatusStrip statusBar) {
            // Setup ambient light
            surfacesAmbientLightColor = new Color();
            foreach (Shading.Light light in lightManager.BlinnLightsWorldSpace) {
                switch (light.lightType) {
                    case Shading.LightType.Point:
                        surfacesAmbientLightColor = surfacesAmbientLightColor + ((Shading.PointLight)light).ambient;
                        break;
                    case Shading.LightType.Directional:
                        break;
                }
            }
            fogAmbientLightColor = surfacesAmbientLightColor * Settings.Setup.Scene.FogAmbientLightAmplifier;

            // Optimize KD-Tree
            kdTree.Optimize();

            // Emit photons
            if (Settings.Setup.PhotonMapping.EmitPhotons) {
                PhotonTracer photonTracer = new PhotonTracer(
                        this,
                        Settings.Setup.PhotonMapping.StoredPhotonsCount,
                        Settings.Setup.PhotonMapping.TracingMaxRecursionDepth,
                        progressBar,
                        statusBar);
                photonTracer.EmitPhotons();
            }
        }

        public Color GetBackgroundColor(Ray ray) {
            if (useCubeMap)
                return cubeMap.getColor(ray);
            return backgroundColor;
                
        }

        public bool Intersect(Ray ray) {
            return kdTree.Intersect(ray);
        }

        public bool Intersect(Ray ray, out RayIntersectionPoint firstIntersection) {
            return kdTree.Intersect(ray, out firstIntersection);
        }

        public int Intersect(Ray ray, ref SortedList<float, RayIntersectionPoint> intersections) {            
            return kdTree.Intersect(ray, ref intersections);
        }

    }
}
