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
        public List<IObject> transformedObjects;
        public ILightingModel lightingModel;
        public LightManager lightManager;
        public Camera cam;

        public CubeMap cubeMap;
        public Color backgroundColor;
        public bool useCubeMap;
        
        public float refractionIndex = 1.0f;

        public Scene(Camera cam) {       
            this.cam = cam;
            transformedObjects = new List<IObject>();
            
            lightingModel = new BlinnPhongLightingModel();
            lightManager = new LightManager();

            backgroundColor = Color.LightSlateGray;
            cubeMap = new CubeMap(100, 100, 100, "stpeters");
            useCubeMap = true;

            //refractionIndex = 1.0f;         
        }

        public DSphere AddDSphere(Vec3 worldPos, float radius, Material material) {
            DSphere sphere = new DSphere(Vec3.Zero, radius, material);
            sphere.Transform(Matrix.GetTranslation(worldPos));
            transformedObjects.Add(sphere);
            return sphere;
        }

        public DMesh AddDMesh(DMesh mesh, Matrix transformation) {
            mesh.Transform(transformation);
            transformedObjects.Add(mesh);
            return mesh;
        }

        public DBox AddDBox(Matrix transformation, float width, float height, float depth, Material material) {
            DBox box = new DBox(Vec3.Zero, width, height, depth, material);
            box.Transform(transformation);
            transformedObjects.Add(box);
            return box;
        }

        public Color GetBackgroundColor(Ray ray) {
            if (useCubeMap)
                return cubeMap.getColor(ray);
            return backgroundColor;
                
        }

        public bool Intersect(Ray ray) {
            foreach (IObject geoObj in transformedObjects) {
                if (geoObj.Intersect(ray))
                    return true;                
            }
            return false;
        }

        public bool Intersect(Ray ray, out RayIntersectionPoint firstIntersection) {
            firstIntersection = null;
            float nearestT = float.PositiveInfinity;
            RayIntersectionPoint currentIntersection;
            foreach (IObject geoObj in transformedObjects) {                
                if (geoObj.Intersect(ray, out currentIntersection)) {
                    if (currentIntersection.t < nearestT) {
                        nearestT = currentIntersection.t;
                        firstIntersection = currentIntersection;
                    }
                }
            }
            return firstIntersection != null;
        }

        public int Intersect(Ray ray, ref SortedList<float, RayIntersectionPoint> intersections) {            
            int numIntersections = 0;
            foreach (IObject geoObj in transformedObjects) {
                numIntersections += geoObj.Intersect(ray, ref intersections);
            }
            return numIntersections;
        }

    }
}
