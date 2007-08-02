using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracerFramework.Geometry {
    public class Vec4 {
        public float x, y, z, w;

        public Vec4() : this(0, 0, 0, 0) { }

        public Vec4(float x, float y, float z, float w) {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }


        public float Length() {
            return (float)Math.Sqrt(x * x + y * y + z * z + w * w);
        }

        public float LengthSq() {
            return x * x + y * y + z * z + w * w;
        }

        public static Vec4 Lerp(Vec4 left, Vec4 right, float interpolator) {
            return left + interpolator * (right - left);
        }

        public Vec4 Normalize() {
            float length = (float)Math.Sqrt(x * x + y * y + z * z + w * w);
            return new Vec4(x / length, y / length, z / length, w / length);
        }


        public static Vec4 Transform(Vec4 v, Matrix m) {
            return new Vec4(v.x * m.m11 + v.y * m.m21 + v.z * m.m31 + v.w * m.m41,
                            v.x * m.m12 + v.y * m.m22 + v.z * m.m32 + v.w * m.m42,
                            v.x * m.m13 + v.y * m.m23 + v.z * m.m33 + v.w * m.m43,
                            v.x * m.m14 + v.y * m.m24 + v.z * m.m34 + v.w * m.m44);
        }

        //-------------- Overloaded operators -------------------
        public static Vec4 operator +(Vec4 lhs, Vec4 rhs) {
            return new Vec4(lhs.x + rhs.x, lhs.y + rhs.y, lhs.z + rhs.z, lhs.w + rhs.w);
        }

        public static Vec4 operator -(Vec4 v) {
            return new Vec4(-v.x, -v.y, -v.z, -v.w);
        }

        public static Vec4 operator -(Vec4 lhs, Vec4 rhs) {
            return new Vec4(lhs.x - rhs.x, lhs.y - rhs.y, lhs.z - rhs.z, lhs.w - rhs.w);
        }

        public static Vec4 operator *(float scalar, Vec4 v) {
            return new Vec4(scalar * v.x, scalar * v.y, scalar * v.z, scalar * v.w);
        }

        public static Vec4 operator *(Vec4 v, float scalar) {
            return new Vec4(scalar * v.x, scalar * v.y, scalar * v.z, scalar * v.w);
        }

        public override string ToString() {
            return "(" + x + ", " + y + ", " + z + ", " + w + ")";
        }

    }
    
}
