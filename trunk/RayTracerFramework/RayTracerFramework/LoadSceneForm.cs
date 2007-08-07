using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using RayTracerFramework.RayTracer;
using RayTracerFramework.Loading;
using Blinn = RayTracerFramework.Shading;
using Photon = RayTracerFramework.PhotonMapping;
using System.IO;
using RayTracerFramework.Geometry;

namespace RayTracerFramework {
    public partial class LoadSceneForm : Form {
        private SceneManager sceneManager;
        public Scene scene;       
        public int resolutionX, resolutionY;

        public string sceneFileBaseDirectory = "../../Scenes/";
        public string standardSceneFilename = "standardscene.xml";
        public string selectedScene;

        public LoadSceneForm() {
            InitializeComponent();
            selectedScene = standardSceneFilename;
          
            sceneManager = new SceneManager();

            string[] sceneFiles = Directory.GetFiles(sceneFileBaseDirectory, "*.xml");
            foreach(string sceneFile in sceneFiles)
                cmbSceneName.Items.Add(Path.GetFileName(sceneFile));

            #region Load all cubemaps in the texture directory
            // Show all cubemaps in the texture directory
            /* Currently disabled since user input isn't accepted in the form
            string[] cubeMapFiles = Directory.GetFiles(Settings.Setup.Loading.DefaultStandardTextureDirectory, "cube_*.jpg");
            foreach (string s in cubeMapFiles) {
                string[] tokens = s.Split(new string[] { 
                    Settings.Setup.Loading.DefaultCubeMapPrefix, "." },
                    StringSplitOptions.None);
                string cubeMapFile = tokens[tokens.Length - 2];
                cubeMapFile = cubeMapFile.Substring(0, cubeMapFile.Length - 2);
                if (!cmbCubeMapFile.Items.Contains(cubeMapFile))
                    cmbCubeMapFile.Items.Add(cubeMapFile);
            }
            */
            #endregion

            cmbSceneName.SelectedItem = standardSceneFilename;
        }
          

