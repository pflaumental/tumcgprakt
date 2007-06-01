using System;
using System.Collections.Generic;
using System.Text;
using RayTracerFramework.Utility;

namespace RayTracerFramework.Geometry {
    class TriangleKDTree : IIntersectable {
        public TriangleKDNode root;
        public List<Triangle> triangles;
        public int MaxDesiredTrianglesPerLeafCount;
        public int MaxHeight;        
        public int Height;
        
        // For test only :
        //public int xSplitCount = 0;
        //public int ySplitCount = 0;
        //public int zSplitCount = 0;
        //public int leafesTriangleSum = 0;
        //public int LeafesCount;
        //public int GreatestTrianglePerLeafCount = 0;

        private float weightDivisionQuality;
        public float WeightDivisionQuality {
            get { return weightDivisionQuality; }
            set {
                if (value < 0 || value > 1)
                    throw new Exception("WeightDivisionQuality must be between 0 and 1");
                weightDivisionQuality = value;
                weightTriangleSum = 1 - value;
            }
        }

        private float weightTriangleSum;
        public float WeightTriangleSum {
            get { return weightTriangleSum; }
            set {
                if (value < 0 || value > 1)
                    throw new Exception("WeightBadTriangleCount must be between 0 and 1");
                weightTriangleSum = value;
                weightDivisionQuality = 1 - value;
            }
        }

        private readonly static int DefaultMaxHeight = 25;
        private readonly static int DefaultMaxDesiredTrianglesPerLeafCount = 12;
        private readonly static float DefaultWeightDivisionQuality = 0f;
        private readonly static float DefaultWeightBadTriangleCount = 1f - DefaultWeightDivisionQuality;

        public enum Axis { X=0, Y, Z }

        public TriangleKDTree() {
            triangles = new List<Triangle>();
            root = new TriangleKDLeaf(triangles);
            MaxDesiredTrianglesPerLeafCount = DefaultMaxDesiredTrianglesPerLeafCount;
            MaxHeight = DefaultMaxHeight;
            WeightTriangleSum = DefaultWeightBadTriangleCount;
            WeightDivisionQuality = DefaultWeightDivisionQuality;
            Height = 1;
            //LeafesCount = 1;
        }

        public TriangleKDTree(List<Triangle> triangles) {
            root = new TriangleKDLeaf(triangles);
            this.triangles = triangles;
            MaxDesiredTrianglesPerLeafCount = DefaultMaxDesiredTrianglesPerLeafCount;
            MaxHeight = DefaultMaxHeight;
            WeightTriangleSum = DefaultWeightBadTriangleCount;
            WeightDivisionQuality = DefaultWeightDivisionQuality;
            Height = 1;
            //LeafesCount = 1;
        }

        public void Optimize() {
            //LeafesCount = 1;
            root = Optimize(new TriangleKDLeaf(triangles), 1);
        }

