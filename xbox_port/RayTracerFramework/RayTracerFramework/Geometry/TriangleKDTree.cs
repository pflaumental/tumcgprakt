using System;
using System.Collections.Generic;
using System.Text;
using RayTracerFramework.Utility;

namespace RayTracerFramework.Geometry {
    class TriangleKDTree : KDTree {        

        public TriangleKDTree() : base() {
        }

        public TriangleKDTree(List<IIntersectable> content) : base(content) {
        }

        protected override Vec3 CalculateMid(List<IIntersectable> content) {
            Triangle currentTriangle = (Triangle)content[0];
            Vec3 mid = (1f / 3f) * (currentTriangle.p1 + currentTriangle.p2 + currentTriangle.p3);
            for (int i = 1; i < content.Count; i++) {
                currentTriangle = (Triangle)content[i];
                mid = (i / (i + 1f)) * mid + (1f / (i + 1f)) * (1f / 3f) * (currentTriangle.p1 + currentTriangle.p2 + currentTriangle.p3);
            }
            return mid;
        }

        protected override void SplitOnPlane(
                List<IIntersectable> splitContent,
                Axis axis,
                Vec3 position,
                out List<IIntersectable> leftContent,
                out List<IIntersectable> rightContent) {

            leftContent = new List<IIntersectable>();
            rightContent = new List<IIntersectable>();

            switch (axis) {
                case Axis.X:
                    foreach (Triangle triangle in splitContent) {
                        if (triangle.p1.x <= position.x || triangle.p2.x <= position.x || triangle.p3.x <= position.x)
                            leftContent.Add(triangle);
                        if (triangle.p1.x > position.x || triangle.p2.x > position.x || triangle.p3.x > position.x)
                            rightContent.Add(triangle);
                    }
                    break;
                case Axis.Y:
                    foreach (Triangle triangle in splitContent) {
                        if (triangle.p1.y <= position.y || triangle.p2.y <= position.y || triangle.p3.y <= position.y)
                            leftContent.Add(triangle);
                        if (triangle.p1.y > position.y || triangle.p2.y > position.y || triangle.p3.y > position.y)
                            rightContent.Add(triangle);
                    }
                    break;
                case Axis.Z:
                    foreach (Triangle triangle in splitContent) {
                        if (triangle.p1.z <= position.z || triangle.p2.z <= position.z || triangle.p3.z <= position.z)
                            leftContent.Add(triangle);
                        if (triangle.p1.z > position.z || triangle.p2.z > position.z || triangle.p3.z > position.z)
                            rightContent.Add(triangle);
                    }
                    break;
            }
        }
    }
}
