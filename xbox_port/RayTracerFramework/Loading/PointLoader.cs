using System;
using System.Collections.Generic;
using System.Text;
using RayTracerFramework.Geometry;
using System.IO;
using System.Text.RegularExpressions;
using System.Globalization;
using RayTracerFramework.Shading;

namespace RayTracerFramework.Loading {
    class PointLoader : IPointLoader {

        public string standardMeshDirectory = "Content/Models/";
        
        public List<IIntersectable> LoadFromFile(string filename) {
            List<IIntersectable> pointlist = new List<IIntersectable>();

            StreamReader reader = new StreamReader(standardMeshDirectory + filename);
            Regex regex = new Regex(@"\s+");

            //List<Vec3> vertices = new List<Vec3>();
            //List<Vec3> normals = new List<Vec3>();

            while (!reader.EndOfStream) {
                string[] tokens = regex.Split(reader.ReadLine());
                pointlist.Add(new DPoint(1f/100000f * new Vec3(float.Parse(tokens[0], CultureInfo.CreateSpecificCulture("en-us")),
                        float.Parse(tokens[1], CultureInfo.CreateSpecificCulture("en-us")),
                        float.Parse(tokens[2], CultureInfo.CreateSpecificCulture("en-us")))));

            }
            reader.Close();

            return pointlist;            
        }
    }
}
