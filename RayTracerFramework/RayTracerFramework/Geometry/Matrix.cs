using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracerFramework.Geometry {
    

    // 4x4 row-major Matrix
    class Matrix {
        public float m11, m12, m13, m14;
        public float m21, m22, m23, m24;
        public float m31, m32, m33, m34;
        public float m41, m42, m43, m44;

        private static Matrix identity = new Matrix(1, 0, 0, 0, 
                                                    0, 1, 0, 0, 
                                                    0, 0, 1, 0, 
                                                    0, 0, 0, 1);
        private static Matrix zero = new Matrix();

        public Matrix():this(0, 0, 0, 0,
                             0, 0, 0, 0,
                             0, 0, 0, 0,
                             0, 0, 0, 0) { }

        public Matrix(float m11, float m12, float m13, float m14,
                      float m21, float m22, float m23, float m24,
                      float m31, float m32, float m33, float m34,
                      float m41, float m42, float m43, float m44) {
            this.m11 = m11;
            this.m12 = m12;
            this.m13 = m13;
            this.m14 = m14;
            this.m21 = m21;
            this.m22 = m22;
            this.m23 = m23;
            this.m24 = m24;
            this.m31 = m31;
            this.m32 = m32;
            this.m33 = m33;
            this.m34 = m34;
            this.m41 = m41;
            this.m42 = m42;
            this.m43 = m43;
            this.m44 = m44;   
        }

        public static Matrix operator+(Matrix m1, Matrix m2) {
            return new Matrix(m1.m11 + m2.m11, m1.m12 + m2.m12, m1.m13 + m2.m13, m1.m14 + m2.m14,
                              m1.m21 + m2.m21, m1.m22 + m2.m22, m1.m23 + m2.m23, m1.m24 + m2.m24,
                              m1.m31 + m2.m31, m1.m32 + m2.m32, m1.m33 + m2.m33, m1.m34 + m2.m34,
                              m1.m41 + m2.m41, m1.m42 + m2.m42, m1.m43 + m2.m43, m1.m44 + m2.m44);
        }

        public static Matrix operator *(Matrix a, Matrix b) {
            return new Matrix(a.m11 * b.m11 + a.m12 * b.m21 + a.m13 * b.m31 + a.m14 * b.m41,
                              a.m11 * b.m12 + a.m12 * b.m22 + a.m13 * b.m32 + a.m14 * b.m42,
                              a.m11 * b.m13 + a.m12 * b.m23 + a.m13 * b.m33 + a.m14 * b.m43,
                              a.m11 * b.m14 + a.m12 * b.m24 + a.m13 * b.m34 + a.m14 * b.m44,

                              a.m21 * b.m11 + a.m22 * b.m21 + a.m23 * b.m31 + a.m24 * b.m41,
                              a.m21 * b.m12 + a.m22 * b.m22 + a.m23 * b.m32 + a.m24 * b.m42,
                              a.m21 * b.m13 + a.m22 * b.m23 + a.m23 * b.m33 + a.m24 * b.m43,
                              a.m21 * b.m14 + a.m22 * b.m24 + a.m23 * b.m34 + a.m24 * b.m44,

                              a.m31 * b.m11 + a.m32 * b.m21 + a.m33 * b.m31 + a.m34 * b.m41,
                              a.m31 * b.m12 + a.m32 * b.m22 + a.m33 * b.m32 + a.m34 * b.m42,
                              a.m31 * b.m13 + a.m32 * b.m23 + a.m33 * b.m33 + a.m34 * b.m43,
                              a.m31 * b.m14 + a.m32 * b.m24 + a.m33 * b.m34 + a.m34 * b.m44,

                              a.m41 * b.m11 + a.m42 * b.m21 + a.m43 * b.m31 + a.m44 * b.m41,
                              a.m41 * b.m12 + a.m42 * b.m22 + a.m43 * b.m32 + a.m44 * b.m42,
                              a.m41 * b.m13 + a.m42 * b.m23 + a.m43 * b.m33 + a.m44 * b.m43,
                              a.m41 * b.m14 + a.m42 * b.m24 + a.m43 * b.m34 + a.m44 * b.m44);
        }

        // Creates an rotation matrix about the x-axis with angle measured in radians
        public static Matrix GetRotationX(float angle) {
            float sinAngle = (float)Math.Sin(angle);
            float cosAngle = (float)Math.Cos(angle);

            return new Matrix(1, 0, 0, 0,
                              0, cosAngle, sinAngle, 0,
                              0, -sinAngle, cosAngle, 0,
                              0, 0, 0, 1);
        }

        public static Matrix GetRotationY(float angle) {
            float sinAngle = (float)Math.Sin(angle);
            float cosAngle = (float)Math.Cos(angle);

            return new Matrix(cosAngle, 0, -sinAngle, 0,
                              0, 1, 0, 0,
                              sinAngle, 0, cosAngle, 0,
                              0, 0, 0, 1);
        }

        public static Matrix GetRotationZ(float angle) {
            float sinAngle = (float)Math.Sin(angle);
            float cosAngle = (float)Math.Cos(angle);

            return new Matrix(cosAngle, sinAngle, 0, 0,
                              -sinAngle, cosAngle, 0, 0,
                              0, 0, 1, 0,
                              0, 0, 0, 1);
        }

        // Axis must be normalized
        public static Matrix GetRotationAxis(Vec3 axis, float angle) {
            float sinAngle = (float)Math.Sin(angle);
            float cosAngle = (float)Math.Cos(angle);
            float invCosAngle = 1 - cosAngle;
            float x = axis.x, y = axis.y, z = axis.z;

            return new Matrix(x * x * invCosAngle + cosAngle, x * y * invCosAngle + z * sinAngle, x * z * invCosAngle - y * sinAngle, 0,
                              x * y * invCosAngle - z * sinAngle, y * y * invCosAngle + cosAngle, y * z * invCosAngle + x * sinAngle, 0,
                              x * z * invCosAngle + y * sinAngle, y * z * invCosAngle - x * sinAngle, z * z * invCosAngle + cosAngle, 0,
                              0, 0, 0, 1);
        }

        public static Matrix GetTranslation(float x, float y, float z) {
            return new Matrix(1, 0, 0, 0,
                              0, 1, 0, 0,
                              0, 0, 1, 0,
                              x, y, z, 1);
        }

        public static Matrix GetTranslation(Vec3 v) {
            return new Matrix(1, 0, 0, 0,
                             0, 1, 0, 0,
                             0, 0, 1, 0,
                             v.x, v.y, v.z, 1);
        }

        public static Matrix GetScale(float x, float y, float z) {
            return new Matrix(x, 0, 0, 0,
                              0, y, 0, 0,
                              0, 0, z, 0,
                              0, 0, 0, 1);
        }

        public static Matrix GetScale(Vec3 v) {
            return new Matrix(v.x, 0, 0, 0,
                              0, v.y, 0, 0,
                              0, 0, v.z, 0,
                              0, 0,  0,  1);
        }

        public static Matrix GetScale(float scale) {
            return new Matrix(scale, 0, 0, 0,
                              0, scale, 0, 0,
                              0, 0, scale, 0,
                              0, 0,   0,   1);
        }

        public static Matrix GetView( Vec3 eye, Vec3 target, Vec3 up) {
            Vec3 camZAxis = Vec3.Normalize(target - eye);
            Vec3 camXAxis = Vec3.Normalize(Vec3.Cross(up, camZAxis));
            Vec3 camYAxis = Vec3.Cross(camZAxis, camXAxis);
            return new Matrix(camXAxis.x, camYAxis.x, camZAxis.x, 0,
                              camXAxis.y, camYAxis.y, camZAxis.y, 0,
                              camXAxis.z, camYAxis.z, camZAxis.z, 0,
                              -Vec3.Dot(camXAxis, eye), -Vec3.Dot(camYAxis, eye), -Vec3.Dot(camZAxis, eye), 1);
        }

        public static Matrix Identity {
            get {
                return identity; 
            }
        }

        public static Matrix Zero {
            get { 
                return zero;
            }
        }


    }
}
