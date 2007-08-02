using System;
using System.Collections.Generic;
using System.Text;
using RayTracerFramework.Geometry;
using RayTracerFramework.Utility;
using System.Xml.Serialization;

namespace RayTracerFramework.RayTracer {
    public class Camera {

        [XmlElement("EyePos")]
        public Vec3 eyePos;

        [XmlElement("LookAtPos")]
        public Vec3 lookAtPos;

        [XmlElement("UpDir")]
        public Vec3 upDir; // must be normalized

        [XmlElement("HalfHorizontalFOV")]
        public float hFov; // half horizontal view frustrum angle

        [XmlElement("AspectRatio")]
        public float aspectRatio;
        //public float vFov; // half vertical view frustrum angle

        public Camera()
            : this(new Vec3(0, 0, -5), Vec3.Zero, Vec3.StdYAxis,
                   Trigonometric.PI_QUARTER, 1) { }

        public Camera(Vec3 eyePos, Vec3 lookAtPos, Vec3 upDir, float hFov, float aspectRatio) {
            this.eyePos = eyePos;
            this.lookAtPos = lookAtPos;
            this.upDir = upDir;
            this.hFov = hFov;
            this.aspectRatio = aspectRatio;
        }

        /*
        public void AdjustVerticalFov(float aspectRatio) {
            vFov = hFov / aspectRatio;
        }

        public void AdjustHorizontalFov(float aspectRatio) {
            hFov = vFov * aspectRatio;
        }
        */

        public float GetViewPlaneWidth() {
            return (float)Math.Tan(hFov) * 2;
        }

        public float GetViewPlaneHeight() {
            return (float)(Math.Tan(hFov) * 2) / aspectRatio;
        }

        public Vec3 ViewDir {
            get { return Vec3.Normalize(lookAtPos - eyePos); }
        }

        public Matrix GetViewMatrix() {
            return Matrix.GetView(eyePos, lookAtPos, upDir);
        }

        public Matrix GetInverseViewMatrix() {
            return Matrix.GetInverseView(eyePos, lookAtPos, upDir);
        }
    }
}
