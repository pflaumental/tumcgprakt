using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracerFramework.Geometry {
    interface IIntersectable {
        bool Intersect(Ray ray);
        bool Intersect(Ray ray, out RayIntersectionPoint firstIntersection);
        int Intersect(Ray ray, out SortedList<float, RayIntersectionPoint> intersections);
    }
}
