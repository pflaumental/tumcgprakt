using System;
using System.Collections.Generic;
using System.Text;
using RayTracerFramework.Geometry;

namespace RayTracerFramework.Shading {
    
    class LightManager {
        // public List<Light> lights;
        public Matrix viewMatrix;
        private List<Light> lightsViewSpace;

        public LightManager(Matrix viewMatrix) {
            this.viewMatrix = viewMatrix;
            this.lightsViewSpace = new List<Light>();
        }

        public void AddWorldSpaceLight(Light lightWorldSpace) {
            Light lightViewSpace;
            
            switch(lightWorldSpace.lightType) {
                case LightType.Point:
                    lightViewSpace = new PointLight(Vec3.TransformPosition3(
                                                    ((PointLight)lightWorldSpace).position, 
                                                    viewMatrix));
                    lightViewSpace.ambient = lightWorldSpace.ambient;
                    lightViewSpace.diffuse = lightWorldSpace.diffuse;
                    lightViewSpace.specular = lightWorldSpace.specular;
                    break;
                default:
                    lightViewSpace = new PointLight(Vec3.Zero);
                    break;
            }

            this.lightsViewSpace.Add(lightViewSpace);
        }

        public List<Light> LightsViewSpace {
            get { return lightsViewSpace; }
        }
    }
}
