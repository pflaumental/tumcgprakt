using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracerFramework.Geometry {
    interface IGeometricObject : IIntersectable {
        void Transform(Matrix transformation);
        void Transform(Matrix transformation, Matrix invTransformation);
        void Setup();
    }
}
