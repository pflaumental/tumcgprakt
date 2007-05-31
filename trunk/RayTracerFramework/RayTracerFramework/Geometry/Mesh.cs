using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracerFramework.Geometry {
    
    
    class Mesh : IGeometricObject {
        protected List<MeshSubset> subsets;
        protected BSphere boundingSphere;

        public List<Vec3> vertices;
        public List<Vec3> normals;

        protected Mesh() {
            subsets = new List<MeshSubset>();
            boundingSphere = new BSphere(Vec3.Zero, 0f);
            vertices = new List<Vec3>();
            normals = new List<Vec3>();
        }

        protected Mesh(List<MeshSubset> subsets, BSphere boundingSphere,
                       List<Vec3> vertices, List<Vec3> normals) {
            this.subsets = subsets;
            this.boundingSphere = boundingSphere;
            this.vertices = vertices;
            this.normals = normals;
            Setup();
        }

        public void AddSubset(MeshSubset subset) {
            subsets.Add(subset);
            boundingSphere = new BSphere(Vec3.Zero, 0f);            
        }

        public void Setup() {
            // Update bounding sphere
            Vec3 center = vertices[0];
            for (int i = 1; i < vertices.Count; i++) {
                float f = 1f / i;
                center = (1f - f) * center + f * vertices[i];
            }
            float radiusSq = 0f, distSq = 0f;
            foreach (Vec3 vertex in vertices) {
                distSq = Vec3.GetLengthSq(center - vertex);
                if (distSq > radiusSq)
                    radiusSq = distSq;
            }
            this.boundingSphere = new BSphere(center, (float)Math.Sqrt(radiusSq), radiusSq);

            // Optimize kd-trees
            foreach (MeshSubset subset in subsets) {
                subset.kdTree.Optimize();
            }
        }

        public BSphere BoundingSphere {
            get {
                return boundingSphere;
            }
        }

        public void Transform(Matrix transformation) {            
            for (int i = 0; i < vertices.Count; i++) {
                Vec3 v = Vec3.TransformPosition3(vertices[i], transformation);
                vertices[i].x = v.x;
                vertices[i].y = v.y;
                vertices[i].z = v.z;
                Vec3 n = Vec3.TransformNormal3n(normals[i], transformation);
                normals[i].x = n.x;
                normals[i].y = n.y;
                normals[i].z = n.z;
            }
            Setup();
        }

        public void Transform(Matrix transformation, Matrix invTransformation) {
            Matrix transformationNormal = Matrix.Transpose(invTransformation);
            for (int i = 0; i < vertices.Count; i++) {
                Vec3 v = Vec3.TransformPosition3(vertices[i], transformation);
                vertices[i].x = v.x;
                vertices[i].y = v.y;
                vertices[i].z = v.z;
                Vec3 n = Vec3.Normalize(Vec3.TransformPosition3(normals[i], transformationNormal));
                normals[i].x = n.x;
                normals[i].y = n.y;
                normals[i].z = n.z;
            }
            Setup();
        }

        public bool Intersect(Ray ray) {
            // First test against bounding sphere
            if (!boundingSphere.Intersect(ray)) {
                return false;
            }

            foreach (MeshSubset subset in subsets) {
                if (subset.kdTree.Intersect(ray))
                    return true;
            }

            return false;
        }

        // Returns the firstintersection with the mesh. FirstIntersection references a triangle
        public bool Intersect(Ray ray, out RayIntersectionPoint firstIntersection) {
            // First test against bounding sphere
            if (!boundingSphere.Intersect(ray/*, out firstIntersection*/)) {
                firstIntersection = null; //                      ^
                return false; //                                  |
            } //                                                  |
            //// Uncomment to see bounding sphere: -------------------------------------          
            //else {
            //    firstIntersection = new RayMeshIntersectionPoint(
            //            firstIntersection.position,
            //            firstIntersection.normal,
            //            firstIntersection.t, this, this.subsets[0], 0.5f, 0.5f);
            //    return true;
            //}
            //// ---------------------------------------------------------------------------
            
            RayIntersectionPoint currentIntersection = null;
            firstIntersection = null;
            float currentT = float.PositiveInfinity;
            MeshSubset firstSubset = null;

            foreach (MeshSubset subset in subsets) {
                if(subset.kdTree.Intersect(ray, out currentIntersection)) {
                    if (currentIntersection.t < currentT) {
                        currentT = currentIntersection.t;
                        firstIntersection = currentIntersection;
                        firstSubset = subset;
                    }
                }                            
            }

            if (firstIntersection == null)
                return false;

            firstIntersection = new RayMeshIntersectionPoint(firstIntersection.position,
                firstIntersection.normal, firstIntersection.t, this, firstSubset, 0.5f, 0.5f);
            return true;    
        }


        public int Intersect(Ray ray, ref SortedList<float, RayIntersectionPoint> intersections) {
            // First test against bounding sphere
            if (!boundingSphere.Intersect(ray)) {
                return 0;
            }

            int numIntersections = 0;
            SortedList<float, RayIntersectionPoint> subsetIntersections = new SortedList<float, RayIntersectionPoint>();

            foreach (MeshSubset subset in subsets) {
                numIntersections += subset.kdTree.Intersect(ray, ref subsetIntersections);
                foreach (RayIntersectionPoint intersectionPoint in subsetIntersections.Values) {
                    intersections.Add(intersectionPoint.t, new RayMeshIntersectionPoint(
                            intersectionPoint.position, intersectionPoint.normal,
                            intersectionPoint.t, this, subset, 0.5f, 0.5f));
                }
            }
            return numIntersections;     
        }
    }
}
