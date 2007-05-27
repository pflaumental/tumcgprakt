using System;
using System.Collections.Generic;
using System.Text;
using RayTracerFramework.Shading;

namespace RayTracerFramework.Geometry {
    
    
    class Mesh : IGeometricObject {
        protected List<MaterialGroup> materialGroups;

        protected Matrix transform;
        protected Matrix transformInv;

        protected Mesh() {
            materialGroups = new List<MaterialGroup>();
        }

        protected Mesh(Matrix transform, Matrix transformInv, List<MaterialGroup> materialGroups) {
            this.materialGroups = materialGroups;
            this.transform = transform;
            this.transformInv = transformInv;
        }


        public void Transform(Matrix transformation) {
            this.transform = transformation;
            this.transformInv = Matrix.Invert(transformation);
        }


        public void Transform(Matrix transformation, Matrix invTransformation) {
            this.transform = transformation;
            this.transformInv = invTransformation;
        }


        public bool Intersect(Ray ray) {
            Ray rayOS = ray.Transform(transformInv);

            foreach (MaterialGroup mg in materialGroups) {
                foreach (DTriangle triangle in mg.triangles) {
                    if (triangle.Intersect(rayOS))
                        return true;
                }
            }
            return false;
        }

        // Returns the firstintersection with the mesh. FirstIntersection references a triangle
        public bool Intersect(Ray ray, out RayIntersectionPoint firstIntersection) {
            Ray rayOS = ray.Transform(transformInv);

            foreach (MaterialGroup mg in materialGroups) {
                foreach (DTriangle triangle in mg.triangles) {
                    if (triangle.Intersect(rayOS, out firstIntersection)) {
                        return true;
                    }
                }
            }
            firstIntersection = null;
            return false;    
        }


        public int Intersect(Ray ray, out SortedList<float, RayIntersectionPoint> intersections) {
            Ray rayOS = ray.Transform(transformInv);
            RayIntersectionPoint intersectionTriangle;
            intersections = null;

            foreach (MaterialGroup mg in materialGroups) {
                foreach (DTriangle triangle in mg.triangles) {
                    if (triangle.Intersect(rayOS, out intersectionTriangle)) {
                        if (intersections == null)
                            intersections = new SortedList<float, RayIntersectionPoint>();
                        intersections.Add(intersectionTriangle.t, intersectionTriangle);
                    }
                }
            }
            return intersections == null ? 0 : intersections.Count;     
        }
    }
}
