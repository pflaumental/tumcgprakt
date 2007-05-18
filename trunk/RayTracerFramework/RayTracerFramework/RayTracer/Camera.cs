using System;
using System.Collections.Generic;
using System.Text;
using RayTracerFramework.Geometry;
using RayTracerFramework.Utility;

namespace RayTracerFramework.RayTracer {
    class Camera {
        public Vec3 eyePos;
        public Vec3 lookAtPos;
        public Vec3 upDir; // must be normalized
        public float hFov; // half horizontal view frustrum angle
        public float vFov; // half vertical view frustrum angle

        public Camera()
            : this(new Vec3(0, 0, -5), new Vec3(), Vec3.StdYAxis,
                   Trigonometric.PI_QUARTER,
                   Trigonometric.PI_QUARTER) { }

        public Camera(Vec3 eyePos, Vec3 lookAtPos, Vec3 upDir, float hFov, float vFov) {
            this.eyePos = eyePos;
            this.lookAtPos = lookAtPos;
            this.upDir = upDir;
            this.hFov = hFov;
            this.vFov = vFov;
        }

        public void AdjustVerticalFov(float aspectRatio) {
            vFov = hFov / aspectRatio;
        }

        public void AdjustHorizontalFov(float aspectRatio) {
            hFov = vFov * aspectRatio;
        }

        public Vec3 ViewDir {
            get { return Vec3.Normalize(lookAtPos - eyePos); }
        }

        public Matrix GetViewMatrix() {
            return Matrix.GetView(eyePos, lookAtPos, upDir);
        }
    }
}
