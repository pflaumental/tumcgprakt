using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracerFramework.Geometry {
    interface IIntersectable {
        bool Intersect(Ray ray);
        bool Intersect(Ray ray, out IntersectionPoint firstIntersection, out float t);
        int Intersect(Ray ray, out IntersectionPoint[] intersections, out float t1, out float t2);
    }
}
