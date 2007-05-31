using System;
using System.Collections.Generic;
using System.Text;
using RayTracerFramework.Utility;

namespace RayTracerFramework.Geometry {
    class TriangleKDTree : IIntersectable {
        public TriangleKDNode root;
        public List<Triangle> triangles;
        public int MaxTrianglesPerLeaf; // TODO: ???
        public int LeafesCount;

        public enum Axis { X=0, Y, Z }

        public TriangleKDTree() {
            triangles = new List<Triangle>();
            root = new TriangleKDLeaf(triangles);            
            MaxTrianglesPerLeaf = 50;
            LeafesCount = 1;
        }

        public TriangleKDTree(List<Triangle> triangles) {
            root = new TriangleKDLeaf(triangles);
            this.triangles = triangles;
            MaxTrianglesPerLeaf = 50;
            LeafesCount = 1;
        }

        public void Optimize() {
            LeafesCount = 1;
            root = Optimize(new TriangleKDLeaf(triangles), Axis.X);
        }

        private TriangleKDNode Optimize(TriangleKDLeaf leaf, TriangleKDTree.Axis splitAxis) {
            if (leaf.triangles.Count <= MaxTrianglesPerLeaf)
                return leaf;

            LeafesCount++;

            Triangle currentTriangle = leaf.triangles[0];

            Vec3 mid = (1f / 3f) * (currentTriangle.p1 + currentTriangle.p2 + currentTriangle.p3);
            for (int i = 1; i < leaf.triangles.Count; i++ ) {
                currentTriangle = leaf.triangles[i];
                mid =  (i / (i + 1f)) * mid + (1f / (i + 1f)) * (1f / 3f) * (currentTriangle.p1 + currentTriangle.p2 + currentTriangle.p3);
            }
            
            TriangleKDLeaf leftLeaf = new TriangleKDLeaf(SplitOnPlane(leaf.triangles, splitAxis, mid, true));
            TriangleKDLeaf rightLeaf = new TriangleKDLeaf(SplitOnPlane(leaf.triangles, splitAxis, mid, false));
            TriangleKDInner newNode = null;
            TriangleKDNode leftNode, rightNode;
            switch(splitAxis) {
                case Axis.X:
                    leftNode = Optimize(leftLeaf, Axis.Y);
                    rightNode = Optimize(rightLeaf, Axis.Y);
                    newNode = new TriangleKDInner(leftNode, rightNode, splitAxis, mid.x);
                    break;
                case Axis.Y:
                    leftNode = Optimize(leftLeaf, Axis.Z);
                    rightNode = Optimize(rightLeaf, Axis.Z);
                    newNode = new TriangleKDInner(leftNode, rightNode, splitAxis, mid.y);
                    break;
                case Axis.Z:
                    leftNode = Optimize(leftLeaf, Axis.X);
                    rightNode = Optimize(rightLeaf, Axis.X);
                    newNode = new TriangleKDInner(leftNode, rightNode, splitAxis, mid.z);
                    break;
            }
            return newNode;
        }

        private List<Triangle> SplitOnPlane(List<Triangle> triangles, Axis axis, Vec3 position, bool lower) { 
            List<Triangle> result = new List<Triangle>();
            switch (axis) { 
                case Axis.X:
                    if (lower) {
                        foreach (Triangle triangle in triangles) {
                            if (triangle.p1.x <= position.x || triangle.p2.x <= position.x || triangle.p3.x <= position.x)
                                result.Add(triangle);
                        }
                    } else {
                        foreach (Triangle triangle in triangles) {
                            if (triangle.p1.x > position.x || triangle.p2.x > position.x || triangle.p3.x > position.x)
                                result.Add(triangle);
                        }
                    }
                    break;
                case Axis.Y:
                    if (lower) {
                        foreach (Triangle triangle in triangles) {
                            if (triangle.p1.y <= position.y || triangle.p2.y <= position.y || triangle.p3.y <= position.y)
                                result.Add(triangle);
                        }
                    }
                    else {
                        foreach (Triangle triangle in triangles) {
                            if (triangle.p1.y > position.y || triangle.p2.y > position.y || triangle.p3.y > position.y)
                                result.Add(triangle);
                        }
                    }
                    break;
                case Axis.Z:
                    if (lower) {
                        foreach (Triangle triangle in triangles) {
                            if (triangle.p1.z <= position.z || triangle.p2.z <= position.z || triangle.p3.z <= position.z)
                                result.Add(triangle);
                        }
                    }
                    else {
                        foreach (Triangle triangle in triangles) {
                            if (triangle.p1.z > position.z || triangle.p2.z > position.z || triangle.p3.z > position.z)
                                result.Add(triangle);
                        }
                    }
                    break;
            }
            return result;
        }

