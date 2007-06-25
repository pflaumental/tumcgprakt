using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracerFramework.Shading {
    
    class LightManager {
        private List<Shading.Light> blinnlightsWorldSpace;
        private List<PhotonMapping.Light> photonlightsWorldSpace;

        public LightManager() {
            this.blinnlightsWorldSpace = new List<Shading.Light>();
            this.photonlightsWorldSpace = new List<PhotonMapping.Light>();
        }

        public void AddBlinnWorldSpaceLight(Shading.Light lightWorldSpace) {
            this.blinnlightsWorldSpace.Add(lightWorldSpace);
        }

        public void AddPhotonWorldSpaceLight(PhotonMapping.Light lightWorldSpace) {
            this.photonlightsWorldSpace.Add(lightWorldSpace);
        }

        public List<Light> BlinnLightsWorldSpace {
            get { return blinnlightsWorldSpace; }
        }

        public List<PhotonMapping.Light> PhotonLightsWorldSpace {
            get { return photonlightsWorldSpace; }
        }
    }
}
