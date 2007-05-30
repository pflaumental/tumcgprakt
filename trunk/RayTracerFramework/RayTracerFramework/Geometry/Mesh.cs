using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracerFramework.Geometry {
    
    
    class Mesh : IGeometricObject {
        protected List<MeshSubset> subsets;
        protected BSphere boundingSphere;

        protected Matrix transform;
        protected Matrix invTransform;

        protected Mesh() {
            subsets = new List<MeshSubset>();
            transform = Matrix.Identity;
            invTransform = Matrix.Identity;
            boundingSphere = new BSphere(Vec3.Zero, 0f);
        }

        protected Mesh(
                Matrix transform, 
                Matrix invTransform, 
                List<MeshSubset> subsets, 
                BSphere boundingSphere) {
            this.subsets = subsets;
            this.transform = transform;
            this.invTransform = invTransform;
            this.boundingSphere = boundingSphere;
        }

        public void AddSubset(MeshSubset subset) {
            subsets.Add(subset);
            boundingSphere = new BSphere(Vec3.Zero, 0f); // TODO: unhack this
        }

        public BSphere BoundingSphere {
            get {
                return boundingSphere;
            }
            set {
                boundingSphere = value;
            }
        }

        public void Transform(Matrix transformation) {
            this.transform *= transformation;
            this.invTransform = Matrix.Invert(this.transform);
            // UpdateBoundingShpere(); // TODO
        }

        public void Transform(Matrix transformation, Matrix invTransformation) {
            this.transform *= transformation;
            this.invTransform = invTransform * this.invTransform;
            // UpdateBoundingShpere(); // TODO
        }

        public bool Intersect(Ray ray) {
            // First test against bounding sphere
            if (!boundingSphere.Intersect(ray)) {
                return false;
            }

            Ray rayOS = ray.Transform(invTransform);

            foreach (MeshSubset subset in subsets) {
                foreach (Triangle triangle in subset.triangles) {
                    if (triangle.Intersect(rayOS))
                        return true;
                }
            }
            return false;
        }

        // Returns the firstintersection with the mesh. FirstIntersection references a triangle
        public bool Intersect(Ray ray, out RayIntersectionPoint firstIntersection) {
            Ray rayOS = ray.Transform(invTransform);
            
            // TODO: Boundingsphere-test in world-space
            // First test against bounding sphere
            if (!boundingSphere.Intersect(rayOS/*, out firstIntersection*/)) {
                firstIntersection = null; //                      ^
                return false; //                                  |
            } //                                                  |
            //    // Uncomment to see bounding sphere: -------------------------------------          
            //else {
            //    firstIntersection = new RayMeshIntersectionPoint(
            //            Vec3.TransformPosition3(firstIntersection.position, transform),
            //            Vec3.TransformNormal3n(firstIntersection.normal, transform),
            //            firstIntersection.t, this, this.subsets[0], 0.5f, 0.5f);
            //    return true;
            //}
            //// ---------------------------------------------------------------------------
            
            RayIntersectionPoint currentIntersection = null;
            firstIntersection = null;
            float currentT = float.PositiveInfinity;
            MeshSubset firstSubset = null;

            foreach (MeshSubset subset in subsets) {
                foreach (Triangle triangle in subset.triangles) {
                    if (triangle.Intersect(rayOS, out currentIntersection)) {
                        if (currentIntersection.t < currentT) {
                            currentT = currentIntersection.t;
                            firstIntersection = currentIntersection;
                            firstSubset = subset;
                        }
                    }
                }
            }
            if (firstIntersection == null)
                return false;

            firstIntersection = new RayMeshIntersectionPoint(
                    Vec3.TransformPosition3(firstIntersection.position, transform),
                    Vec3.TransformNormal3n(firstIntersection.normal, transform),
                    firstIntersection.t, this, firstSubset, 0.5f, 0.5f);
            return true;    
        }


        public int Intersect(Ray ray, ref SortedList<float, RayIntersectionPoint> intersections) {
            // First test against bounding sphere
            if (!boundingSphere.Intersect(ray)) {
                return 0;
            }

            Ray rayOS = ray.Transform(invTransform);
            RayIntersectionPoint intersection;
            int numIntersections = 0;

            foreach (MeshSubset subset in subsets) {
                foreach (Triangle triangle in subset.triangles) {
                    if (triangle.Intersect(rayOS, out intersection)) {
                        numIntersections++;
                        intersections.Add(intersection.t, new RayMeshIntersectionPoint(
                             Vec3.TransformPosition3(intersection.position, transform),
                             Vec3.TransformNormal3n(intersection.normal, transform),
                             intersection.t, this, subset, 0.5f, 0.5f));
                    }
                }
            }
            return numIntersections;     
        }
    }
}
