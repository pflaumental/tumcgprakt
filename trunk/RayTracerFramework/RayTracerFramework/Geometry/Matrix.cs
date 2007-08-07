using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracerFramework.Geometry {
    

    // 4x4 row-major Matrix
    public class Matrix {
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

        public Matrix(Matrix other) {
            this.m11 = other.m11;
            this.m12 = other.m12;
            this.m13 = other.m13;
            this.m14 = other.m14;
            this.m21 = other.m21;
            this.m22 = other.m22;
            this.m23 = other.m23;
            this.m24 = other.m24;
            this.m31 = other.m31;
            this.m32 = other.m32;
            this.m33 = other.m33;
            this.m34 = other.m34;
            this.m41 = other.m41;
            this.m42 = other.m42;
            this.m43 = other.m43;
            this.m44 = other.m44;
        }

        public static Matrix operator *(Matrix m, float skalar) {
            return new Matrix(skalar * m.m11, skalar * m.m12, skalar * m.m13, skalar * m.m14,
                      skalar * m.m21, skalar * m.m22, skalar * m.m23, skalar * m.m24,
                      skalar * m.m31, skalar * m.m32, skalar * m.m33, skalar * m.m34,
                      skalar * m.m41, skalar * m.m42, skalar * m.m43, skalar * m.m44);
        }

        public static Matrix operator *(float skalar, Matrix m)
        {
            return new Matrix(skalar * m.m11, skalar * m.m12, skalar * m.m13, skalar * m.m14,
                      skalar * m.m21, skalar * m.m22, skalar * m.m23, skalar * m.m24,
                      skalar * m.m31, skalar * m.m32, skalar * m.m33, skalar * m.m34,
                      skalar * m.m41, skalar * m.m42, skalar * m.m43, skalar * m.m44);
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

        //public static Matrix GetView( Vec3 eye, Vec3 target, Vec3 up) {
        //    Vec3 camZAxis = Vec3.Normalize(target - eye);
        //    Vec3 camXAxis = Vec3.Normalize(Vec3.Cross(up, camZAxis));
        //    Vec3 camYAxis = Vec3.Cross(camZAxis, camXAxis);
        //    return new Matrix(camXAxis.x, camYAxis.x, camZAxis.x, 0,
        //                      camXAxis.y, camYAxis.y, camZAxis.y, 0,
        //                      camXAxis.z, camYAxis.z, camZAxis.z, 0,
        //                      -Vec3.Dot(camXAxis, eye), -Vec3.Dot(camYAxis, eye), -Vec3.Dot(camZAxis, eye), 1);
        //}

        //public static Matrix GetInverseView(Vec3 eye, Vec3 target, Vec3 up) {
        //    Vec3 camZAxis = Vec3.Normalize(target - eye);
        //    Vec3 camXAxis = Vec3.Normalize(Vec3.Cross(up, camZAxis));
        //    Vec3 camYAxis = Vec3.Cross(camZAxis, camXAxis);
        //    return new Matrix(camXAxis.x, camXAxis.y, camXAxis.z, 0,
        //                      camYAxis.x, camYAxis.y, camYAxis.z, 0,
        //                      camZAxis.x, camZAxis.y, camZAxis.z, 0,
        //                      eye.x, eye.y, eye.z, 1);
        //}

        public static Matrix Invert(Matrix m) {
            float[] tmp = new float[12];
            Matrix result = new Matrix();
            Matrix mT = Matrix.Transpose(m);

            // Calculate pairs for first 8 elements (cofactors) 
            tmp[0] = mT.m33 * mT.m44;
            tmp[1] = mT.m34 * mT.m43;
            tmp[2] = mT.m32 * mT.m44;
            tmp[3] = mT.m34 * mT.m42;
            tmp[4] = mT.m32 * mT.m43;
            tmp[5] = mT.m33 * mT.m42;
            tmp[6] = mT.m31 * mT.m44;
            tmp[7] = mT.m34 * mT.m41;
            tmp[8] = mT.m31 * mT.m43;
            tmp[9] = mT.m33 * mT.m41;
            tmp[10] = mT.m31 * mT.m42;
            tmp[11] = mT.m32 * mT.m41;

            result.m11 = tmp[0] * mT.m22 + tmp[3] * mT.m23 + tmp[4] * mT.m24;
            result.m11 -= tmp[1] * mT.m22 + tmp[2] * mT.m23 + tmp[5] * mT.m24;
            result.m12 = tmp[1] * mT.m21 + tmp[6] * mT.m23 + tmp[9] * mT.m24;
            result.m12 -= tmp[0] * mT.m21 + tmp[7] * mT.m23 + tmp[8] * mT.m24;
            result.m13 = tmp[2] * mT.m21 + tmp[7] * mT.m22 + tmp[10] * mT.m24;
            result.m13 -= tmp[3] * mT.m21 + tmp[6] * mT.m22 + tmp[11] * mT.m24;
            result.m14 = tmp[5] * mT.m21 + tmp[8] * mT.m22 + tmp[11] * mT.m23;
            result.m14 -= tmp[4] * mT.m21 + tmp[9] * mT.m22 + tmp[10] * mT.m23;
            result.m21 = tmp[1] * mT.m12 + tmp[2] * mT.m13 + tmp[5] * mT.m14;
            result.m21 -= tmp[0] * mT.m12 + tmp[3] * mT.m13 + tmp[4] * mT.m14;
            result.m22 = tmp[0] * mT.m11 + tmp[7] * mT.m13 + tmp[8] * mT.m14;
            result.m22 -= tmp[1] * mT.m11 + tmp[6] * mT.m13 + tmp[9] * mT.m14;
            result.m23 = tmp[3] * mT.m11 + tmp[6] * mT.m12 + tmp[11] * mT.m14;
            result.m23 -= tmp[2] * mT.m11 + tmp[7] * mT.m12 + tmp[10] * mT.m14;
            result.m24 = tmp[4] * mT.m11 + tmp[9] * mT.m12 + tmp[10] * mT.m13;
            result.m24 -= tmp[5] * mT.m11 + tmp[8] * mT.m12 + tmp[11] * mT.m13;

            // Calculate pairs for second 8 elements (cofactors)
            tmp[0] = mT.m13 * mT.m24;
            tmp[1] = mT.m14 * mT.m23;
            tmp[2] = mT.m12 * mT.m24;
            tmp[3] = mT.m14 * mT.m22;
            tmp[4] = mT.m12 * mT.m23;
            tmp[5] = mT.m13 * mT.m22;
            tmp[6] = mT.m11 * mT.m24;
            tmp[7] = mT.m14 * mT.m21;
            tmp[8] = mT.m11 * mT.m23;
            tmp[9] = mT.m13 * mT.m21;
            tmp[10] = mT.m11 * mT.m22;
            tmp[11] = mT.m12 * mT.m21;

            result.m31 = tmp[0] * mT.m42 + tmp[3] * mT.m43 + tmp[4] * mT.m44;
            result.m31 -= tmp[1] * mT.m42 + tmp[2] * mT.m43 + tmp[5] * mT.m44;
            result.m32 = tmp[1] * mT.m41 + tmp[6] * mT.m43 + tmp[9] * mT.m44;
            result.m32 -= tmp[0] * mT.m41 + tmp[7] * mT.m43 + tmp[8] * mT.m44;
            result.m33 = tmp[2] * mT.m41 + tmp[7] * mT.m42 + tmp[10] * mT.m44;
            result.m33 -= tmp[3] * mT.m41 + tmp[6] * mT.m42 + tmp[11] * mT.m44;
            result.m34 = tmp[5] * mT.m41 + tmp[8] * mT.m42 + tmp[11] * mT.m43;
            result.m34 -= tmp[4] * mT.m41 + tmp[9] * mT.m42 + tmp[10] * mT.m43;
            result.m41 = tmp[2] * mT.m33 + tmp[5] * mT.m34 + tmp[1] * mT.m32;
            result.m41 -= tmp[4] * mT.m34 + tmp[0] * mT.m32 + tmp[3] * mT.m33;
            result.m42 = tmp[8] * mT.m34 + tmp[0] * mT.m31 + tmp[7] * mT.m33;
            result.m42 -= tmp[6] * mT.m33 + tmp[9] * mT.m34 + tmp[1] * mT.m31;
            result.m43 = tmp[6] * mT.m32 + tmp[11] * mT.m34 + tmp[3] * mT.m31;
            result.m43 -= tmp[10] * mT.m34 + tmp[2] * mT.m31 + tmp[7] * mT.m32;
            result.m44 = tmp[10] * mT.m33 + tmp[4] * mT.m31 + tmp[9] * mT.m32;
            result.m44 -= tmp[8] * mT.m32 + tmp[11] * mT.m33 + tmp[5] * mT.m31;
            
            // Calculate determinant
            float det = mT.m11 * result.m11 + mT.m12 * result.m12 + mT.m13 * result.m13 + mT.m14 * result.m14;
            
            // Calculate matrix inverse
            float detInv = 1.0f / det;
            return result * detInv;
        }

        public static Matrix Transpose(Matrix m) {
            return new Matrix(m.m11, m.m21, m.m31, m.m41, 
                              m.m12, m.m22, m.m32, m.m42, 
                              m.m13, m.m23, m.m33, m.m43, 
                              m.m14, m.m24, m.m34, m.m44);
        }

        public static Matrix Identity {
            get {
                return new Matrix(identity); 
            }
        }

        public static Matrix Zero {
            get { 
                return new Matrix(zero);
            }
        }

        public Matrix Inverse() {
            return Invert(this);
        }

        public Matrix Transposition() {
            return Transpose(this);
        }

    }
}
