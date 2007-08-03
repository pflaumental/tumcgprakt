using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using RayTracerFramework.RayTracer;
using RayTracerFramework.Shading;
using RayTracerFramework.Geometry;
using System.Windows.Forms;
using RayTracerFramework.PhotonMapping;

using BlinnLight = RayTracerFramework.Shading.Light;
using PhotonLight = RayTracerFramework.PhotonMapping.Light;
using RayTracerFramework.Utility;

namespace RayTracerFramework.Loading {
    public class SceneManager {

        public string meshBaseDirectory = "../../Models/";
        public string textureBaseDirectory = "../../Textures/";

        public int resolutionX, resolutionY;
        public int globalPhotonCount;

        public SceneManager() { }

        public Scene LoadScene(string sceneFile) {
            XmlSerializer s = new XmlSerializer(typeof(SceneXML));
            TextReader reader = new StreamReader(sceneFile);
            SceneXML sceneXML = (SceneXML)s.Deserialize(reader);
            reader.Close();

            OBJLoader loader = new OBJLoader();
            loader.standardMeshDirectory = meshBaseDirectory;

            // Fill scene with the deserialized sceneXML
            Scene scene = new Scene();
            scene.photonTracer = null;
            scene.photonMap = null;
            resolutionX = (int)sceneXML.targetResolution.x;
            resolutionY = (int)sceneXML.targetResolution.y;
            globalPhotonCount = sceneXML.globalPhotonCount;

            scene.backgroundColor = sceneXML.backgroundColor;
            scene.cubeMap = new CubeMap(sceneXML.cubeMapScene.width,
                                        sceneXML.cubeMapScene.height,
                                        sceneXML.cubeMapScene.depth,
                                        sceneXML.cubeMapScene.cubeMapFilename);
            scene.useCubeMap = sceneXML.cubeMapScene.useCubeMap;
            PhotonMap.storedPhotonsCount = sceneXML.globalPhotonCount;
            scene.cam = sceneXML.camera;          
     
            // Place objects in the scene
            scene.kdTree = new GeoObjectKDTree();
            foreach (SceneObject obj in sceneXML.sceneObjects) {
                if (obj is SceneBox) {
                    SceneBox box = (SceneBox)obj;
                    Matrix transform = Matrix.GetScale(box.scaling) * Matrix.GetRotationX(box.rotation.x) *
                                       Matrix.GetRotationY(box.rotation.y) * Matrix.GetRotationZ(box.rotation.z) *
                                       Matrix.GetTranslation(box.position);
                    scene.AddDBox(transform, box.width, box.height, box.depth, box.textured, 
                        new Material(box.material.emissive, box.material.ambient, box.material.diffuse, 
                                     box.material.specular, box.material.specularPower, box.material.reflective, 
                                     box.material.refractive, box.material.refractionRatio, 
                                     box.material.refractionRate, box.material.textureName));
                }
                else if (obj is SceneSphere) {
                    SceneSphere sphere = (SceneSphere)obj;
                    scene.AddDSphere(sphere.center, sphere.radius, new Material(sphere.material.emissive, 
                        sphere.material.ambient, sphere.material.diffuse, sphere.material.specular,
                        sphere.material.specularPower, sphere.material.reflective, sphere.material.refractive,
                        sphere.material.refractionRatio, sphere.material.refractionRate, 
                        sphere.material.textureName));
                }
                else if (obj is SceneMesh) {
                    SceneMesh mesh = (SceneMesh)obj;
                    DMesh dmesh = loader.LoadFromFile(mesh.meshFilename);
                    scene.AddDMesh(dmesh, Matrix.GetScale(mesh.scaling) * Matrix.GetRotationX(mesh.rotation.x) *
                                          Matrix.GetRotationY(mesh.rotation.y) * Matrix.GetRotationZ(mesh.rotation.z) *
                                          Matrix.GetTranslation(mesh.position));
                }
            }

            // Place lights in the scene
            scene.lightManager = new LightManager();
            foreach (BlinnLight light in sceneXML.blinnLights) 
                scene.lightManager.AddBlinnWorldSpaceLight(light);
            
            foreach (PhotonLight light in sceneXML.photonLights) 
                scene.lightManager.AddPhotonWorldSpaceLight(light);

            scene.lightingModel = new BlinnPhongLightingModel();

            scene.backgroundColor = Color.LightSlateGray;
            scene.cubeMap = new CubeMap(100, 100, 100, "stpeters");
            scene.useCubeMap = true;

            // scene.refractionIndex = 1.0f;
            
            return scene;
        }


        public void SaveScene(string sceneFile, Scene scene, Vec2 targetResolution) {
            SceneXML sceneXML = new SceneXML();
            sceneXML.camera = scene.cam;
            sceneXML.targetResolution = targetResolution;
            sceneXML.backgroundColor = scene.backgroundColor;
            sceneXML.cubeMapScene = new CubeMapScene(scene.cubeMap.cubeMapFilename,
                scene.cubeMap.xMax - scene.cubeMap.xMin,
                scene.cubeMap.yMax - scene.cubeMap.yMin,
                scene.cubeMap.zMax - scene.cubeMap.zMin, scene.useCubeMap);

            sceneXML.globalPhotonCount = PhotonMap.storedPhotonsCount;

            foreach (BlinnLight light in scene.lightManager.BlinnLightsWorldSpace) 
                sceneXML.blinnLights.Add(light);
            
            foreach (PhotonLight light in scene.lightManager.PhotonLightsWorldSpace) 
                sceneXML.photonLights.Add(light);        

            foreach (IIntersectable obj in scene.kdTree.content) {
                if (obj is DBox) {
                    DBox dbox = (DBox)obj;
                    SceneBox box = new SceneBox(new Vec3(1f, 1f, 1f), Vec3.Zero, Vec3.Zero, 
                                                dbox.dx, dbox.dy, dbox.dz, dbox.textured, dbox.Material);
                    sceneXML.sceneObjects.Add(box);
                }
                else if (obj is DSphere) {
                    DSphere dsphere = (DSphere)obj;
                    Vec3 center = Vec3.TransformPosition3(new Vec3(0f, 0f, 0f), dsphere.transform);
                    SceneSphere sphere = new SceneSphere(center, dsphere.radius, dsphere.textured, dsphere.Material);
                    sceneXML.sceneObjects.Add(sphere);
                }
                else if (obj is DMesh) {
                    DMesh dmesh = (DMesh)obj;
                    SceneMesh mesh = new SceneMesh(dmesh.meshFilename, new Vec3(1f, 1f, 1f), Vec3.Zero, Vec3.Zero);
                    sceneXML.sceneObjects.Add(mesh);
                }
            }

            XmlSerializer s = new XmlSerializer(typeof(SceneXML));
            TextWriter writer = new StreamWriter(sceneFile);
            s.Serialize(writer, sceneXML);
            writer.Close();          
        }

    }


}
