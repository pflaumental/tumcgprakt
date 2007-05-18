using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracerFramework.Geometry {

    class Vec2 {
        public float x, y;
        public static readonly Vec2 StdXAxis = new Vec2(1, 0);
        public static readonly Vec2 StdYAxis = new Vec2(0, 1);

        public Vec2() : this(0, 0) { }

        public Vec2(float x, float y) {
            this.x = x;
            this.y = y;
        }

        public static float Dot(Vec2 left, Vec2 right) {
            return left.x * left.y + left.y * right.y;
        }

        public float Length {
            get {
                return (float)Math.Sqrt(x * x + y * y);
            }
        }

        public float LengthSq {
            get {
                return x * x + y * y;
            }
        }

        public static Vec2 Lerp(Vec2 left, Vec2 right, float interpolator) {
            return left + interpolator * (right - left);
        }

        public static Vec2 Normalize(Vec2 v) {
            float length = (float)Math.Sqrt(v.x * v.x + v.y * v.y);
            return new Vec2(v.x / length, v.y / length);
        }

        public void Normalize() {
            float length = (float)Math.Sqrt(x * x + y * y);
            x /= length;
            y /= length;
        }        

        //-------------- Overloaded operators -------------------
        public static Vec2 operator +(Vec2 lhs, Vec2 rhs) {
            return new Vec2(lhs.x + rhs.x, lhs.y + rhs.y);
        }

        public static Vec2 operator -(Vec2 v) {
            return new Vec2(-v.x, -v.y);
        }

        public static Vec2 operator-(Vec2 lhs, Vec2 rhs) {
            return new Vec2(lhs.x - rhs.x, lhs.y - rhs.y);
        }

        public static Vec2 operator*(float scalar, Vec2 v) {
            return new Vec2(scalar * v.x, scalar * v.y);
        }

        public static Vec2 operator *(Vec2 v, float scalar) {
            return new Vec2(scalar * v.x, scalar * v.y);
        }

        public override string ToString() {
            return "(" + x + ", " + y + ")";
        }
        
    }
}