        private void FillFormFromSceneFile(string sceneFile) {
            scene = sceneManager.LoadScene(sceneFileBaseDirectory + sceneFile);

            txtEyePosX.Text = scene.cam.eyePos.x.ToString();
            txtEyePosY.Text = scene.cam.eyePos.y.ToString();
            txtEyePosZ.Text = scene.cam.eyePos.z.ToString();

            txtLookAtX.Text = scene.cam.lookAtPos.x.ToString();
            txtLookAtY.Text = scene.cam.lookAtPos.y.ToString();
            txtLookAtZ.Text = scene.cam.lookAtPos.z.ToString();

            txtUpDirX.Text = scene.cam.upDir.x.ToString();
            txtUpDirY.Text = scene.cam.upDir.y.ToString();
            txtUpDirZ.Text = scene.cam.upDir.z.ToString();

            txtHFOV.Text = scene.cam.hFov.ToString();
            txtAspectRatio.Text = scene.cam.aspectRatio.ToString();

            txtTargetResolutionX.Text = sceneManager.resolutionX.ToString();
            txtTargetResolutionY.Text = sceneManager.resolutionY.ToString();   
           
            pnlBackgroundColor.BackColor = Color.FromArgb(scene.backgroundColor.RedInt,
                scene.backgroundColor.GreenInt, scene.backgroundColor.BlueInt);
            pictureBoxCubeMap.Image = scene.cubeMap.xMaxTexture.sourceBitmap;

            txtBackgroundTechnique.Text = scene.useCubeMap ? "Cube Map" : "Background Color";

            // Filling the three scene content tabs
            listBlinnLights.Items.Clear();
            foreach (Blinn.Light light in scene.lightManager.BlinnLightsWorldSpace) 
                listBlinnLights.Items.Add(light);
            if (listBlinnLights.Items.Count > 0)
                listBlinnLights.SelectedIndex = 0;

            listPhotonLights.Items.Clear();
            foreach (Photon.Light light in scene.lightManager.PhotonLightsWorldSpace)
                listPhotonLights.Items.Add(light);
            if (listPhotonLights.Items.Count > 0)
                listPhotonLights.SelectedIndex = 0;

            treeSceneObjects.Nodes.Clear();
            for (int i = 0; i < scene.kdTree.content.Count; i++) {
                IIntersectable obj = scene.kdTree.content[i];
                if (obj is Shading.DSphere) {
                    Shading.DSphere sphere = (Shading.DSphere)obj;
                    treeSceneObjects.Nodes.Add("Sphere");

                    Vec3 center = Vec3.TransformPosition3(new Vec3(0f, 0f, 0f), sphere.transform);
                    treeSceneObjects.Nodes[i].Nodes.Add("Center: " + center);
                    treeSceneObjects.Nodes[i].Nodes.Add("Radius: " + sphere.radius);
                    treeSceneObjects.Nodes[i].Nodes.Add("Textured: " + sphere.textured);
                    treeSceneObjects.Nodes[i].Nodes.Add("Material");
                    treeSceneObjects.Nodes[i].Nodes[3].Nodes.Add("Material Name: " + sphere.Material.name);
                    treeSceneObjects.Nodes[i].Nodes[3].Nodes.Add("Emissive Color: " + sphere.Material.emissive);
                    treeSceneObjects.Nodes[i].Nodes[3].Nodes.Add("Ambient Color: " + sphere.Material.ambient);
                    treeSceneObjects.Nodes[i].Nodes[3].Nodes.Add("Diffuse Color: " + sphere.Material.diffuse);
                    treeSceneObjects.Nodes[i].Nodes[3].Nodes.Add("Specular Color: " + sphere.Material.specular);
                    treeSceneObjects.Nodes[i].Nodes[3].Nodes.Add("Specular Power: " + sphere.Material.specularPower);
                    treeSceneObjects.Nodes[i].Nodes[3].Nodes.Add("Reflective: " + sphere.Material.reflective);
                    treeSceneObjects.Nodes[i].Nodes[3].Nodes.Add("Refractive: " + sphere.Material.refractive);
                    treeSceneObjects.Nodes[i].Nodes[3].Nodes.Add("Refraction Ratio: " + sphere.Material.refractionRatio);
                    treeSceneObjects.Nodes[i].Nodes[3].Nodes.Add("Refraction Rate: " + sphere.Material.refractionRate);
                    treeSceneObjects.Nodes[i].Nodes[3].Nodes.Add("Texture Name: " + sphere.Material.textureName);
                }
                else if(obj is Shading.DBox){
                    Shading.DBox box = (Shading.DBox)obj;
                    treeSceneObjects.Nodes.Add("Box");
                    treeSceneObjects.Nodes[i].Nodes.Add("Width: " + box.dx);
                    treeSceneObjects.Nodes[i].Nodes.Add("Height: " + box.dy);
                    treeSceneObjects.Nodes[i].Nodes.Add("Depth: " + box.dz);
                    treeSceneObjects.Nodes[i].Nodes.Add("Textured: " + box.textured);
                    treeSceneObjects.Nodes[i].Nodes.Add("Material");
                    treeSceneObjects.Nodes[i].Nodes[4].Nodes.Add("Material Name: " + box.Material.name);
                    treeSceneObjects.Nodes[i].Nodes[4].Nodes.Add("Emissive Color: " + box.Material.emissive);
                    treeSceneObjects.Nodes[i].Nodes[4].Nodes.Add("Ambient Color: " + box.Material.ambient);
                    treeSceneObjects.Nodes[i].Nodes[4].Nodes.Add("Diffuse Color: " + box.Material.diffuse);
                    treeSceneObjects.Nodes[i].Nodes[4].Nodes.Add("Specular Color: " + box.Material.specular);
                    treeSceneObjects.Nodes[i].Nodes[4].Nodes.Add("Specular Power: " + box.Material.specularPower);
                    treeSceneObjects.Nodes[i].Nodes[4].Nodes.Add("Reflective: " + box.Material.reflective);
                    treeSceneObjects.Nodes[i].Nodes[4].Nodes.Add("Refractive: " + box.Material.refractive);
                    treeSceneObjects.Nodes[i].Nodes[4].Nodes.Add("Refraction Ratio: " + box.Material.refractionRatio);
                    treeSceneObjects.Nodes[i].Nodes[4].Nodes.Add("Refraction Rate: " + box.Material.refractionRate);
                    treeSceneObjects.Nodes[i].Nodes[4].Nodes.Add("Texture Name: " + box.Material.textureName);
                } 
                else if (obj is Shading.DMesh) {
                    Shading.DMesh mesh = (Shading.DMesh)obj;
                    treeSceneObjects.Nodes.Add("Mesh");
                    treeSceneObjects.Nodes[i].Nodes.Add("Mesh Filename: " + mesh.meshFilename);
                }
            }
        }

