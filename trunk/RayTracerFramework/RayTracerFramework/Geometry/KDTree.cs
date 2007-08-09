using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracerFramework.Geometry {

    abstract public class KDTree : IIntersectable {

        // Related assignement: 5.3

        // Variables
        public List<IIntersectable> content;
        public KDTree.Node root;
        public int MaxDesiredObjectsPerLeafCount;
        public int MaxHeight;
        public int Height;
        //// For test only :
        //public int xSplitCount = 0;
        //public int ySplitCount = 0;
        //public int zSplitCount = 0;
        //public int leafesObjectSum = 0;
        //public int LeafesCount;
        //public int GreatestObjectsPerLeafCount = 0;


        // Properties
        private float weightDivisionQuality;
        public float WeightDivisionQuality {
            get { return weightDivisionQuality; }
            set {
                if (value < 0 || value > 1)
                    throw new Exception("WeightDivisionQuality must be between 0 and 1");
                weightDivisionQuality = value;
                weightSum = 1 - value;
            }
        }
        private float weightSum;
        public float WeightSum {
            get { return weightSum; }
            set {
                if (value < 0 || value > 1)
                    throw new Exception("WeightSum must be between 0 and 1");
                weightSum = value;
                weightDivisionQuality = 1 - value;
            }
        }

        // Enums
        public enum Axis { X = 0, Y, Z }

        // Inner public classes
        public abstract class Node {
            public readonly bool isLeaf;

            protected Node(bool isLeaf) {
                this.isLeaf = isLeaf;
            }
        }

        public class Leaf : KDTree.Node {
            public List<IIntersectable> content;

            public Leaf()
                : base(true) {
                content = new List<IIntersectable>();
            }

            public Leaf(List<IIntersectable> content)
                : base(true) {
                this.content = content;
            }
        }

        public class Inner : KDTree.Node {
            public KDTree.Node left, right;
            public KDTree.Axis axis;
            public float planePosition;

            public Inner(KDTree.Node left, KDTree.Node right, KDTree.Axis axis, float planePosition)
                : base(false) {
                this.left = left;
                this.right = right;
                this.axis = axis;
                this.planePosition = planePosition;
            }
        }

        // Constructors
        public KDTree() {
            content = new List<IIntersectable>();
            root = new KDTree.Leaf(content);
            MaxDesiredObjectsPerLeafCount = Settings.Setup.KDTree.DefaultMaxDesiredObjectsPerLeafCount;
            MaxHeight = Settings.Setup.KDTree.DefaultMaxHeight;
            WeightSum = Settings.Setup.KDTree.DefaultWeightSum;
            WeightDivisionQuality = Settings.Setup.KDTree.DefaultWeightDivisionQuality;
            Height = 1;
            //LeafesCount = 1;
        }

        public KDTree(List<IIntersectable> content) {
            root = new KDTree.Leaf(content);
            this.content = content;
            MaxDesiredObjectsPerLeafCount = Settings.Setup.KDTree.DefaultMaxDesiredObjectsPerLeafCount;
            MaxHeight = Settings.Setup.KDTree.DefaultMaxHeight;
            WeightSum = Settings.Setup.KDTree.DefaultWeightSum;
            WeightDivisionQuality = Settings.Setup.KDTree.DefaultWeightDivisionQuality;
            Height = 1;
            //LeafesCount = 1;
        }

        // Abstract methods
        protected abstract void SplitOnPlane(
                List<IIntersectable> splitContent,
                Axis axis,
                Vec3 position,
                out List<IIntersectable> leftContent,
                out List<IIntersectable> rightContent);

        protected abstract Vec3 CalculateMid(List<IIntersectable> content);

        // Methods
        public void Optimize() {
            //LeafesCount = 1;
            root = Optimize(new KDTree.Leaf(content), 1);
        }

        protected KDTree.Node Optimize(KDTree.Leaf leaf, int currentHeight) {
            if (currentHeight > Height)
                Height = currentHeight;

            // Stop here if max leaf count is already satisfied or tree gets too high
            if (leaf.content.Count <= MaxDesiredObjectsPerLeafCount
                    || currentHeight++ == MaxHeight) {
                //// For test only:
                //if (GreatestObjectsPerLeafCount < leaf.content.Count)
                //    GreatestObjectsPerLeafCount = leaf.content.Count;
                return leaf;
            }

            // Find mid of all content (will be used as splitting position)
            Vec3 mid = CalculateMid(leaf.content);

            // Choose best splitting axis
            // This should be the one wich divides the content best
            // and does have the least content in both sides
            Axis splitAxis = Axis.X;
            float planePosition = mid.x;

            List<IIntersectable> leftContent, rightContent;
            SplitOnPlane(leaf.content, Axis.X, mid, out leftContent, out rightContent);

            float currentDivisionQuality = leftContent.Count > rightContent.Count
                    ? ((float)rightContent.Count) / leftContent.Count
                    : ((float)leftContent.Count) / rightContent.Count;
            float currentSum = leftContent.Count + rightContent.Count;

            List<IIntersectable> alternativeLeftContent, alternativeRightContent;
            SplitOnPlane(leaf.content, Axis.Y, mid, out alternativeLeftContent, out alternativeRightContent);

            float alternativeDivisionQuality = alternativeLeftContent.Count > alternativeRightContent.Count
                    ? ((float)alternativeRightContent.Count) / alternativeLeftContent.Count
                    : ((float)alternativeLeftContent.Count) / alternativeRightContent.Count;
            float alternativeSum = alternativeLeftContent.Count + alternativeRightContent.Count;

            if ((WeightDivisionQuality * alternativeDivisionQuality / currentDivisionQuality +
                    WeightSum * currentSum / alternativeSum) > 1f) {
                leftContent = alternativeLeftContent;
                rightContent = alternativeRightContent;
                splitAxis = Axis.Y;
                planePosition = mid.y;
                currentDivisionQuality = alternativeDivisionQuality;
                currentSum = alternativeSum;
            }

            SplitOnPlane(leaf.content, Axis.Z, mid, out alternativeLeftContent, out alternativeRightContent);

            alternativeDivisionQuality = alternativeLeftContent.Count > alternativeRightContent.Count
                    ? ((float)alternativeRightContent.Count) / alternativeLeftContent.Count
                    : ((float)alternativeLeftContent.Count) / alternativeRightContent.Count;
            alternativeSum = alternativeLeftContent.Count + alternativeRightContent.Count;

            if ((WeightDivisionQuality * alternativeDivisionQuality / currentDivisionQuality +
                    WeightSum * currentSum / alternativeSum) > 1f) {
                leftContent = alternativeLeftContent;
                rightContent = alternativeRightContent;
                splitAxis = Axis.Z;
                planePosition = mid.z;
            }

            // Stop here if obj count could not be lowered anymore
            if (leftContent.Count == leaf.content.Count || rightContent.Count == leaf.content.Count) {
                //// For test only:
                //if (GreatestObjectsPerLeafCount < leaf.content.Count)
                //    GreatestObjectsPerLeafCount = leaf.content.Count;
                return leaf;
            }

            //// For test only:
            //leafesObjectSum += (int)currentSum;
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
            KDTree.Node leftNode = Optimize(new KDTree.Leaf(leftContent), currentHeight);
            // Recursivly optimize right side
            KDTree.Node rightNode = Optimize(new KDTree.Leaf(rightContent), currentHeight);

            // Create and return new inner node                        
            return new KDTree.Inner(leftNode, rightNode, splitAxis, planePosition);
        }

        protected bool Traverse(Ray ray, KDTree.Node node, float tMin, float tMax, out RayIntersectionPoint firstIntersection) {
            if (node.isLeaf) {
                KDTree.Leaf leaf = (KDTree.Leaf)node;

                RayIntersectionPoint currentIntersection = null;
                firstIntersection = null;
                float currentT = float.PositiveInfinity;

                foreach (IIntersectable obj in leaf.content) {
                    if (obj.Intersect(ray, out currentIntersection)) {
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

            KDTree.Inner inner = (KDTree.Inner)node;

            KDTree.Node near, far;
            float tSplit;
            CalculateInner(ray, inner, out near, out far, out tSplit);

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

        protected int Traverse(Ray ray, KDTree.Node node, float tMin, float tMax, ref SortedList<float, RayIntersectionPoint> intersections) {
            if (node.isLeaf) {
                KDTree.Leaf leaf = (KDTree.Leaf)node;

                int numCurrentIntersections = 0;

                foreach (IIntersectable obj in leaf.content) {
                    numCurrentIntersections += obj.Intersect(ray, ref intersections);
                }

                return numCurrentIntersections;
            }

            KDTree.Inner inner = (KDTree.Inner)node;

            KDTree.Node near, far;
            float tSplit;
            CalculateInner(ray, inner, out near, out far, out tSplit);

            if ((tSplit >= tMax) || (tSplit < 0f))
                return Traverse(ray, near, tMin, tMax, ref intersections);
            else if (tSplit < tMin)
                return Traverse(ray, far, tMin, tMax, ref intersections);
            else {
                int numCurrentIntersections = Traverse(ray, near, tMin, tSplit, ref intersections);
                return numCurrentIntersections + Traverse(ray, far, tSplit, tMax, ref intersections);
            }
        }

        protected void CalculateInner(
                Ray ray,
                KDTree.Inner inner,
                out KDTree.Node near,
                out KDTree.Node far,
                out float tSplit) {
            
            near = null;
            far = null;
            tSplit = 0f;

            switch (inner.axis) {
                case Axis.X:
                    if (ray.position.x > inner.planePosition) {
                        near = inner.right;
                        far = inner.left;
                    } else {
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
                    } else {
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
                    } else {
                        near = inner.left;
                        far = inner.right;
                    }
                    if (ray.direction.z == 0f)
                        tSplit = float.PositiveInfinity;
                    else
                        tSplit = (inner.planePosition - ray.position.z) / ray.direction.z;
                    break;
            }
        }

        public bool Intersect(Ray ray) {
            RayIntersectionPoint dummy;
            return Traverse(ray, root, 0f, float.PositiveInfinity, out dummy);
        }

        public bool Intersect(Ray ray, out RayIntersectionPoint firstIntersection) {
            return Traverse(ray, root, 0f, float.PositiveInfinity, out firstIntersection);
        }

        public int Intersect(Ray ray, ref SortedList<float, RayIntersectionPoint> intersections) {
            return Traverse(ray, root, 0f, float.PositiveInfinity, ref intersections);
        }

    } // End class

} // End namespace
