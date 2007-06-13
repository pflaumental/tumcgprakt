using System;
using System.Collections.Generic;
using System.Text;
using RayTracerFramework.Shading;
using RayTracerFramework.Geometry;
using System.IO;
using System.Text.RegularExpressions;
using System.Globalization;

namespace RayTracerFramework.Loading {
    class OBJPointLoader : IPointLoader {

        public string standardMeshDirectory = "../../../Models/";

        public List<IIntersectable> LoadFromFile(string filename) {
            List<IIntersectable> pointlist = new List<IIntersectable>();

            StreamReader reader = new StreamReader(standardMeshDirectory + filename);
            Regex regex = new Regex(@"\s+");

            //List<Vec3> vertices = new List<Vec3>();
            //List<Vec3> normals = new List<Vec3>();

            while (!reader.EndOfStream) {
                string[] tokens = regex.Split(reader.ReadLine());

                switch (tokens[0]) {
                    case "v":
                        pointlist.Add(new DPoint(new Vec3(float.Parse(tokens[1], CultureInfo.CreateSpecificCulture("en-us")),
                                              float.Parse(tokens[2], CultureInfo.CreateSpecificCulture("en-us")),
                                              float.Parse(tokens[3], CultureInfo.CreateSpecificCulture("en-us")))));
                        break;
                }
            }
            reader.Close();

            return pointlist;
        }
    }
}
