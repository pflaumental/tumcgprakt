using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracerFramework.Geometry {

    // A box with lower left for corner centered at the object space origin (0,0,0) and
    // the upper right back corner at (dx, dy, dz)
    abstract class Box : IGeometricObject {        
        protected float dx, dy, dz;

        protected Matrix transform;
        protected Matrix invTransform;


        protected Box(Vec3 position, float width, float height, float depth) {            
            this.dx = width;
            this.dy = height;
            this.dz = depth;
            transform = Matrix.GetTranslation(position);
            invTransform = Matrix.GetTranslation(-position);
        }

        protected Box(Matrix transform, Matrix invTransform, float width, float height, float depth) {
            this.dx = width;
            this.dy = height;
            this.dz = depth;
            this.transform = transform;
            this.invTransform = invTransform;
        }

        public void Transform(Matrix transformation)
        {
            this.transform *= transformation;
            this.invTransform = Matrix.Invert(this.transform);
        }

        public void Transform(Matrix transformation, Matrix invTransformation)
        {
            this.transform *= transformation;
            this.invTransform = invTransformation * this.invTransform;
        }
       
        public bool Intersect(Ray ray) {
            // Transform ray to object space
            Ray rayOS = ray.Transform(invTransform);

            float t, x, y, z;
            if (rayOS.direction.z > 0 && rayOS.position.z < 0) { // Test against front plane
                t = -rayOS.position.z / rayOS.direction.z;
                if (t < 0) {
                    x = rayOS.position.x + rayOS.direction.x * t;
                    y = rayOS.position.y + rayOS.direction.y * t;
                    if (x >= 0 && x <= dx && y >= 0 && y <= dy)
                        return true;
                }
            } else if (rayOS.direction.z < 0 && rayOS.position.z > dz) { // Test against back plane
                t = (rayOS.position.z - dz) / -rayOS.direction.z;
                if (t < 0) {
                    x = rayOS.position.x + rayOS.direction.x * t;
                    y = rayOS.position.y + rayOS.direction.y * t;
                    if (x >= 0 && x <= dx && y >= 0 && y <= dy)
                        return true;
                }
            }

            if (rayOS.direction.y > 0 && rayOS.position.y < 0) { // Test against lower plane
                t = -rayOS.position.y / rayOS.direction.y;
                if (t < 0) {
                    x = rayOS.position.x + rayOS.direction.x * t;
                    z = rayOS.position.z + rayOS.direction.z * t;
                    if (x >= 0 && x <= dx && z >= 0 && z <= dz)
                        return true;
                }
            } else if (rayOS.direction.y < 0 && rayOS.position.y > dy) { // Test against upper plane
                t = (rayOS.position.y - dy) / -rayOS.direction.y;
                if (t < 0) {
                    x = rayOS.position.x + rayOS.direction.x * t;
                    z = rayOS.position.z + rayOS.direction.z * t;
                    if (x >= 0 && x <= dx && z >= 0 && z <= dz)
                        return true;
                }
            }

            if (rayOS.direction.x > 0 && rayOS.position.x < 0) { // Test against left plane
                t = -rayOS.position.x / rayOS.direction.x;
                if (t < 0) {
                    y = rayOS.position.y + rayOS.direction.y * t;
                    z = rayOS.position.z + rayOS.direction.z * t;
                    if (y >= 0 && y <= dy && z >= 0 && z <= dz)
                        return true;
                }
            } else if (rayOS.direction.x < 0 && rayOS.position.x > dx) { // Test against right plane
                t = (rayOS.position.x - dx) / -rayOS.direction.x;
                if (t < 0) {
                    y = rayOS.position.y + rayOS.direction.y * t;
                    z = rayOS.position.z + rayOS.direction.z * t;
                    if (y >= 0 && y <= dy && z >= 0 && z <= dz)
                        return true;
                }
            }
            return false;
        }

        public bool Intersect(Ray ray, out IntersectionPoint firstIntersection, out float t) {
            // Transform ray to object space
            Ray rayOS = ray.Transform(invTransform);

            float tOS, x, y, z;
            Vec3 intersectionPos, intersectionNormal;
            if (rayOS.direction.z > 0 && rayOS.position.z < 0)
            { // Test against front plane
                tOS = -rayOS.position.z / rayOS.direction.z;
                if (tOS >= 0) {
                    x = rayOS.position.x + rayOS.direction.x * tOS;
                    y = rayOS.position.y + rayOS.direction.y * tOS;
                    if (x >= 0 && x <= dx && y >= 0 && y <= dy) {
                        intersectionPos = Vec3.TransformPosition3(ray.GetPoint(tOS), transform);
                        intersectionNormal = Vec3.TransformNormal3n(-Vec3.StdZAxis, transform);
                        firstIntersection = new IntersectionPoint(intersectionPos, intersectionNormal);
                        t = Vec3.GetLength(intersectionPos - ray.position);
                        return true;
                    }
                }
            } else if (rayOS.direction.z < 0 && rayOS.position.z > dz)
            { // Test against back plane
                tOS = (rayOS.position.z - dz) / -rayOS.direction.z;
                if (tOS >= 0) {
                    x = rayOS.position.x + rayOS.direction.x * tOS;
                    y = rayOS.position.y + rayOS.direction.y * tOS;
                    if (x >= 0 && x <= dx && y >= 0 && y <= dy) {
                        intersectionPos = Vec3.TransformPosition3(ray.GetPoint(tOS), transform);
                        intersectionNormal = Vec3.TransformNormal3n(Vec3.StdZAxis, transform);
                        firstIntersection = new IntersectionPoint(intersectionPos, intersectionNormal);
                        t = Vec3.GetLength(intersectionPos - ray.position);
                        return true;
                    }
                }
            }

            if (rayOS.direction.y > 0 && rayOS.position.y < 0)
            { // Test against lower plane
                tOS = -rayOS.position.y / rayOS.direction.y;
                if (tOS >= 0) {
                    x = rayOS.position.x + rayOS.direction.x * tOS;
                    z = rayOS.position.z + rayOS.direction.z * tOS;
                    if (x >= 0 && x <= dx && z >= 0 && z <= dz) {
                        intersectionPos = Vec3.TransformPosition3(ray.GetPoint(tOS), transform);
                        intersectionNormal = Vec3.TransformNormal3n(-Vec3.StdYAxis, transform);
                        firstIntersection = new IntersectionPoint(intersectionPos, intersectionNormal);
                        t = Vec3.GetLength(intersectionPos - ray.position);
                        return true;
                    }
                }
            } else if (rayOS.direction.y < 0 && rayOS.position.y > dy)
            { // Test against upper plane
                tOS = (rayOS.position.y - dy) / -rayOS.direction.y;
                if (tOS >= 0) {
                    x = rayOS.position.x + rayOS.direction.x * tOS;
                    z = rayOS.position.z + rayOS.direction.z * tOS;
                    if (x >= 0 && x <= dx && z >= 0 && z <= dz) {
                        intersectionPos = Vec3.TransformPosition3(ray.GetPoint(tOS), transform);
                        intersectionNormal = Vec3.TransformNormal3n(Vec3.StdYAxis, transform);
                        firstIntersection = new IntersectionPoint(intersectionPos, intersectionNormal);
                        t = Vec3.GetLength(intersectionPos - ray.position);
                        return true;
                    }
                }
            }

            if (rayOS.direction.x > 0 && rayOS.position.x < 0)
            { // Test against left plane
                tOS = -rayOS.position.x / rayOS.direction.x;
                if (tOS >= 0) {
                    y = rayOS.position.y + rayOS.direction.y * tOS;
                    z = rayOS.position.z + rayOS.direction.z * tOS;
                    if (y >= 0 && y <= dy && z >= 0 && z <= dz) {
                        intersectionPos = Vec3.TransformPosition3(ray.GetPoint(tOS), transform);
                        intersectionNormal = Vec3.TransformNormal3n(-Vec3.StdXAxis, transform);
                        firstIntersection = new IntersectionPoint(intersectionPos, intersectionNormal);
                        t = Vec3.GetLength(intersectionPos - ray.position);
                        return true;
                    }
                }
            } else if (rayOS.direction.x < 0 && rayOS.position.x > dx)
            { // Test against right plane
                tOS = (rayOS.position.x - dx) / -rayOS.direction.x;
                if (tOS >= 0) {
                    y = rayOS.position.y + rayOS.direction.y * tOS;
                    z = rayOS.position.z + rayOS.direction.z * tOS;
                    if (y >= 0 && y <= dy && z >= 0 && z <= dz) {
                        intersectionPos = Vec3.TransformPosition3(ray.GetPoint(tOS), transform);
                        intersectionNormal = Vec3.TransformNormal3n(Vec3.StdXAxis, transform);
                        firstIntersection = new IntersectionPoint(intersectionPos, intersectionNormal);
                        t = Vec3.GetLength(intersectionPos - ray.position);
                        return true;
                    }
                }
            }
            firstIntersection = null;
            t = float.PositiveInfinity;
            return false;
        }

        public int Intersect(Ray ray, out IntersectionPoint[] intersections, out float t1, out float t2) {
            // Transform ray to object space
            Ray rayOS = ray.Transform(invTransform);
            bool firstFound = false;
            t1 = t2 = float.PositiveInfinity;
            intersections = null;

            float tOS, x, y, z;
            Vec3 intersectionPos, intersectionNormal;
            IntersectionPoint firstIntersection = null, secondIntersection = null;
            if (rayOS.direction.z > 0 && rayOS.position.z < 0) { // Test against front plane
                tOS = -rayOS.position.z / rayOS.direction.z;
                if (tOS >= 0) {
                    x = rayOS.position.x + rayOS.direction.x * tOS;
                    y = rayOS.position.y + rayOS.direction.y * tOS;
                    if (x >= 0 && x <= dx && y >= 0 && y <= dy) {
                        intersectionPos = Vec3.TransformPosition3(ray.GetPoint(tOS), transform);
                        intersectionNormal = Vec3.TransformNormal3n(-Vec3.StdZAxis, transform);
                        firstIntersection = new IntersectionPoint(intersectionPos, intersectionNormal);
                        t1 = Vec3.GetLength(intersectionPos - ray.position);
                        firstFound = true;
                    }
                }
            }
            if (rayOS.direction.z < 0 && rayOS.position.z > dz) { // Test against back plane
                tOS = (rayOS.position.z - dz) / -rayOS.direction.z;
                if (tOS >= 0) {
                    x = rayOS.position.x + rayOS.direction.x * tOS;
                    y = rayOS.position.y + rayOS.direction.y * tOS;
                    if (x >= 0 && x <= dx && y >= 0 && y <= dy) {
                        if (firstFound) {
                            intersectionPos = Vec3.TransformPosition3(ray.GetPoint(tOS), transform);
                            intersectionNormal = Vec3.TransformNormal3n(Vec3.StdZAxis, transform);
                            secondIntersection = new IntersectionPoint(intersectionPos, intersectionNormal);
                            t2 = Vec3.GetLength(intersectionPos - ray.position);
                            OrderIntersections(ref t1, firstIntersection, ref t2, secondIntersection, out intersections);
                            return 2;
                        } else {
                            intersectionPos = Vec3.TransformPosition3(ray.GetPoint(tOS), transform);
                            intersectionNormal = Vec3.TransformNormal3n(Vec3.StdZAxis, transform);
                            firstIntersection = new IntersectionPoint(intersectionPos, intersectionNormal);
                            t1 = Vec3.GetLength(intersectionPos - ray.position);
                        }
                    }
                }
            }

            if (rayOS.direction.y > 0 && rayOS.position.y < 0) { // Test against lower plane
                tOS = -rayOS.position.y / rayOS.direction.y;
                if (tOS >= 0) {
                    x = rayOS.position.x + rayOS.direction.x * tOS;
                    z = rayOS.position.z + rayOS.direction.z * tOS;
                    if (x >= 0 && x <= dx && z >= 0 && z <= dz) {
                        if (firstFound) {
                            intersectionPos = Vec3.TransformPosition3(ray.GetPoint(tOS), transform);
                            intersectionNormal = Vec3.TransformNormal3n(-Vec3.StdYAxis, transform);
                            secondIntersection = new IntersectionPoint(intersectionPos, intersectionNormal);
                            t2 = Vec3.GetLength(intersectionPos - ray.position);
                            OrderIntersections(ref t1, firstIntersection, ref t2, secondIntersection, out intersections);
                            return 2;
                        } else {
                            intersectionPos = Vec3.TransformPosition3(ray.GetPoint(tOS), transform);
                            intersectionNormal = Vec3.TransformNormal3n(-Vec3.StdYAxis, transform);
                            firstIntersection = new IntersectionPoint(intersectionPos, intersectionNormal);
                            t1 = Vec3.GetLength(intersectionPos - ray.position);
                        }
                    }
                }
            }
            if (rayOS.direction.y < 0 && rayOS.position.y > dy) { // Test against upper plane
                tOS = (rayOS.position.y - dy) / -rayOS.direction.y;
                if (tOS >= 0) {
                    x = rayOS.position.x + rayOS.direction.x * tOS;
                    z = rayOS.position.z + rayOS.direction.z * tOS;
                    if (x >= 0 && x <= dx && z >= 0 && z <= dz) {
                        if (firstFound) {
                            intersectionPos = Vec3.TransformPosition3(ray.GetPoint(tOS), transform);
                            intersectionNormal = Vec3.TransformNormal3n(Vec3.StdYAxis, transform);
                            secondIntersection = new IntersectionPoint(intersectionPos, intersectionNormal);
                            t2 = Vec3.GetLength(intersectionPos - ray.position);
                            OrderIntersections(ref t1, firstIntersection, ref t2, secondIntersection, out intersections);
                            return 2;
                        } else {
                            intersectionPos = Vec3.TransformPosition3(ray.GetPoint(tOS), transform);
                            intersectionNormal = Vec3.TransformNormal3n(Vec3.StdYAxis, transform);
                            firstIntersection = new IntersectionPoint(intersectionPos, intersectionNormal);
                            t1 = Vec3.GetLength(intersectionPos - ray.position);
                        }
                    }
                }
            }

            if (rayOS.direction.x > 0 && rayOS.position.x < 0) { // Test against left plane
                tOS = -rayOS.position.x / rayOS.direction.x;
                if (tOS >= 0) {
                    y = rayOS.position.y + rayOS.direction.y * tOS;
                    z = rayOS.position.z + rayOS.direction.z * tOS;
                    if (y >= 0 && y <= dy && z >= 0 && z <= dz) {
                        if (firstFound) {
                            intersectionPos = Vec3.TransformPosition3(ray.GetPoint(tOS), transform);
                            intersectionNormal = Vec3.TransformNormal3n(-Vec3.StdXAxis, transform);
                            secondIntersection = new IntersectionPoint(intersectionPos, intersectionNormal);
                            t2 = Vec3.GetLength(intersectionPos - ray.position);
                            OrderIntersections(ref t1, firstIntersection, ref t2, secondIntersection, out intersections);
                            return 2;
                        } else {
                            intersectionPos = Vec3.TransformPosition3(ray.GetPoint(tOS), transform);
                            intersectionNormal = Vec3.TransformNormal3n(-Vec3.StdXAxis, transform);
                            firstIntersection = new IntersectionPoint(intersectionPos, intersectionNormal);
                            t1 = Vec3.GetLength(intersectionPos - ray.position);
                        }
                    }
                }
            }
            if (rayOS.direction.x < 0 && rayOS.position.x > dx) { // Test against right plane
                tOS = (rayOS.position.x - dx) / -rayOS.direction.x;
                if (tOS >= 0) {
                    y = rayOS.position.y + rayOS.direction.y * tOS;
                    z = rayOS.position.z + rayOS.direction.z * tOS;
                    if (y >= 0 && y <= dy && z >= 0 && z <= dz) {
                        if (firstFound) {
                            intersectionPos = Vec3.TransformPosition3(ray.GetPoint(tOS), transform);
                            intersectionNormal = Vec3.TransformNormal3n(Vec3.StdXAxis, transform);
                            secondIntersection = new IntersectionPoint(intersectionPos, intersectionNormal);
                            t2 = Vec3.GetLength(intersectionPos - ray.position);
                            OrderIntersections(ref t1, firstIntersection, ref t2, secondIntersection, out intersections);
                            return 2;
                        } else {
                            intersectionPos = Vec3.TransformPosition3(ray.GetPoint(tOS), transform);
                            intersectionNormal = Vec3.TransformNormal3n(Vec3.StdXAxis, transform);
                            firstIntersection = new IntersectionPoint(intersectionPos, intersectionNormal);
                            t1 = Vec3.GetLength(intersectionPos - ray.position);
                        }
                    }
                }
            }
            return firstFound ? 1 : 0;
        }

        private void OrderIntersections(ref float t1, IntersectionPoint ip1, 
                                        ref float t2, IntersectionPoint ip2,
                                        out IntersectionPoint[] intersections) {
            intersections = new IntersectionPoint[2];
            if (t1 < t2) {
                intersections[0] = ip1;
                intersections[1] = ip2;
                return;
            }
            intersections[0] = ip2;
            intersections[1] = ip1;
            float tmpT = t1;
            t1 = t2;
            t2 = tmpT;
        }
    }
}