        public bool Intersect(Ray ray) {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool Intersect(Ray ray, out RayIntersectionPoint firstIntersection) {
            return Traverse(ray, root, 0f, float.PositiveInfinity, out firstIntersection);
        }

        public int Intersect(Ray ray, ref SortedList<float, RayIntersectionPoint> intersections) {
            throw new Exception("The method or operation is not implemented and never will be!.");
        }

        private bool Traverse(Ray ray, TriangleKDNode node, float tMin, float tMax, out RayIntersectionPoint firstIntersection) {
            if (node.isLeaf) {
                TriangleKDLeaf leaf = (TriangleKDLeaf)node;

                RayIntersectionPoint currentIntersection = null;
                firstIntersection = null;
                float currentT = float.PositiveInfinity;

                foreach (Triangle triangle in leaf.triangles) {
                    if (triangle.Intersect(ray, out currentIntersection)) {
                        if (currentIntersection.t < currentT) {
                            currentT = currentIntersection.t;
                            firstIntersection = currentIntersection;
                        }
                    }
                }
                if (currentT > tMax)
                    firstIntersection = null;

                return firstIntersection != null;
            }

            TriangleKDInner inner = (TriangleKDInner)node;

            TriangleKDNode near = null, far = null;
            float tSplit = 0f;
            switch (inner.axis) { 
                case Axis.X:
                    if (ray.position.x > inner.planePosition) {
                        near = inner.right;
                        far = inner.left;
                    }
                    else {
                        near = inner.left;
                        far = inner.right;
                    }
                    if (ray.direction.x == 0f)
                        tSplit = float.PositiveInfinity;
                    else
                        tSplit = (inner.planePosition - ray.position.x) / ray.direction.x;
                    break;
                case Axis.Y:
                    if (ray.position.y > inner.planePosition) {
                        near = inner.right;
                        far = inner.left;
                    }
                    else {
                        near = inner.left;
                        far = inner.right;
                    }
                    if (ray.direction.y == 0f)
                        tSplit = float.PositiveInfinity;
                    else
                        tSplit = (inner.planePosition - ray.position.y) / ray.direction.y;
                    break;
                case Axis.Z:
                    if (ray.position.z > inner.planePosition) {
                        near = inner.right;
                        far = inner.left;
                    }
                    else {
                        near = inner.left;
                        far = inner.right;
                    }
                    if (ray.direction.z == 0f)
                        tSplit = float.PositiveInfinity;
                    else
                        tSplit = (inner.planePosition - ray.position.z) / ray.direction.z;
                    break;
            }

            if ((tSplit >= tMax) || (tSplit < 0f))
                return Traverse(ray, near, tMin, tMax, out firstIntersection);
            else if (tSplit < tMin)
                return Traverse(ray, far, tMin, tMax, out firstIntersection);
            else {
                if (Traverse(ray, near, tMin, tSplit, out firstIntersection))
                    if (firstIntersection.t < tSplit)
                        return true;                    
                return Traverse(ray, far, tSplit, tMax, out firstIntersection);
            }
        }
    }

    abstract class TriangleKDNode {
        protected TriangleKDNode(bool isLeaf) {
            this.isLeaf = isLeaf;
        }
        public readonly bool isLeaf;
    }

    class TriangleKDLeaf : TriangleKDNode {
        public List<Triangle> triangles;

        public TriangleKDLeaf() : base(true) {
            triangles = new List<Triangle>();
        }

        public TriangleKDLeaf(List<Triangle> triangles) : base(true) {
            this.triangles = triangles;
        }
    }

    class TriangleKDInner : TriangleKDNode {
        public TriangleKDNode left, right;
        public TriangleKDTree.Axis axis;
        public float planePosition;

        public TriangleKDInner(TriangleKDNode left, TriangleKDNode right, TriangleKDTree.Axis axis, float planePosition) : base(false) {
            this.left = left;
            this.right = right;
            this.axis = axis;
            this.planePosition = planePosition;
        }
    }
   

}