        private TriangleKDNode Optimize(TriangleKDLeaf leaf, int currentHeight) {
            if (currentHeight > Height)
                Height = currentHeight;

            // Stop here if max leaf count is already satisfied or tree gets too high
            if (leaf.triangles.Count <= MaxDesiredTrianglesPerLeafCount || currentHeight++ == MaxHeight) {
                //if (GreatestTrianglePerLeafCount < leaf.triangles.Count)
                //    GreatestTrianglePerLeafCount = leaf.triangles.Count;
                return leaf;
            }

            // Find mid of all triangles (will be used as splitting position)
            Triangle currentTriangle = leaf.triangles[0];
            Vec3 mid = (1f / 3f) * (currentTriangle.p1 + currentTriangle.p2 + currentTriangle.p3);
            for (int i = 1; i < leaf.triangles.Count; i++ ) {
                currentTriangle = leaf.triangles[i];
                mid =  (i / (i + 1f)) * mid + (1f / (i + 1f)) * (1f / 3f) * (currentTriangle.p1 + currentTriangle.p2 + currentTriangle.p3);
            }

            // Choose best splitting axis
            // This should be the one wich divides the triangles best
            // and does have the least triangles in both sides
            Axis splitAxis = Axis.X;
            float planePosition = mid.x;
            
            List<Triangle> leftTriangles, rightTriangles;
            SplitOnPlane(leaf.triangles, Axis.X, mid, out leftTriangles, out rightTriangles);

            float currentDivisionQuality = leftTriangles.Count > rightTriangles.Count
                    ? ((float)rightTriangles.Count) / leftTriangles.Count
                    : ((float)leftTriangles.Count) / rightTriangles.Count;
            float currentTriangleSum = leftTriangles.Count + rightTriangles.Count;
            
            List<Triangle> alternativeLeftTriangles, alternativeRightTriangles;                        
            SplitOnPlane(leaf.triangles, Axis.Y, mid, out alternativeLeftTriangles, out alternativeRightTriangles);

            float alternativeDivisionQuality = alternativeLeftTriangles.Count > alternativeRightTriangles.Count
                    ? ((float)alternativeRightTriangles.Count) / alternativeLeftTriangles.Count
                    : ((float)alternativeLeftTriangles.Count) / alternativeRightTriangles.Count;
            float alternativeTriangleSum = alternativeLeftTriangles.Count + alternativeRightTriangles.Count;

            if ((WeightDivisionQuality * alternativeDivisionQuality / currentDivisionQuality +
                    WeightTriangleSum * currentTriangleSum / alternativeTriangleSum) > 1f) {
                leftTriangles = alternativeLeftTriangles;
                rightTriangles = alternativeRightTriangles;
                splitAxis = Axis.Y;
                planePosition = mid.y;
                currentDivisionQuality = alternativeDivisionQuality;
                currentTriangleSum = alternativeTriangleSum;
            }

            SplitOnPlane(leaf.triangles, Axis.Z, mid, out alternativeLeftTriangles, out alternativeRightTriangles);
            
            alternativeDivisionQuality = alternativeLeftTriangles.Count > alternativeRightTriangles.Count
                    ? ((float)alternativeRightTriangles.Count) / alternativeLeftTriangles.Count
                    : ((float)alternativeLeftTriangles.Count) / alternativeRightTriangles.Count;
            alternativeTriangleSum = alternativeLeftTriangles.Count + alternativeRightTriangles.Count;

            if ((WeightDivisionQuality * alternativeDivisionQuality / currentDivisionQuality +
                    WeightTriangleSum * currentTriangleSum / alternativeTriangleSum) > 1f) {
                leftTriangles = alternativeLeftTriangles;
                rightTriangles = alternativeRightTriangles;
                splitAxis = Axis.Z;
                planePosition = mid.z;
            }            

            // Stop here if triangle count could not be lowered anymore
            if (leftTriangles.Count == leaf.triangles.Count || rightTriangles.Count == leaf.triangles.Count) {
                //if (GreatestTrianglePerLeafCount < leaf.triangles.Count)
                //    GreatestTrianglePerLeafCount = leaf.triangles.Count;
                return leaf;
            }

            // For test only:
            //leafesTriangleSum += (int)currentTriangleSum;
            //switch (splitAxis) {
            //    case Axis.X:
            //        xSplitCount++;
            //        break;
            //    case Axis.Y:
            //        ySplitCount++;
            //        break;
            //    case Axis.Z:
            //        zSplitCount++;
            //        break;
            //}

            //LeafesCount++;

            // Recursivly optimize left side
            TriangleKDNode leftNode = Optimize(new TriangleKDLeaf(leftTriangles), currentHeight);
            // Recursivly optimize right side
            TriangleKDNode rightNode = Optimize(new TriangleKDLeaf(rightTriangles), currentHeight);

            // Create and return new inner node                        
            return new TriangleKDInner(leftNode, rightNode, splitAxis, planePosition);
        }

        private void SplitOnPlane(
                List<Triangle> triangles, 
                Axis axis, 
                Vec3 position, 
                out List<Triangle> leftTriangles, 
                out List<Triangle> rightTriangles) { 

            leftTriangles = new List<Triangle>();
            rightTriangles = new List<Triangle>();

            switch (axis) { 
                case Axis.X:
                    foreach (Triangle triangle in triangles) {
                        if (triangle.p1.x <= position.x || triangle.p2.x <= position.x || triangle.p3.x <= position.x)
                            leftTriangles.Add(triangle);
                        if (triangle.p1.x > position.x || triangle.p2.x > position.x || triangle.p3.x > position.x)
                            rightTriangles.Add(triangle);
                    }
                    break;
                case Axis.Y:
                    foreach (Triangle triangle in triangles) {
                        if (triangle.p1.y <= position.y || triangle.p2.y <= position.y || triangle.p3.y <= position.y)
                            leftTriangles.Add(triangle);
                        if (triangle.p1.y > position.y || triangle.p2.y > position.y || triangle.p3.y > position.y)
                            rightTriangles.Add(triangle);
                    }
                    break;
                case Axis.Z:
                    foreach (Triangle triangle in triangles) {
                        if (triangle.p1.z <= position.z || triangle.p2.z <= position.z || triangle.p3.z <= position.z)
                            leftTriangles.Add(triangle);
                        if (triangle.p1.z > position.z || triangle.p2.z > position.z || triangle.p3.z > position.z)
                            rightTriangles.Add(triangle);
                    }
                    break;
            }
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
