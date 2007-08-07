using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracerFramework.Settings.Setup {
    public static class Scene {
        public static float FogAmbientLightAmplifier = 10f;
        public static RayTracerFramework.Shading.Color StdFogColor = new RayTracerFramework.Shading.Color(0.5f, 0.5f, 0.5f);
    }

    public static class GeoObjectKDTree {
        public static int DefaultMaxDesiredObjectsPerLeafCount = 2;
    }

    public static class KDTree {
        public static int DefaultMaxHeight = 25;
        public static int DefaultMaxDesiredObjectsPerLeafCount = 12;
        public static float DefaultWeightDivisionQuality = 0f;
        public static float DefaultWeightSum = 1f - DefaultWeightDivisionQuality;
    }

    public static class PhotonMapping {
        public static bool EmitPhotons = false;
        public static int StoredPhotonsCount = 200000;
        public static float PowerLevel = 5f;
        public static int TracingMaxRecursionDepth = 4;
        public static bool MediumIsParticipating = false;
        public static float MediumLightingDensity = 0.22f;   // estimated number of photons stored on a each ray on each unit length
    }

    public static class Loading {
        public static string DefaultStandardMeshDirectory = "../../Models/";
        public static string DefaultStandardTextureDirectory = "../../Textures/";
        public static string DefaultCubeMapName = "stpeters";
        public static string DefaultCubeMapPrefix = "cube_";
    }
}
