using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracerFramework.Geometry {
    public class GeoObjectKDTree : KDTree {
        protected readonly static int DefaultMaxDesiredGEOMETRICObjectsPerLeafCount = 2;

        public GeoObjectKDTree() : base() {
            MaxDesiredObjectsPerLeafCount = DefaultMaxDesiredGEOMETRICObjectsPerLeafCount;
        }

        public GeoObjectKDTree(List<IIntersectable> content) : base(content) {
            MaxDesiredObjectsPerLeafCount = DefaultMaxDesiredGEOMETRICObjectsPerLeafCount;
        }

        protected override void SplitOnPlane(List<IIntersectable> splitContent, KDTree.Axis axis, Vec3 position, out List<IIntersectable> leftContent, out List<IIntersectable> rightContent) {
            leftContent = new List<IIntersectable>();
            rightContent = new List<IIntersectable>();

            switch (axis) {
                case Axis.X:
                    foreach (IGeometricObject geoObj in splitContent) {
                        if (geoObj.BSphere.center.x - geoObj.BSphere.radius <= position.x)
                            leftContent.Add(geoObj);
                        if (geoObj.BSphere.center.x + geoObj.BSphere.radius >= position.x)
                            rightContent.Add(geoObj);
                    }
                    break;
                case Axis.Y:
                    foreach (IGeometricObject geoObj in splitContent) {
                        if (geoObj.BSphere.center.y - geoObj.BSphere.radius <= position.y)
                            leftContent.Add(geoObj);
                        if (geoObj.BSphere.center.y + geoObj.BSphere.radius >= position.y)
                            rightContent.Add(geoObj);
                    }
                    break;
                case Axis.Z:
                    foreach (IGeometricObject geoObj in splitContent) {
                        if (geoObj.BSphere.center.z - geoObj.BSphere.radius <= position.z)
                            leftContent.Add(geoObj);
                        if (geoObj.BSphere.center.z + geoObj.BSphere.radius >= position.z)
                            rightContent.Add(geoObj);
                    }
                    break;
            }
        }

        protected override Vec3 CalculateMid(List<IIntersectable> content) {
            IGeometricObject currentObj = (IGeometricObject)content[0];
            Vec3 mid = currentObj.BSphere.center;
            for (int i = 1; i < content.Count; i++) {
                currentObj = (IGeometricObject)content[i];
                mid = (i / (i + 1f)) * mid + (1f / (i + 1f)) * currentObj.BSphere.center;
            }
            return mid;
        }
    }
}