        private void cmbSceneName_SelectedIndexChanged(object sender, EventArgs e) {
            selectedScene = (string)cmbSceneName.SelectedItem;
            FillFormFromSceneFile(selectedScene);
        }

        private void btnChange_Click(object sender, EventArgs e) {
            if (colorDialog.ShowDialog() == DialogResult.OK)
                pnlBackgroundColor.BackColor = colorDialog.Color;
        }

       

        

        private void listBlinnLights_SelectedIndexChanged(object sender, EventArgs e) {
            Blinn.Light light = (Blinn.Light)listBlinnLights.SelectedItem;

            txtBlinnAmbientR.Text = light.ambient.RedFloat.ToString();
            txtBlinnAmbientG.Text = light.ambient.GreenFloat.ToString();
            txtBlinnAmbientB.Text = light.ambient.BlueFloat.ToString();

            txtBlinnDiffuseR.Text = light.diffuse.RedFloat.ToString();
            txtBlinnDiffuseG.Text = light.diffuse.GreenFloat.ToString();
            txtBlinnDiffuseB.Text = light.diffuse.BlueFloat.ToString();

            txtBlinnSpecularR.Text = light.specular.RedFloat.ToString();
            txtBlinnSpecularG.Text = light.specular.GreenFloat.ToString();
            txtBlinnSpecularB.Text = light.specular.BlueFloat.ToString();

            pnlBlinnAmbient.BackColor = Color.FromArgb(light.ambient.RedInt,
                                                       light.ambient.GreenInt,
                                                       light.ambient.BlueInt);
            pnlBlinnDiffuse.BackColor = Color.FromArgb(light.diffuse.RedInt,
                                                       light.diffuse.GreenInt,
                                                       light.diffuse.BlueInt);
            pnlBlinnSpecular.BackColor = Color.FromArgb(light.specular.RedInt,
                                                       light.specular.GreenInt,
                                                       light.specular.BlueInt);

            if (light.lightType == Blinn.LightType.Point) {
                Blinn.PointLight pointLight = (Blinn.PointLight)light;
                txtBlinnPositionX.Text = pointLight.position.x.ToString();
                txtBlinnPositionY.Text = pointLight.position.y.ToString();
                txtBlinnPositionZ.Text = pointLight.position.z.ToString();

                txtBlinnPositionX.Enabled = txtBlinnPositionY.Enabled = txtBlinnPositionZ.Enabled = true;    
                txtBlinnDirectionX.Enabled = txtBlinnDirectionY.Enabled = txtBlinnDirectionZ.Enabled = false;
                txtBlinnDirectionX.Text = txtBlinnDirectionY.Text = txtBlinnDirectionZ.Text = "";
            } else if (light.lightType == Blinn.LightType.Directional) {
                Blinn.DirectionalLight directionalLight = (Blinn.DirectionalLight)light;
                txtBlinnDirectionX.Text = directionalLight.direction.x.ToString();
                txtBlinnDirectionY.Text = directionalLight.direction.y.ToString();
                txtBlinnDirectionZ.Text = directionalLight.direction.z.ToString();

                txtBlinnDirectionX.Enabled = txtBlinnDirectionY.Enabled = txtBlinnDirectionZ.Enabled = true;
                txtBlinnPositionX.Enabled = txtBlinnPositionY.Enabled = txtBlinnPositionZ.Enabled = false;
                txtBlinnPositionX.Text = txtBlinnPositionY.Text = txtBlinnPositionZ.Text = "";
            }
        }

