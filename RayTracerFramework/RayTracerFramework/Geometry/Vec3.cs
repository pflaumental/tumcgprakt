using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracerFramework.Geometry {

    public class Vec3 {
        public float x, y, z;
        public static Vec3 StdXAxis { get { return new Vec3(1, 0, 0); } }
        public static Vec3 StdYAxis { get { return new Vec3(0, 1, 0); } }
        public static Vec3 StdZAxis { get { return new Vec3(0, 0, 1); } }
        public static Vec3 Zero { get { return new Vec3(0, 0, 0); } }

        public Vec3() : this(0, 0, 0) { }

        public Vec3(Vec3 v) : this(v.x, v.y, v.z) { }

        public Vec3(Vec4 v) {
            if (v.w != 0 && v.w != 1) {
                this.x = v.x / v.w;
                this.y = v.y / v.w;
                this.z = v.z / v.w;
            }
            else {
                this.x = v.x;
                this.y = v.y;
                this.z = v.z;
            }
        }

        public Vec3(float x, float y, float z) {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public static Vec3 Cross(Vec3 left, Vec3 right) {
            return new Vec3(left.y * right.z - left.z * right.y,
                            left.z * right.x - left.x * right.z,
                            left.x * right.y - left.y * right.x);
        }

        public static float Dot(Vec3 left, Vec3 right) {
            return left.x * right.x + left.y * right.y + left.z * right.z;
        }

        public float Length {
            get {
                return (float)Math.Sqrt(x * x + y * y + z * z);
            }
        }

        public float LengthSq {
            get {
                return x * x + y * y + z * z;
            }
        }

        public static float GetLength(Vec3 v) {
            return (float)Math.Sqrt(v.x * v.x + v.y * v.y + v.z * v.z);
        }

        public static float GetLengthSq(Vec3 v) {
            return v.x * v.x + v.y * v.y + v.z * v.z;
        }

        public static Vec3 Lerp(Vec3 left, Vec3 right, float interpolator) {
            return left + interpolator * (right - left);
        }

        public static Vec3 Normalize(Vec3 v) {
            float length = (float)Math.Sqrt(v.x * v.x + v.y * v.y + v.z * v.z);
            return new Vec3(v.x / length, v.y / length, v.z / length);
        }

        public void Normalize() {
            float length = (float)Math.Sqrt(x * x + y * y + z * z);
            x /= length;
            y /= length;
            z /= length;
        }        

        public static Vec4 TransformPosition(Vec3 v, Matrix m) {
            return new Vec4(v.x * m.m11 + v.y * m.m21 + v.z * m.m31 + m.m41,
                            v.x * m.m12 + v.y * m.m22 + v.z * m.m32 + m.m42,
                            v.x * m.m13 + v.y * m.m23 + v.z * m.m33 + m.m43,
                            v.x * m.m14 + v.y * m.m24 + v.z * m.m34 + m.m44);
        }

        public static Vec3 TransformPosition3(Vec3 v, Matrix m)
        {
            float x = v.x * m.m11 + v.y * m.m21 + v.z * m.m31 + m.m41;
            float y = v.x * m.m12 + v.y * m.m22 + v.z * m.m32 + m.m42;
            float z = v.x * m.m13 + v.y * m.m23 + v.z * m.m33 + m.m43;
            float w = v.x * m.m14 + v.y * m.m24 + v.z * m.m34 + m.m44;
            if (w != 0 && w != 1) {
                x /= w;
                y /= w;
                z /= w;
            }
            return new Vec3(x, y, z);
        }

        // vectors may not be normalized after the transformation
        public static Vec4 TransformNormal(Vec3 v, Matrix m) {
            return new Vec4(v.x * m.m11 + v.y * m.m21 + v.z * m.m31,
                            v.x * m.m12 + v.y * m.m22 + v.z * m.m32,
                            v.x * m.m13 + v.y * m.m23 + v.z * m.m33,
                            v.x * m.m14 + v.y * m.m24 + v.z * m.m34);
        }

        // vectors may not be normalized after the transformation
        public static Vec3 TransformNormal3(Vec3 v, Matrix m)
        {
            float x = v.x * m.m11 + v.y * m.m21 + v.z * m.m31;
            float y = v.x * m.m12 + v.y * m.m22 + v.z * m.m32;
            float z = v.x * m.m13 + v.y * m.m23 + v.z * m.m33;
            float w = v.x * m.m14 + v.y * m.m24 + v.z * m.m34;
            if (w != 0 && w != 1)
            {
                x /= w;
                y /= w;
                z /= w;
            }
            return new Vec3(x, y, z);
        }

        // Normalize after transformation
        // Does only work for orthogonal matrices
        public static Vec3 TransformNormal3n(Vec3 v, Matrix orthogonalTransform)
        {            
            float x = v.x * orthogonalTransform.m11 + v.y * orthogonalTransform.m21 + v.z * orthogonalTransform.m31;
            float y = v.x * orthogonalTransform.m12 + v.y * orthogonalTransform.m22 + v.z * orthogonalTransform.m32;
            float z = v.x * orthogonalTransform.m13 + v.y * orthogonalTransform.m23 + v.z * orthogonalTransform.m33;
            float w = v.x * orthogonalTransform.m14 + v.y * orthogonalTransform.m24 + v.z * orthogonalTransform.m34;
            if (w != 0 && w != 1) {
                x /= w;
                y /= w;
                z /= w;
            }
            float length = (float)Math.Sqrt(x * x + y * y + z * z);
            x /= length;
            y /= length;
            z /= length;
            return new Vec3(x, y, z);
        }

        //-------------- Overloaded operators -------------------
        public static Vec3 operator +(Vec3 lhs, Vec3 rhs) {
            return new Vec3(lhs.x + rhs.x, lhs.y + rhs.y, lhs.z + rhs.z);
        }

        public static Vec3 operator -(Vec3 v) {
            return new Vec3(-v.x, -v.y, -v.z);
        }

        public static Vec3 operator-(Vec3 lhs, Vec3 rhs) {
            return new Vec3(lhs.x - rhs.x, lhs.y - rhs.y, lhs.z - rhs.z);
        }

        public static Vec3 operator*(float scalar, Vec3 v) {
            return new Vec3(scalar * v.x, scalar * v.y, scalar * v.z);
        }

        public static Vec3 operator *(Vec3 v, float scalar) {
            return new Vec3(scalar * v.x, scalar * v.y, scalar * v.z);
        }

        public override string ToString() {
            return "(" + x + " / " + y + " / " + z + ")";
        }
        
    }
}
