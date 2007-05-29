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
            //containsOther = false;
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

        public void Transform(Matrix transformation, Matrix invTransformation) {
            this.transform *= transformation;
            this.invTransform = invTransform * this.invTransform;
        }

        public bool Intersect(Ray ray) {
            // Transform ray to object space
            Ray rayOS = ray.Transform(invTransform);

            // Check if ray starts inside box
            if (rayOS.position.x > 0 && rayOS.position.x < dx
                    && rayOS.position.y > 0 && rayOS.position.y < dy
                    && rayOS.position.z > 0 && rayOS.position.z < dz)
                return true;           

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
            
            // Check if ray starts inside box
            bool inside = false;
            if (rayOS.position.x > 0 && rayOS.position.x < dx
                    && rayOS.position.y > 0 && rayOS.position.y < dy
                    && rayOS.position.z > 0 && rayOS.position.z < dz)
                inside = true;

            // Z
            if (TestOne(ray, rayOS, rayOS.direction.z, rayOS.direction.x, rayOS.direction.y,
                    rayOS.position.z, rayOS.position.x, rayOS.position.y, dz, dx, dy, Vec3.StdZAxis,
                    inside, out firstIntersection))
                return true;

            // Y
            if (TestOne(ray, rayOS, rayOS.direction.y, rayOS.direction.x, rayOS.direction.z,
                    rayOS.position.y, rayOS.position.x, rayOS.position.z, dy, dx, dz, Vec3.StdYAxis,
                    inside, out firstIntersection))
                return true;

            // X
            if (TestOne(ray, rayOS, rayOS.direction.x, rayOS.direction.y, rayOS.direction.z,
                    rayOS.position.x, rayOS.position.y, rayOS.position.z, dx, dy, dz, Vec3.StdXAxis,
                    inside, out firstIntersection))
                return true;

            //assert if (inside) throw new Exception("inside box but no intersection");

            firstIntersection = null;
            return false;
        }

        private bool TestOne(
                Ray ray,
                Ray rayOS,
                float rayOSDir1, // dir.z
                float rayOSDir2, // dir.x
                float rayOSDir3, // dir.y
                float rayOSPos1, // pos.z
                float rayOSPos2, // pos.x
                float rayOSPos3, // pos.y
                float d1, // dz
                float d2, // dx
                float d3, // dy
                Vec3 normalAxis,
                bool inside,
                out RayIntersectionPoint firstIntersection) {
            float tOS = 0.0f, p2, p3;
            Vec3 intersectionPos, intersectionNormal, localPos;
            float t = 0.0f;
            bool intersect = false, negateNormal = false;
            if (inside) {
                if (rayOSDir1 < 0)
                { // Test against front plane
                    tOS = rayOSPos1 / -rayOSDir1;
                    p2 = rayOSPos2 + rayOSDir2 * tOS;
                    p3 = rayOSPos3 + rayOSDir3 * tOS;
                    if (p2 >= 0 && p2 <= d2 && p3 >= 0 && p3 <= d3)
                        intersect = true; 
                }
                else if (rayOSDir1 > 0)
                { // Test against back plane
                    tOS = -(rayOSPos1 - d1) / rayOSDir1;
                    p2 = rayOSPos2 + rayOSDir2 * tOS;
                    p3 = rayOSPos3 + rayOSDir3 * tOS;
                    if (p2 >= 0 && p2 <= d2 && p3 >= 0 && p3 <= d3) {
                        negateNormal = true;
                        intersect = true;
                    }
                }
            } else {
                if (rayOSDir1 > 0 && rayOSPos1 < 0)
                { // Test against front plane
                    tOS = -rayOSPos1 / rayOSDir1;
                    p2 = rayOSPos2 + rayOSDir2 * tOS;
                    p3 = rayOSPos3 + rayOSDir3 * tOS;
                    if (p2 >= 0 && p2 <= d2 && p3 >= 0 && p3 <= d3)
                    {
                        intersect = true;
                        negateNormal = true;
                    }
                }
                else if (rayOSDir1 < 0 && rayOSPos1 > d1)
                { // Test against back plane
                    tOS = (rayOSPos1 - d1) / -rayOSDir1;
                    p2 = rayOSPos2 + rayOSDir2 * tOS;
                    p3 = rayOSPos3 + rayOSDir3 * tOS;
                    if (p2 >= 0 && p2 <= d2 && p3 >= 0 && p3 <= d3)
                        intersect = true;
                    }
            }

            if (intersect) {
                localPos = rayOS.GetPoint(tOS);
                intersectionPos = Vec3.TransformPosition3(rayOS.GetPoint(tOS), transform);
                intersectionNormal = Vec3.TransformNormal3n(negateNormal ? -normalAxis : normalAxis, transform);
                t = Vec3.GetLength(intersectionPos - ray.position);
                firstIntersection = new RayIntersectionPoint(intersectionPos, intersectionNormal, t, this);
                // .. p2/d2, p3/d3)
                return true;
            }
            else {
                firstIntersection = null;
                return false;
            }
        }

        public int Intersect(Ray ray, ref SortedList<float, RayIntersectionPoint> intersections) {
            // Transform ray to object space
            Ray rayOS = ray.Transform(invTransform);                        
            int numIntersections = 0;

            // Check if ray starts inside box
            bool inside = false;
            int maxIntersections = 2;
            if (rayOS.position.x > 0 && rayOS.position.x < dx
                    && rayOS.position.y > 0 && rayOS.position.y < dy
                    && rayOS.position.z > 0 && rayOS.position.z < dz) {
                inside = true;
                maxIntersections = 1;
            }

            // Z
            numIntersections += TestTwo(ray, rayOS, rayOS.direction.z, rayOS.direction.x, rayOS.direction.y,
                    rayOS.position.z, rayOS.position.x, rayOS.position.y, dz, dx, dy, Vec3.StdZAxis,
                    inside, ref intersections);
            if (numIntersections == maxIntersections)
                return numIntersections;

            // Y
            numIntersections += TestTwo(ray, rayOS, rayOS.direction.y, rayOS.direction.x, rayOS.direction.z,
                rayOS.position.y, rayOS.position.x, rayOS.position.z, dy, dx, dz, Vec3.StdYAxis,
                inside, ref intersections);
            if (numIntersections == maxIntersections)
                return numIntersections;

            // X
            numIntersections += TestTwo(ray, rayOS, rayOS.direction.x, rayOS.direction.y, rayOS.direction.z,
                    rayOS.position.x, rayOS.position.y, rayOS.position.z, dx, dy, dz, Vec3.StdXAxis,
                    inside, ref intersections);
            return numIntersections;
        }

        private int TestTwo(
                Ray ray,
                Ray rayOS,
                float rayOSDir1, 
                float rayOSDir2, 
                float rayOSDir3, 
                float rayOSPos1, 
                float rayOSPos2, 
                float rayOSPos3, 
                float d1, 
                float d2, 
                float d3, 
                Vec3 normalAxis,
                bool inside,
                ref SortedList<float, RayIntersectionPoint> intersections) {
            float tOS = 0.0f, p2, p3;
            Vec3 intersectionPos, intersectionNormal, localPos;
            float t = 0.0f;
            int numIntersections = 0;
            if (inside) {
                if (rayOSDir1 < 0) { // Test against front plane
                    tOS = rayOSPos1 / -rayOSDir1;
                    p2 = rayOSPos2 + rayOSDir2 * tOS;
                    p3 = rayOSPos3 + rayOSDir3 * tOS;
                    if (p2 >= 0 && p2 <= d2 && p3 >= 0 && p3 <= d3) {
                        localPos = rayOS.GetPoint(tOS);
                        intersectionPos = Vec3.TransformPosition3(rayOS.GetPoint(tOS), transform);
                        intersectionNormal = Vec3.TransformNormal3n(normalAxis, transform);
                        t = Vec3.GetLength(intersectionPos - ray.position);
                        intersections.Add(t, new RayIntersectionPoint(intersectionPos, intersectionNormal, t, this));
                        numIntersections ++;
                    }
                } else if (rayOSDir1 > 0) { // Test against back plane
                    tOS = -(rayOSPos1 - d1) / rayOSDir1;
                    p2 = rayOSPos2 + rayOSDir2 * tOS;
                    p3 = rayOSPos3 + rayOSDir3 * tOS;
                    if (p2 >= 0 && p2 <= d2 && p3 >= 0 && p3 <= d3) {
                        localPos = rayOS.GetPoint(tOS);
                        intersectionPos = Vec3.TransformPosition3(rayOS.GetPoint(tOS), transform);
                        intersectionNormal = Vec3.TransformNormal3n(-normalAxis, transform);
                        t = Vec3.GetLength(intersectionPos - ray.position);
                        intersections.Add(t, new RayIntersectionPoint(intersectionPos, intersectionNormal, t, this));
                        numIntersections++;
                    }
                }
            } else {
                if (rayOSDir1 > 0 && rayOSPos1 < 0) { // Test against front plane
                    tOS = -rayOSPos1 / rayOSDir1;
                    p2 = rayOSPos2 + rayOSDir2 * tOS;
                    p3 = rayOSPos3 + rayOSDir3 * tOS;
                    if (p2 >= 0 && p2 <= d2 && p3 >= 0 && p3 <= d3) {
                        localPos = rayOS.GetPoint(tOS);
                        intersectionPos = Vec3.TransformPosition3(rayOS.GetPoint(tOS), transform);
                        intersectionNormal = Vec3.TransformNormal3n(-normalAxis, transform);
                        t = Vec3.GetLength(intersectionPos - ray.position);
                        intersections.Add(t, new RayIntersectionPoint(intersectionPos, intersectionNormal, t, this));
                        numIntersections++;
                    }
                } else if (rayOSDir1 < 0 && rayOSPos1 > d1) { // Test against back plane
                    tOS = (rayOSPos1 - d1) / -rayOSDir1;
                    p2 = rayOSPos2 + rayOSDir2 * tOS;
                    p3 = rayOSPos3 + rayOSDir3 * tOS;
                    if (p2 >= 0 && p2 <= d2 && p3 >= 0 && p3 <= d3) {
                        localPos = rayOS.GetPoint(tOS);
                        intersectionPos = Vec3.TransformPosition3(rayOS.GetPoint(tOS), transform);
                        intersectionNormal = Vec3.TransformNormal3n(normalAxis, transform);
                        t = Vec3.GetLength(intersectionPos - ray.position);
                        intersections.Add(t, new RayIntersectionPoint(intersectionPos, intersectionNormal, t, this));
                        numIntersections++;
                    }
                }
            }

            return numIntersections;
        }

    }
}
