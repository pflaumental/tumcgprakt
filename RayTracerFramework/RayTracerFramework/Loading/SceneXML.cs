using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using RayTracerFramework.Geometry;
using RayTracerFramework.RayTracer;

using BlinnLight = RayTracerFramework.Shading.Light;
using PhotonLight = RayTracerFramework.PhotonMapping.Light;
using Material = RayTracerFramework.Shading.Material;
using RayTracerFramework.Shading;

namespace RayTracerFramework.Loading {
    [XmlRoot("Scene")]
    public class SceneXML {
        public SceneXML() { }

        [XmlElement("Camera")]
        public Camera camera;

        [XmlElement("TargetResolution")]
        public Vec2 targetResolution;

        [XmlElement("BackgroundColor")]
        public Color backgroundColor;

        [XmlElement("CubeMap")]
        public CubeMapScene cubeMapScene;

        [XmlElement("GlobalPhotonCount")]
        public int globalPhotonCount;

        [XmlArray(ElementName = "BlinnLights"), XmlArrayItem(ElementName = "Light", Type = typeof(BlinnLight))]
        public List<BlinnLight> blinnLights = new List<BlinnLight>();
        
        [XmlArray(ElementName = "PhotonLights"), XmlArrayItem(ElementName = "Light", Type = typeof(PhotonLight))]
        public List<PhotonLight> photonLights = new List<PhotonLight>();

        [XmlArray(ElementName = "SceneObjects"), XmlArrayItem(ElementName = "SceneObject", Type = typeof(SceneObject))]
        public List<SceneObject> sceneObjects = new List<SceneObject>();

    }

    public class CubeMapScene {
        [XmlElement("CubeMapFilename")]
        public string cubeMapFilename;

        [XmlElement("Width")]
        public float width;

        [XmlElement("Height")]
        public float height;

        [XmlElement("Depth")]
        public float depth;

        [XmlElement("UseCubeMap")]
        public bool useCubeMap;

        public CubeMapScene() { }

        public CubeMapScene(string cubeMapFilename, 
            float width, float height, float depth,
            bool useCubeMap) {
            this.cubeMapFilename = cubeMapFilename;
            this.width = width;
            this.height = height;
            this.depth = depth;
            this.useCubeMap = useCubeMap;
        }
    }

    [XmlInclude(typeof(SceneBox))]
    [XmlInclude(typeof(SceneSphere))]
    [XmlInclude(typeof(SceneMesh))]
    public abstract class SceneObject { }

    [XmlType("Box")]
    public class SceneBox : SceneObject {
        [XmlElement("Scaling")]
        public Vec3 scaling;

        [XmlElement("Rotation")]
        public Vec3 rotation;

        [XmlElement("Position")]
        public Vec3 position;

        [XmlElement("Width")]
        public float width;

        [XmlElement("Height")]
        public float height;

        [XmlElement("Depth")]
        public float depth;

        [XmlElement("Textured")]
        public bool textured;

        [XmlElement("Material")]
        public Material material;

        public SceneBox() { }

        public SceneBox(Vec3 scaling, Vec3 rotation, Vec3 position, 
                        float width, float height, float depth, bool textured, Material material) {
            this.scaling = scaling;
            this.rotation = rotation;
            this.position = position;
            this.width = width;
            this.height = height;
            this.depth = depth;
            this.textured = textured;
            this.material = material;
        }   
    }

    [XmlType("Sphere")]
    public class SceneSphere : SceneObject {
        [XmlElement("Center")]
        public Vec3 center;

        [XmlElement("Radius")]
        public float radius;

        [XmlElement("Textured")]
        public bool textured;

        [XmlElement("Material")]
        public Material material;

        public SceneSphere() { }

        public SceneSphere(Vec3 center, float radius, bool textured, Material material) {
            this.center = center;
            this.radius = radius;
            this.textured = textured;
            this.material = material;   
        } 
    }

    [XmlType("Mesh")]
    public class SceneMesh : SceneObject {
        [XmlElement("MeshFilename")]
        public string meshFilename;

        [XmlElement("Scaling")]
        public Vec3 scaling;

        [XmlElement("Rotation")]
        public Vec3 rotation;

        [XmlElement("Position")]
        public Vec3 position;

        public SceneMesh() { }

        public SceneMesh(string meshFilename, Vec3 scaling, Vec3 rotation, Vec3 position) {
            this.meshFilename = meshFilename;
            this.scaling = scaling;
            this.rotation = rotation;
            this.position = position;
        }
    }
    

}
