using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using RayTracerFramework.Shading;
using System.Globalization;
using RayTracerFramework.Utility;
using System.Drawing;
using RayTracerFramework.Geometry;

namespace RayTracerFramework.Loading {

    // This class loads meshes stored in obj files. Only twodimensional texture coordinates and
    // triangles are allowed.
    // Currently only positive indices are supported
    class OBJLoader : IMeshLoader {

        public string standardMeshDirectory = "../../Models/";

        public DMesh LoadFromFile(string filename) {
            DMesh mesh = new DMesh();

            StreamReader reader = new StreamReader(standardMeshDirectory + filename);
            Regex regex = new Regex(@"\s+");

            List<Vec3> vertices = new List<Vec3>();
            List<Vec3> normals = new List<Vec3>();
            List<Vec3> texCoords = new List<Vec3>();

            Dictionary<string, DMeshSubset> meshSubsets = new Dictionary<string,DMeshSubset>();
            DMeshSubset defaultSubset = new DMeshSubset();
            DMeshSubset currentSubset = defaultSubset;

            while (!reader.EndOfStream) {        
                string[] tokens = regex.Split(reader.ReadLine());

                switch (tokens[0]) {
                    case "mtllib":
                        LoadMaterial(tokens[1], meshSubsets);
                        break;
                    case "usemtl":
                        try {
                            currentSubset = meshSubsets[tokens[1]];
                        } 
                        catch (KeyNotFoundException) {
                            throw new Exception("The material \"" + tokens[1] + "\" could not be found " +
                                                "in the material library.");
                        }
                            break;
                    case "v":
                        vertices.Add(new Vec3(float.Parse(tokens[1], CultureInfo.CreateSpecificCulture("en-us")),
                                              float.Parse(tokens[2], CultureInfo.CreateSpecificCulture("en-us")),
                                              float.Parse(tokens[3], CultureInfo.CreateSpecificCulture("en-us"))));
                        break;
                    case "vt":
                        texCoords.Add(new Vec3(float.Parse(tokens[1], CultureInfo.CreateSpecificCulture("en-us")),
                                               float.Parse(tokens[2], CultureInfo.CreateSpecificCulture("en-us")), 0));
                        break;
                    case "vn":
                        normals.Add(new Vec3(float.Parse(tokens[1], CultureInfo.CreateSpecificCulture("en-us")),
                                             float.Parse(tokens[2], CultureInfo.CreateSpecificCulture("en-us")),
                                             float.Parse(tokens[3], CultureInfo.CreateSpecificCulture("en-us"))));
                        break;
                    case "f":  // triangle definiton
                        string[] v1Tokens = tokens[1].Split('/');
                        string[] v2Tokens = tokens[2].Split('/');
                        string[] v3Tokens = tokens[3].Split('/');
                        
                        // Extract positions
                        Vec3 p1 = vertices[Int32.Parse(v1Tokens[0]) - 1];
                        Vec3 p2 = vertices[Int32.Parse(v2Tokens[0]) - 1];
                        Vec3 p3 = vertices[Int32.Parse(v3Tokens[0]) - 1];

                        // Extract texture coordinates
                        Vec3 t1, t2, t3;
                        if (v1Tokens[1] == "") 
                            t1 = t2 = t3 = null;
                        else {
                            t1 = texCoords[Int32.Parse(v1Tokens[1]) - 1];
                            t2 = texCoords[Int32.Parse(v2Tokens[1]) - 1];
                            t3 = texCoords[Int32.Parse(v3Tokens[1]) - 1];
                        }

                        // Extract normals
                        Vec3 n1, n2, n3;
                        if (v1Tokens[2] == "")
                            n1 = n2 = n3 = null;
                        else {
                            n1 = normals[Int32.Parse(v1Tokens[2]) - 1];
                            n2 = normals[Int32.Parse(v2Tokens[2]) - 1];
                            n3 = normals[Int32.Parse(v3Tokens[2]) - 1];
                        }
                        
                        currentSubset.triangles.Add(new Triangle(p1, p2, p3, n1, n2, n3, t1, t2, t3));
                        break;
                }               
            }
            reader.Close();

            if (defaultSubset.triangles.Count > 0)
                mesh.AddSubset(defaultSubset);
            foreach (DMeshSubset materialGroup in meshSubsets.Values) {
                if (materialGroup.triangles.Count > 0) {
                    mesh.AddSubset(materialGroup);
                }
            }
            return mesh;
        }


        private void LoadMaterial(string filename, Dictionary<string, DMeshSubset> meshSubsets) {
            StreamReader reader = new StreamReader(standardMeshDirectory + filename);
            Regex regex = new Regex(@"\s+");
            DMeshSubset currentSubset = null;

            while (!reader.EndOfStream) {
                string[] tokens = regex.Split(reader.ReadLine());

                switch (tokens[0]) {
                    case "newmtl":
                        if (currentSubset != null)
                            meshSubsets.Add(currentSubset.material.name, currentSubset);
                        currentSubset = new DMeshSubset();
                        currentSubset.material.name = tokens[1];
                        break;
                    case "Ka":
                        currentSubset.material.ambient.RedFloat = float.Parse(tokens[1], CultureInfo.CreateSpecificCulture("en-us"));
                        currentSubset.material.ambient.GreenFloat = float.Parse(tokens[2], CultureInfo.CreateSpecificCulture("en-us"));
                        currentSubset.material.ambient.BlueFloat = float.Parse(tokens[3], CultureInfo.CreateSpecificCulture("en-us"));
                        break;
                    case "Kd":
                        currentSubset.material.diffuse.RedFloat = float.Parse(tokens[1], CultureInfo.CreateSpecificCulture("en-us"));
                        currentSubset.material.diffuse.GreenFloat = float.Parse(tokens[2], CultureInfo.CreateSpecificCulture("en-us"));
                        currentSubset.material.diffuse.BlueFloat = float.Parse(tokens[3], CultureInfo.CreateSpecificCulture("en-us"));
                        break;
                    case "Ks":
                        currentSubset.material.specular.RedFloat = float.Parse(tokens[1], CultureInfo.CreateSpecificCulture("en-us"));
                        currentSubset.material.specular.GreenFloat = float.Parse(tokens[2], CultureInfo.CreateSpecificCulture("en-us"));
                        currentSubset.material.specular.BlueFloat = float.Parse(tokens[3], CultureInfo.CreateSpecificCulture("en-us"));
                        break;
                    case "map_Kd":
                        currentSubset.colorTexture = new FastBitmap(new Bitmap(standardMeshDirectory + tokens[1]));
                        break;
                }
            }
            reader.Close();

            if(currentSubset != null) 
                meshSubsets.Add(currentSubset.material.name, currentSubset);          
              
        }

    }


}
