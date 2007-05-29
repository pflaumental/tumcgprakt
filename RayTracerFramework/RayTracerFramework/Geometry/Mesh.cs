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


        public void AddMaterialGroup(MaterialGroup materialGroup) {
            materialGroups.Add(materialGroup);
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
                foreach (Triangle triangle in mg.triangles) {
                    if (triangle.Intersect(rayOS))
                        return true;
                }
            }
            return false;
        }

        // Returns the firstintersection with the mesh. FirstIntersection references a triangle
        public bool Intersect(Ray ray, out RayIntersectionPoint firstIntersection) {
            Ray rayOS = ray.Transform(transformInv);
            RayIntersectionPoint intersection;
            
            foreach (MaterialGroup mg in materialGroups) {
                foreach (Triangle triangle in mg.triangles) {
                    if (triangle.Intersect(rayOS, out intersection)) {
                        firstIntersection = new RayIntersectionPointTriangle(
                            Vec3.TransformPosition3(intersection.position, transform),
                            Vec3.TransformNormal3n(intersection.normal, transform), 
                            intersection.t, this, (Triangle)intersection.hitObject, 0.5f, 0.5f);
                        return true;
                    }
                }
            }
            firstIntersection = null;
            return false;    
        }


        public int Intersect(Ray ray, ref SortedList<float, RayIntersectionPoint> intersections) {
            return 0;
            //Ray rayOS = ray.Transform(transformInv);
            //RayIntersectionPoint intersection;
            //int numIntersections = 0;

            //foreach (MaterialGroup mg in materialGroups) {
            //    foreach (Triangle triangle in mg.triangles) {
            //        if (triangle.Intersect(rayOS, out intersection)) {
            //            numIntersections++;
            //            intersections.Add(intersection.t, new RayIntersectionPointTriangle(
            //                 Vec3.TransformPosition3(intersection.position, transform),
            //                 Vec3.TransformNormal3n(intersection.normal, transform),
            //                 intersection.t, this, (Triangle)intersection.hitObject, 0.5f, 0.5f));
            //        }
            //    }
            //}
            //return numIntersections;     
        }
    }
}
