using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracerFramework.Geometry {
    public interface IGeometricObject : IIntersectable, IBounding {
        void Transform(Matrix transformation);
        void Transform(Matrix transformation, Matrix invTransformation);
    }
}
