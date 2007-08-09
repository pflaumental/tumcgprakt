using System;
using System.Collections.Generic;
using System.Text;
using RayTracerFramework.Shading;
using RayTracerFramework.Geometry;
using System.IO;
using System.Text.RegularExpressions;
using System.Globalization;

namespace RayTracerFramework.Loading {
    
    // Related assignement: 6.1

    public class OBJPointLoader : IPointLoader {

        public string standardMeshDirectory = Settings.Setup.Loading.DefaultStandardMeshDirectory;

        public List<IIntersectable> LoadFromFile(string filename) {
            List<IIntersectable> pointlist = new List<IIntersectable>();

            StreamReader reader = new StreamReader(standardMeshDirectory + filename);
            Regex regex = new Regex(@"\s+");
 
            
            //List<Vec3> vertices = new List<Vec3>();
            //List<Vec3> normals = new List<Vec3>();
            int count = 1024 * 1024 * 50;
            char[] buffer = new char[count];
            string prefix = "";
            StringBuilder sb = new StringBuilder();
           
            float z = float.PositiveInfinity;
            float currentZ;

            while (!reader.EndOfStream) {
                reader.ReadBlock(buffer, 0, count);
                            
                string[] tokens = new String(buffer).Insert(0, prefix).Split('\n');
                prefix = tokens[tokens.Length - 1];
                for (int i = 0; i < tokens.Length - 1; i++) {
                    string[] tok = regex.Split(tokens[i]);
                    if (tok[0] == "v") {
                        currentZ =  float.Parse(tok[3], CultureInfo.CreateSpecificCulture("en-us"));
                        pointlist.Add(new DPoint(new Vec3(float.Parse(tok[1], CultureInfo.CreateSpecificCulture("en-us")),
                                                  float.Parse(tok[2], CultureInfo.CreateSpecificCulture("en-us")),
                                                  z)));
                        if( currentZ < z) {
                            z = currentZ;
                        }
                        if (pointlist.Count > 10000) {
                            Console.WriteLine(z);
                            return pointlist;
                        }
                    }
                
                }
            
            }
            
            /*
            while (!reader.EndOfStream) {
                string[] tokens = regex.Split(reader.ReadLine());
                

                switch (tokens[0]) {
                    case "v":
                        if ((i++ % 1000000000) == 0) {

                            pointlist.Add(new DPoint(new Vec3(float.Parse(tokens[1], CultureInfo.CreateSpecificCulture("en-us")),
                                                  float.Parse(tokens[2], CultureInfo.CreateSpecificCulture("en-us")),
                                                  float.Parse(tokens[3], CultureInfo.CreateSpecificCulture("en-us")))));
                        }
                        break;
                }
            }
            */
            reader.Close();

            return pointlist;
        }
    }
}
