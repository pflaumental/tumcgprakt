using System;
using System.Collections.Generic;
using System.Text;
using RayTracerFramework.Shading;

namespace RayTracerFramework.Geometry {
    class GeometryManager {
        private List<Instance> instances; // (untransformedObject, worldMatrix)
        public List<IObject> TransformedObjects; // Transformed objects in view space
        public Matrix viewMatrix;

        // An instance is a tupel of an untransformed object and the related world matrix
        private class Instance {
            public readonly IObject untransformedObject;
            public readonly Matrix worldMatrix;

            public Instance(IObject untransformedObject, Matrix worldMatrix) {
                this.untransformedObject = untransformedObject;
                this.worldMatrix = worldMatrix;
            }
        }

        public GeometryManager() {
            instances = new List<Instance>();
            TransformedObjects = new List<IObject>();
        }

        public void AddInstance(IObject obj, Matrix worldMatrix) {
            instances.Add(new Instance(obj, worldMatrix));
        }

        public void TransformAll() {
            TransformedObjects.Clear();
            foreach (Instance instance in instances) {
                IObject instObj = instance.untransformedObject.Clone();
                instObj.Transform(instance.worldMatrix * viewMatrix);
                TransformedObjects.Add(instObj);                    
            }
        }
    }
}
