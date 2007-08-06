using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracerFramework.Settings.Render {
    public static class Renderer {
        public static int MaxRecursionDepth = 10;
    }
    public static class PhotonMapping {
        public static bool RenderSurfacePhotons = true;
        public static float LocalScaleDown = 0.5f;
        public static float SphereRadius = 0.8f;
        public static float SphereRadiusSq = SphereRadius * SphereRadius;
        public static float ConeFilterConstantK = 1.5f;//1.6

        public static bool RenderMediumPhotons = false;
        public static float CapsuleRadius = 0.25f;
        public static float CapsuleRadiusSq = CapsuleRadius * CapsuleRadius;
        public static float MediumEnlightmentAmplifier = 2f;
        public static float MediumConeFilterConstantK = 1.5f;
    }

    public static class StdShading {
        public static bool enableFog = false;
        public static float FogLevel = 0.3f;
        public static float LocalThreshold = 0.02f;
        public static float ContributionThreshold = 0.005f;
    }

    public static class DPoint {
        public static float Epsilon = 0.2f;
        public static float EpsilonSq = Epsilon * Epsilon;
    }

    public static class Ray {
        public readonly static float PositionEpsilon = 0.001f;
    }

    public static class Trigonometric {
        public static float Pi = (float)Math.PI;
        public static float TwoPi = Pi * 2;
        public static float PiHalf = Pi / 2;
        public static float PiQuarter = Pi / 4;
        public static float Epsilon = 0.000001f;
    }
}
