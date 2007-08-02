using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace RayTracerFramework.Geometry {

    // A box with lower left corner centered at the object space origin (0,0,0) and
    // the upper right back corner at (dx, dy, dz)
    public abstract class Box : IGeometricObject {        
        public float dx, dy, dz;

        //   (0,dy,dz)_____________(dx,dy,dz)
        //           /|           /|
        //          / |   5      / |
        // (0,dy,0)/__|_____3___/<-|(dx,dy,0)
        //         | 4|         | 2|
        // (0,0,dz)|->|_________|__|(dx,0,dz)
        //         |  /   1     | /
        //         | /     6    |/
        // (0,0,0) |/___________/(dx,0,0)

        public bool textured;
        [XmlIgnore()]
        public Vec2 tex1_00 = Vec2.Vec00,     // ( 0,  0,  0)
                tex1_01 = Vec2.Vec01,         // (dx,  0,  0)
                tex1_10 = Vec2.Vec10,         // ( 0, dy,  0)
                tex1_11 = Vec2.Vec11;         // (dx, dy,  0)

        [XmlIgnore()]
        public Vec2 tex2_00 = Vec2.Vec00,     // (dx,  0,  0)
                tex2_01 = Vec2.Vec01,         // (dx,  0, dz)
                tex2_10 = Vec2.Vec10,         // (dx, dy,  0)
                tex2_11 = Vec2.Vec11;         // (dx, dy, dz)

        [XmlIgnore()]
        public Vec2 tex3_00 = Vec2.Vec00,     // (dx,  0, dz)
                tex3_01 = Vec2.Vec01,         // ( 0,  0, dz)
                tex3_10 = Vec2.Vec10,         // (dx, dy, dz)
                tex3_11 = Vec2.Vec11;         // ( 0, dy, dz)

        [XmlIgnore()]
        public Vec2 tex4_00 = Vec2.Vec00,     // ( 0,  0, dz)
                tex4_01 = Vec2.Vec01,         // ( 0,  0,  0)
                tex4_10 = Vec2.Vec10,         // ( 0, dy, dz)
                tex4_11 = Vec2.Vec11;         // ( 0, dy,  0)

        [XmlIgnore()]
        public Vec2 tex5_00 = Vec2.Vec00,     // ( 0, dy,  0)
                tex5_01 = Vec2.Vec01,         // (dx, dy,  0)
                tex5_10 = Vec2.Vec10,         // ( 0, dy, dz)
                tex5_11 = Vec2.Vec11;         // (dx, dy, dz)

        [XmlIgnore()]
        public Vec2 tex6_00 = Vec2.Vec00,     // ( 0,  0, dz)
                tex6_01 = Vec2.Vec01,         // (dx,  0, dz)
                tex6_10 = Vec2.Vec10,         // ( 0,  0,  0)
                tex6_11 = Vec2.Vec11;         // (dx,  0,  0)

        public Matrix transform;
        public Matrix invTransform;

        protected BSphere boundingSphere;

        protected Box(Vec3 position, float width, float height, float depth, bool textured) {
            this.dx = width;
            this.dy = height;
            this.dz = depth;
            this.textured = textured;
            transform = Matrix.GetTranslation(position);
            invTransform = Matrix.GetTranslation(-position);
            Vec3 r = new Vec3(width / 2, width / 2, width / 2);
            float radiusSq = Vec3.GetLengthSq(r);
            boundingSphere = new BSphere(position + r, (float)Math.Sqrt(radiusSq), radiusSq);            
        }

        protected Box(
                Matrix transform, 
                Matrix invTransform, 
                float width, 
                float height, 
                float depth, 
                bool textured, 
                BSphere boundingSphere) {
            this.dx = width;
            this.dy = height;
            this.dz = depth;
            this.textured = textured;
            this.transform = transform;
            this.invTransform = invTransform;
            this.boundingSphere = boundingSphere;
        }

        public void Transform(Matrix transformation)
        {
            this.transform *= transformation;
            this.invTransform = Matrix.Invert(this.transform);
            Setup();
        }

        
        public void Transform(Matrix transformation, Matrix invTransformation) {
            this.transform *= transformation;
            this.invTransform = invTransform * this.invTransform;
            Setup();
        }

        protected void Setup() {
            Vec3 p1 = Vec3.TransformPosition3(new Vec3(0f, 0f, 0f), transform);
            Vec3 p2 = Vec3.TransformPosition3(new Vec3(0f, 0f, dz), transform);
            Vec3 p3 = Vec3.TransformPosition3(new Vec3(0f, dy, 0f), transform);
            Vec3 p4 = Vec3.TransformPosition3(new Vec3(0f, dy, dz), transform);
            Vec3 p5 = Vec3.TransformPosition3(new Vec3(dx, 0f, 0f), transform);
            Vec3 p6 = Vec3.TransformPosition3(new Vec3(dx, 0f, dz), transform);
            Vec3 p7 = Vec3.TransformPosition3(new Vec3(dx, dy, 0f), transform);
            Vec3 p8 = Vec3.TransformPosition3(new Vec3(dx, dy, dz), transform);

            Vec3 center = (1f/8f) * (p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8);
            
            float radiusSq = Vec3.GetLengthSq(p1 - center);
            radiusSq = Math.Max(radiusSq, Vec3.GetLengthSq(p2 - center));
            radiusSq = Math.Max(radiusSq, Vec3.GetLengthSq(p3 - center));
            radiusSq = Math.Max(radiusSq, Vec3.GetLengthSq(p4 - center));
            radiusSq = Math.Max(radiusSq, Vec3.GetLengthSq(p5 - center));
            radiusSq = Math.Max(radiusSq, Vec3.GetLengthSq(p6 - center));
            radiusSq = Math.Max(radiusSq, Vec3.GetLengthSq(p7 - center));
            radiusSq = Math.Max(radiusSq, Vec3.GetLengthSq(p8 - center));

            boundingSphere = new BSphere(center, (float)Math.Sqrt(radiusSq), radiusSq);
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
                    rayOS.position.z, rayOS.position.x, rayOS.position.y, dz, dx, dy,
                    tex1_00, tex1_01, tex1_10, tex1_11,
                    tex3_01, tex3_00, tex3_11, tex3_10,
                    Vec3.StdZAxis, inside, out firstIntersection))
                return true;

            // Y
            if (TestOne(ray, rayOS, rayOS.direction.y, rayOS.direction.x, rayOS.direction.z,
                    rayOS.position.y, rayOS.position.x, rayOS.position.z, dy, dx, dz,
                    tex6_10, tex6_11, tex6_00, tex6_01,
                    tex5_00, tex5_01, tex5_10, tex5_11,
                    Vec3.StdYAxis, inside, out firstIntersection))
                return true;

            // X
            if (TestOne(ray, rayOS, rayOS.direction.x, rayOS.direction.y, rayOS.direction.z,
                    rayOS.position.x, rayOS.position.y, rayOS.position.z, dx, dy, dz,
                    tex4_01, tex4_11, tex4_00, tex4_10,
                    tex2_00, tex2_10, tex2_01, tex2_11,
                    Vec3.StdXAxis, inside, out firstIntersection))
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
                Vec2 texFront00,
                Vec2 texFront01,
                Vec2 texFront10,
                Vec2 texFront11,
                Vec2 texBack00,
                Vec2 texBack01,
                Vec2 texBack10,
                Vec2 texBack11,
                Vec3 normalAxis,
                bool inside,
                out RayIntersectionPoint firstIntersection) {
            float tOS = 0.0f, p2, p3;
            Vec3 intersectionPos, intersectionNormal, localPos;
            Vec2 tex = null;
            float t = 0.0f;
            bool intersect = false, negateNormal = false;
            if (inside) {
                if (rayOSDir1 < 0)
                { // Test against front plane
                    tOS = rayOSPos1 / -rayOSDir1;
                    p2 = rayOSPos2 + rayOSDir2 * tOS;
                    p3 = rayOSPos3 + rayOSDir3 * tOS;
                    if (p2 >= 0 && p2 <= d2 && p3 >= 0 && p3 <= d3) {
                        intersect = true;
                        if (textured)
                            tex = Vec2.BiLerp(texFront00, texFront01, texFront10, texFront11, p2 / d2, p3 / d3);
                    }
                }
                else if (rayOSDir1 > 0)
                { // Test against back plane
                    tOS = -(rayOSPos1 - d1) / rayOSDir1;
                    p2 = rayOSPos2 + rayOSDir2 * tOS;
                    p3 = rayOSPos3 + rayOSDir3 * tOS;
                    if (p2 >= 0 && p2 <= d2 && p3 >= 0 && p3 <= d3) {
                        negateNormal = true;
                        intersect = true;
                        if (textured) 
                            tex = Vec2.BiLerp(texBack00, texBack01, texBack10, texBack11, p2 / d2, p3 / d3);
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
                        if (textured) 
                            tex = Vec2.BiLerp(texFront00, texFront01, texFront10, texFront11, p2 / d2, p3 / d3);
                    }
                }
                else if (rayOSDir1 < 0 && rayOSPos1 > d1)
                { // Test against back plane
                    tOS = (rayOSPos1 - d1) / -rayOSDir1;
                    p2 = rayOSPos2 + rayOSDir2 * tOS;
                    p3 = rayOSPos3 + rayOSDir3 * tOS;
                    if (p2 >= 0 && p2 <= d2 && p3 >= 0 && p3 <= d3) {
                        intersect = true;
                        if (textured)
                            tex = Vec2.BiLerp(texBack00, texBack01, texBack10, texBack11, p2 / d2, p3 / d3);
                    }
                }
            }

            if (intersect) {
                localPos = rayOS.GetPoint(tOS);
                intersectionPos = Vec3.TransformPosition3(rayOS.GetPoint(tOS), transform);
                intersectionNormal = Vec3.TransformNormal3n(negateNormal ? -normalAxis : normalAxis, transform);
                t = Vec3.GetLength(intersectionPos - ray.position);
                firstIntersection = new RayIntersectionPoint(intersectionPos, intersectionNormal, t, this, tex);
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
                    rayOS.position.z, rayOS.position.x, rayOS.position.y, dz, dx, dy,
                    tex1_00, tex1_01, tex1_10, tex1_11,
                    tex3_01, tex3_00, tex3_11, tex3_10, 
                    Vec3.StdZAxis, inside, ref intersections);
            if (numIntersections == maxIntersections)
                return numIntersections;

            // Y
            numIntersections += TestTwo(ray, rayOS, rayOS.direction.y, rayOS.direction.x, rayOS.direction.z,
                rayOS.position.y, rayOS.position.x, rayOS.position.z, dy, dx, dz,
                    tex6_10, tex6_11, tex6_00, tex6_01,
                    tex5_00, tex5_01, tex5_10, tex5_11, 
                    Vec3.StdYAxis, inside, ref intersections);
            if (numIntersections == maxIntersections)
                return numIntersections;

            // X
            numIntersections += TestTwo(ray, rayOS, rayOS.direction.x, rayOS.direction.y, rayOS.direction.z,
                    rayOS.position.x, rayOS.position.y, rayOS.position.z, dx, dy, dz, 
                    tex4_01, tex4_11, tex4_00, tex4_10,
                    tex2_00, tex2_10, tex2_01, tex2_11, 
                    Vec3.StdXAxis, inside, ref intersections);
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
                Vec2 texFront00,
                Vec2 texFront01,
                Vec2 texFront10,
                Vec2 texFront11,
                Vec2 texBack00,
                Vec2 texBack01,
                Vec2 texBack10,
                Vec2 texBack11,
                Vec3 normalAxis,
                bool inside,
                ref SortedList<float, RayIntersectionPoint> intersections) {
            float tOS = 0.0f, p2, p3;
            Vec2 tex = null;
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
                        if (textured) 
                            tex = Vec2.BiLerp(texFront00, texFront01, texFront10, texFront11, p2 / d2, p3 / d3);
                        intersections.Add(t, new RayIntersectionPoint(intersectionPos, intersectionNormal, t, this, tex));
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
                        if (textured) 
                            tex = Vec2.BiLerp(texBack00, texBack01, texBack10, texBack11, p2 / d2, p3 / d3);
                        intersections.Add(t, new RayIntersectionPoint(intersectionPos, intersectionNormal, t, this, tex));
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
                        if (textured) 
                            tex = Vec2.BiLerp(texFront00, texFront01, texFront10, texFront11, p2 / d2, p3 / d3);
                        intersections.Add(t, new RayIntersectionPoint(intersectionPos, intersectionNormal, t, this, tex));
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
                        if (textured) 
                            tex = Vec2.BiLerp(texBack00, texBack01, texBack10, texBack11, p2 / d2, p3 / d3);
                        intersections.Add(t, new RayIntersectionPoint(intersectionPos, intersectionNormal, t, this, tex));
                        numIntersections++;
                    }
                }
            }

            return numIntersections;
        }

        public BSphere BSphere {
            get { return boundingSphere; }
        }

    }
}
