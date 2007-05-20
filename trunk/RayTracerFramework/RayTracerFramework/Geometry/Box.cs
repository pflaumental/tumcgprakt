using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracerFramework.Geometry {

    // A box with lower left corner centered at the object space origin (0,0,0) and
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

            float tOS, x, y, z;
            if (rayOS.direction.z > 0 && rayOS.position.z < 0) { // Test against front plane
                tOS = -rayOS.position.z / rayOS.direction.z;
                if (tOS > 0) {
                    x = rayOS.position.x + rayOS.direction.x * tOS;
                    y = rayOS.position.y + rayOS.direction.y * tOS;
                    if (x >= 0 && x <= dx && y >= 0 && y <= dy)
                        return true;
                }
            } else if (rayOS.direction.z < 0 && rayOS.position.z > dz) { // Test against back plane
                tOS = (rayOS.position.z - dz) / -rayOS.direction.z;
                if (tOS > 0) {
                    x = rayOS.position.x + rayOS.direction.x * tOS;
                    y = rayOS.position.y + rayOS.direction.y * tOS;
                    if (x >= 0 && x <= dx && y >= 0 && y <= dy)
                        return true;
                }
            }

            if (rayOS.direction.y > 0 && rayOS.position.y < 0) { // Test against lower plane
                tOS = -rayOS.position.y / rayOS.direction.y;
                if (tOS > 0) {
                    x = rayOS.position.x + rayOS.direction.x * tOS;
                    z = rayOS.position.z + rayOS.direction.z * tOS;
                    if (x >= 0 && x <= dx && z >= 0 && z <= dz)
                        return true;
                }
            } else if (rayOS.direction.y < 0 && rayOS.position.y > dy) { // Test against upper plane
                tOS = (rayOS.position.y - dy) / -rayOS.direction.y;
                if (tOS > 0) {
                    x = rayOS.position.x + rayOS.direction.x * tOS;
                    z = rayOS.position.z + rayOS.direction.z * tOS;
                    if (x >= 0 && x <= dx && z >= 0 && z <= dz)
                        return true;
                }
            }

            if (rayOS.direction.x > 0 && rayOS.position.x < 0) { // Test against left plane
                tOS = -rayOS.position.x / rayOS.direction.x;
                if (tOS > 0) {
                    y = rayOS.position.y + rayOS.direction.y * tOS;
                    z = rayOS.position.z + rayOS.direction.z * tOS;
                    if (y >= 0 && y <= dy && z >= 0 && z <= dz)
                        return true;
                }
            } else if (rayOS.direction.x < 0 && rayOS.position.x > dx) { // Test against right plane
                tOS = (rayOS.position.x - dx) / -rayOS.direction.x;
                if (tOS > 0) {
                    y = rayOS.position.y + rayOS.direction.y * tOS;
                    z = rayOS.position.z + rayOS.direction.z * tOS;
                    if (y >= 0 && y <= dy && z >= 0 && z <= dz)
                        return true;
                }
            }
            return false;
        }

        public bool Intersect(Ray ray, out RayIntersectionPoint firstIntersection) {
            // Transform ray to object space
            Ray rayOS = ray.Transform(invTransform);

            float tOS, x, y, z;
            Vec3 intersectionPos, intersectionNormal;
            float t = 0.0f;
            if (rayOS.direction.z > 0 && rayOS.position.z < 0)
            { // Test against front plane
                tOS = -rayOS.position.z / rayOS.direction.z;
                if (tOS >= 0) {
                    x = rayOS.position.x + rayOS.direction.x * tOS;
                    y = rayOS.position.y + rayOS.direction.y * tOS;
                    if (x >= 0 && x <= dx && y >= 0 && y <= dy) {
                        intersectionPos = Vec3.TransformPosition3(rayOS.GetPoint(tOS), transform);
                        intersectionNormal = Vec3.TransformNormal3n(-Vec3.StdZAxis, transform);                        
                        t = Vec3.GetLength(intersectionPos - ray.position);
                        firstIntersection = new RayIntersectionPoint(intersectionPos, intersectionNormal, t, this);
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
                        intersectionPos = Vec3.TransformPosition3(rayOS.GetPoint(tOS), transform);
                        intersectionNormal = Vec3.TransformNormal3n(Vec3.StdZAxis, transform);
                        t = Vec3.GetLength(intersectionPos - ray.position);
                        firstIntersection = new RayIntersectionPoint(intersectionPos, intersectionNormal, t, this);
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
                        intersectionPos = Vec3.TransformPosition3(rayOS.GetPoint(tOS), transform);
                        intersectionNormal = Vec3.TransformNormal3n(-Vec3.StdYAxis, transform);
                        t = Vec3.GetLength(intersectionPos - ray.position);
                        firstIntersection = new RayIntersectionPoint(intersectionPos, intersectionNormal, t, this);
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
                        intersectionPos = Vec3.TransformPosition3(rayOS.GetPoint(tOS), transform);
                        intersectionNormal = Vec3.TransformNormal3n(Vec3.StdYAxis, transform);
                        t = Vec3.GetLength(intersectionPos - ray.position);
                        firstIntersection = new RayIntersectionPoint(intersectionPos, intersectionNormal, t, this);
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
                        intersectionPos = Vec3.TransformPosition3(rayOS.GetPoint(tOS), transform);
                        intersectionNormal = Vec3.TransformNormal3n(-Vec3.StdXAxis, transform);
                        t = Vec3.GetLength(intersectionPos - ray.position);
                        firstIntersection = new RayIntersectionPoint(intersectionPos, intersectionNormal, t, this);
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
                        intersectionPos = Vec3.TransformPosition3(rayOS.GetPoint(tOS), transform);
                        intersectionNormal = Vec3.TransformNormal3n(Vec3.StdXAxis, transform);
                        t = Vec3.GetLength(intersectionPos - ray.position);
                        firstIntersection = new RayIntersectionPoint(intersectionPos, intersectionNormal, t, this);
                        return true;
                    }
                }
            }
            firstIntersection = null;
            t = float.PositiveInfinity;
            return false;
        }

        public int Intersect(Ray ray, out SortedList<float, RayIntersectionPoint> intersections) {
            // Transform ray to object space
            Ray rayOS = ray.Transform(invTransform);
            bool firstFound = false;
            float t = float.PositiveInfinity;
            intersections = null;

            float tOS, x, y, z;
            Vec3 intersectionPos, intersectionNormal;
            intersections = new SortedList<float,RayIntersectionPoint>();
            if (rayOS.direction.z > 0 && rayOS.position.z < 0) { // Test against front plane
                tOS = -rayOS.position.z / rayOS.direction.z;
                if (tOS >= 0) {
                    x = rayOS.position.x + rayOS.direction.x * tOS;
                    y = rayOS.position.y + rayOS.direction.y * tOS;
                    if (x >= 0 && x <= dx && y >= 0 && y <= dy) {
                        intersectionPos = Vec3.TransformPosition3(rayOS.GetPoint(tOS), transform);
                        intersectionNormal = Vec3.TransformNormal3n(-Vec3.StdZAxis, transform);
                        t = Vec3.GetLength(intersectionPos - ray.position);
                        intersections.Add(t, new RayIntersectionPoint(intersectionPos, intersectionNormal, t, this));
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
                        intersectionPos = Vec3.TransformPosition3(rayOS.GetPoint(tOS), transform);
                        intersectionNormal = Vec3.TransformNormal3n(Vec3.StdZAxis, transform);
                        t = Vec3.GetLength(intersectionPos - ray.position);
                        intersections.Add(t, new RayIntersectionPoint(intersectionPos, intersectionNormal, t, this));
                        if (firstFound)
                            return 2;
                        else
                            firstFound = true;
                    }
                }
            }

            if (rayOS.direction.y > 0 && rayOS.position.y < 0) { // Test against lower plane
                tOS = -rayOS.position.y / rayOS.direction.y;
                if (tOS >= 0) {
                    x = rayOS.position.x + rayOS.direction.x * tOS;
                    z = rayOS.position.z + rayOS.direction.z * tOS;
                    if (x >= 0 && x <= dx && z >= 0 && z <= dz) {
                        intersectionPos = Vec3.TransformPosition3(rayOS.GetPoint(tOS), transform);
                        intersectionNormal = Vec3.TransformNormal3n(-Vec3.StdYAxis, transform);
                        t = Vec3.GetLength(intersectionPos - ray.position);
                        intersections.Add(t, new RayIntersectionPoint(intersectionPos, intersectionNormal, t, this));
                        if (firstFound)
                            return 2;
                        else
                            firstFound = true;
                    }                    
                }
            }
            if (rayOS.direction.y < 0 && rayOS.position.y > dy) { // Test against upper plane
                tOS = (rayOS.position.y - dy) / -rayOS.direction.y;
                if (tOS >= 0) {
                    x = rayOS.position.x + rayOS.direction.x * tOS;
                    z = rayOS.position.z + rayOS.direction.z * tOS;
                    if (x >= 0 && x <= dx && z >= 0 && z <= dz) {
                        intersectionPos = Vec3.TransformPosition3(rayOS.GetPoint(tOS), transform);
                        intersectionNormal = Vec3.TransformNormal3n(Vec3.StdYAxis, transform);
                        t = Vec3.GetLength(intersectionPos - ray.position);
                        intersections.Add(t, new RayIntersectionPoint(intersectionPos, intersectionNormal, t, this));
                        if (firstFound)
                            return 2;
                        else
                            firstFound = true;
                    }
                }
            }

            if (rayOS.direction.x > 0 && rayOS.position.x < 0) { // Test against left plane
                tOS = -rayOS.position.x / rayOS.direction.x;
                if (tOS >= 0) {
                    y = rayOS.position.y + rayOS.direction.y * tOS;
                    z = rayOS.position.z + rayOS.direction.z * tOS;
                    if (y >= 0 && y <= dy && z >= 0 && z <= dz) {
                        intersectionPos = Vec3.TransformPosition3(rayOS.GetPoint(tOS), transform);
                        intersectionNormal = Vec3.TransformNormal3n(-Vec3.StdXAxis, transform);
                        t = Vec3.GetLength(intersectionPos - ray.position);
                        intersections.Add(t, new RayIntersectionPoint(intersectionPos, intersectionNormal, t, this));
                        if (firstFound)
                            return 2;
                        else
                            firstFound = true;
                    }
                }
            }
            if (rayOS.direction.x < 0 && rayOS.position.x > dx) { // Test against right plane
                tOS = (rayOS.position.x - dx) / -rayOS.direction.x;
                if (tOS >= 0) {
                    y = rayOS.position.y + rayOS.direction.y * tOS;
                    z = rayOS.position.z + rayOS.direction.z * tOS;
                    if (y >= 0 && y <= dy && z >= 0 && z <= dz) {
                        intersectionPos = Vec3.TransformPosition3(rayOS.GetPoint(tOS), transform);
                        intersectionNormal = Vec3.TransformNormal3n(Vec3.StdXAxis, transform);
                        t = Vec3.GetLength(intersectionPos - ray.position);
                        intersections.Add(t, new RayIntersectionPoint(intersectionPos, intersectionNormal, t, this));
                        if (firstFound)
                            return 2;
                        else
                            firstFound = true;
                    }
                }
            }
            return firstFound ? 1 : 0;
        }

    }
}
