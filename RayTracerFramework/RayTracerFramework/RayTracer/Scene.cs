using System;
using System.Collections.Generic;
using System.Text;
using RayTracerFramework.Geometry;
using RayTracerFramework.Utility;
using RayTracerFramework.Shading;
using System.Drawing;
using Color = RayTracerFramework.Shading.Color;

namespace RayTracerFramework.RayTracer {
    class Scene : IIntersectable {
        public GeometryManager geoMng;
        public ILightingModel lightingModel;
        public LightManager lightManager;
        public Camera cam;

        public CubeMap cubeMap;
        public Color backgroundColor;
        public bool useCubeMap;
        
        public float refractionIndex = 1.0f;

        public Scene(Camera cam) {       
            this.cam = cam;
            geoMng = new GeometryManager();
            //geoMng.viewMatrix = cam.GetViewMatrix();
            
            lightingModel = new BlinnPhongLightingModel();
            lightManager = new LightManager();

            backgroundColor = Color.LightSlateGray;
            cubeMap = new CubeMap(100, 100, 100, "stpeters");
            useCubeMap = true;

            //refractionIndex = 1.0f;         
        }

        public void Setup() {
            // Transform all objects and the cubemap into world space
            geoMng.TransformAll();
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

        public Color GetBackgroundColor(Ray ray) {
            if (useCubeMap)
                return cubeMap.getColor(ray);
            return backgroundColor;
                
        }

        public bool Intersect(Ray ray) {
            foreach (IObject geoObj in geoMng.TransformedObjects) {
                if (geoObj.Intersect(ray))
                    return true;                
            }
            return false;
        }

        public bool Intersect(Ray ray, out RayIntersectionPoint firstIntersection) {
            firstIntersection = null;
            float nearestT = float.PositiveInfinity;
            RayIntersectionPoint currentIntersection;
            foreach (IObject geoObj in geoMng.TransformedObjects) {                
                if (geoObj.Intersect(ray, out currentIntersection)) {
                    if (currentIntersection.t < nearestT) {
                        nearestT = currentIntersection.t;
                        firstIntersection = currentIntersection;
                    }
                }
            }
            return firstIntersection != null;
        }

        public int Intersect(Ray ray, out SortedList<float, RayIntersectionPoint> intersections) {
            intersections = new SortedList<float, RayIntersectionPoint>();
            int numIntersections = 0, numCurrentIntersections;
            //float nearestT = float.PositiveInfinity;
            SortedList<float, RayIntersectionPoint> currentIntersections;
            foreach (IObject geoObj in geoMng.TransformedObjects) {
                numCurrentIntersections = geoObj.Intersect(ray, out currentIntersections);
                if(numCurrentIntersections > 0) {
                    numIntersections += numCurrentIntersections;
                    foreach (KeyValuePair<float, RayIntersectionPoint> intersectionPair in currentIntersections) {
                        intersections.Add(intersectionPair.Key, intersectionPair.Value);
                        numIntersections++;
                    }
                }
            }
            return numIntersections;
        }

    }
}