        private void listPhotonLights_SelectedIndexChanged(object sender, EventArgs e) {
            Photon.Light light = (Photon.Light)listPhotonLights.SelectedItem;

            txtPhotonDiffuseR.Text = light.diffuse.RedInt.ToString();
            txtPhotonDiffuseG.Text = light.diffuse.GreenInt.ToString();
            txtPhotonDiffuseB.Text = light.diffuse.BlueInt.ToString();

            pnlPhotonDiffuse.BackColor = Color.FromArgb(light.diffuse.RedInt,
                                                        light.diffuse.BlueInt,
                                                        light.diffuse.GreenInt);
            txtPhotonPower.Text = light.power.ToString();

            if (light.lightType == Photon.LightType.Point) {
                Photon.PointLight pointLight = (Photon.PointLight)light;

                txtPhotonPositionX.Text = pointLight.position.x.ToString();
                txtPhotonPositionY.Text = pointLight.position.y.ToString();
                txtPhotonPositionZ.Text = pointLight.position.z.ToString();

                txtPhotonPositionX.Enabled = txtPhotonPositionY.Enabled = txtPhotonPositionZ.Enabled = true;
                txtPhotonNormalX.Text = txtPhotonNormalY.Text = txtPhotonNormalZ.Text = "";
                txtPhotonTangentX.Text = txtPhotonTangentY.Text = txtPhotonTangentZ.Text = "";
                txtPhotonBinormalX.Text = txtPhotonBinormalY.Text = txtPhotonBinormalZ.Text = "";
                
                txtPhotonTangentX.Enabled = txtPhotonTangentY.Enabled = txtPhotonTangentZ.Enabled = false;
                txtPhotonNormalX.Enabled = txtPhotonNormalY.Enabled = txtPhotonNormalZ.Enabled = false;
                txtPhotonBinormalX.Enabled = txtPhotonBinormalY.Enabled = txtPhotonBinormalZ.Enabled = false;
            } else if (light.lightType == Photon.LightType.Area) {
                Photon.AreaLight areaLight = (Photon.AreaLight)light;

                txtPhotonPositionX.Text = areaLight.topLeftPos.x.ToString();
                txtPhotonPositionY.Text = areaLight.topLeftPos.y.ToString();
                txtPhotonPositionZ.Text = areaLight.topLeftPos.z.ToString();

                txtPhotonNormalX.Text = areaLight.normal.x.ToString();
                txtPhotonNormalY.Text = areaLight.normal.y.ToString();
                txtPhotonNormalZ.Text = areaLight.normal.z.ToString();

                txtPhotonTangentX.Text = areaLight.tangent.x.ToString();
                txtPhotonTangentY.Text = areaLight.tangent.y.ToString();
                txtPhotonTangentZ.Text = areaLight.tangent.z.ToString();

                txtPhotonBinormalX.Text = areaLight.binormal.x.ToString();
                txtPhotonBinormalY.Text = areaLight.binormal.y.ToString();
                txtPhotonBinormalZ.Text = areaLight.binormal.z.ToString();

                txtPhotonTangentX.Enabled = txtPhotonTangentY.Enabled = txtPhotonTangentZ.Enabled = true;
                txtPhotonNormalX.Enabled = txtPhotonNormalY.Enabled = txtPhotonNormalZ.Enabled = true;
                txtPhotonBinormalX.Enabled = txtPhotonBinormalY.Enabled = txtPhotonBinormalZ.Enabled = true;
            }
        }

        private void treeSceneObjects_AfterSelect(object sender, TreeViewEventArgs e) {
            IIntersectable obj = (IIntersectable)treeSceneObjects.SelectedNode.Tag;
            if (obj is Shading.DSphere) {
            }
        }
    }
}