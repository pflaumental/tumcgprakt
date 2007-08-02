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
        //public List<IObject> transformedObjects;
        public GeoObjectKDTree kdTree;
        public ILightingModel lightingModel;
        public LightManager lightManager;
        public PhotonMap photonMap;
        public PhotonTracer photonTracer;
        public Camera cam;

        public CubeMap cubeMap;
        public Color backgroundColor;
        public bool useCubeMap;
        public bool usePhotonMapping;
      
        public float mediumRefractionIndex;
        public Color mediumColor;

        public Scene() { }

        public Scene(Camera cam) {
            photonTracer = null;
            photonMap = null;
            this.cam = cam;
            //transformedObjects = new List<IObject>();
            kdTree = new GeoObjectKDTree();
            
            lightingModel = new BlinnPhongLightingModel();
            lightManager = new LightManager();

            backgroundColor = Color.LightSlateGray;
            cubeMap = new CubeMap(100, 100, 100, "stpeters");
            useCubeMap = true;          

            mediumRefractionIndex = 1.0f;
            mediumColor = new Color(1f, 0.5f, 0.5f);
        }

        public float GetTotalPhotonLightPower() {
            float totalPhotonLightPower = 0;
            foreach (PhotonMapping.Light light in this.lightManager.PhotonLightsWorldSpace)
                totalPhotonLightPower += light.power;
            return totalPhotonLightPower;
        }

        public void ActivatePhotonMapping(
                int photonMapSize, 
                System.Windows.Forms.ProgressBar progressBar,
                System.Windows.Forms.StatusStrip statusBar) {
            usePhotonMapping = true;
            photonTracer = new PhotonTracer(
                    this, 
                    photonMapSize, 
                    4, 
                    progressBar,
                    statusBar);
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
            //transformedObjects.Add(sphere);
            kdTree.content.Add(sphere);
            return sphere;
        }

        public DMesh AddDMesh(DMesh mesh, Matrix transformation) {
            mesh.Transform(transformation);
            //transformedObjects.Add(mesh);
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
            //transformedObjects.Add(box);
            kdTree.content.Add(box);
            return box;
        }

        public void Setup() {            
            kdTree.Optimize();
            if (photonTracer != null) {
                photonMap = photonTracer.EmitPhotons();
            }
        }

        public Color GetBackgroundColor(Ray ray) {
            if (useCubeMap)
                return cubeMap.getColor(ray);
            return backgroundColor;
                
        }

        public bool Intersect(Ray ray) {
            //foreach (IObject geoObj in transformedObjects) {
            //    if (geoObj.Intersect(ray))
            //        return true;                
            //}
            //return false;
            return kdTree.Intersect(ray);
        }

        public bool Intersect(Ray ray, out RayIntersectionPoint firstIntersection) {
            //firstIntersection = null;
            //float nearestT = float.PositiveInfinity;
            //RayIntersectionPoint currentIntersection;
            //foreach (IObject geoObj in transformedObjects) {                
            //    if (geoObj.Intersect(ray, out currentIntersection)) {
            //        if (currentIntersection.t < nearestT) {
            //            nearestT = currentIntersection.t;
            //            firstIntersection = currentIntersection;
            //        }
            //    }
            //}
            //return firstIntersection != null;
            return kdTree.Intersect(ray, out firstIntersection);
        }

        public int Intersect(Ray ray, ref SortedList<float, RayIntersectionPoint> intersections) {            
            //int numIntersections = 0;
            //foreach (IObject geoObj in transformedObjects) {
            //    numIntersections += geoObj.Intersect(ray, ref intersections);
            //}
            //return numIntersections;
            return kdTree.Intersect(ray, ref intersections);
        }

    }
}
