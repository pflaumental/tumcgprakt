using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Color = RayTracerFramework.Shading.Color;
using RayTracerFramework.Geometry;

namespace RayTracerFramework.Shading {
    class CubeMap {
        public float xMin, xMax, yMin, yMax, zMin, zMax;
        public Image xMinImage, xMaxImage, yMinImage, yMaxImage, zMinImage, zMaxImage;

        // Noch nicht benutzen!

        public CubeMap(float xMin, float xMax, float yMin, float yMax, float zMin, float zMax) {
            this.xMin = xMin;
            this.xMax = xMax;
            this.yMin = yMin;
            this.yMax = yMax;
            this.zMin = zMin;
            this.zMax = zMax;
        }

        // Todo: Hack entfrickeln, getPixel umgehen
        public Color getColor(Ray ray) {
            float t;
            Vec3 dir = ray.direction;
            

            if (dir.x > 0) {
                t = (xMax - ray.position.x) / dir.x;
                Vec3 p = ray.position + dir * t;
                if (p.y <= yMax && p.y >= yMin && p.z >= zMin && p.z <= zMax) {
                    Console.WriteLine("Collision at: " + p);
                    float x = -p.z + xMax ;
                    float y = -p.y + yMax;
                    float xFactor = (xMaxImage.Width - 1) / (xMax - xMin);
                    float yFactor = (xMaxImage.Height - 1) / (yMax - yMin);
                    
                    int pixelX = (int)(x * yFactor);
                    int pixelY = (int)(y * yFactor);
                    //Console.WriteLine(x + "/" + y);
                    //Console.WriteLine(pixelX + "/" + pixelY);
                    System.Drawing.Color c = new Bitmap(xMaxImage).GetPixel(pixelX, pixelY);
                    return new Color(c.R, c.G, c.B);
                }
            } 
            else if (dir.x < 0) {
                t = (xMin - ray.position.x) / dir.x;
                Vec3 p = ray.position + dir * t;
                if (p.y <= yMax && p.y >= yMin && p.z >= zMin && p.z <= zMax) {
                    Console.WriteLine("Collision at: " + p);
                    return Color.White;
                }
            }


            if (dir.y > 0) {
                t = (yMax - ray.position.y) / dir.y;
                Vec3 p = ray.position + dir * t;
                if (p.x <= xMax && p.x >= xMin && p.z >= zMin && p.z <= zMax) {
                    Console.WriteLine("Collision at: " + p);
                    return Color.White;
                }
            } 
            else if (dir.y < 0) {
                t = (yMin - ray.position.y) / dir.y;
                Vec3 p = ray.position + dir * t;
                if (p.x <= xMax && p.x >= xMin && p.z >= zMin && p.z <= zMax) {
                    Console.WriteLine("Collision at: " + p);
                    return Color.White;
                }
            }

            if (dir.z > 0) {
                t = (zMax - ray.position.z) / dir.z;
                Vec3 p = ray.position + dir * t;
                if (p.x <= xMax && p.x >= xMin && p.y >= yMin && p.y <= yMax) {
                    Console.WriteLine("Collision at: " + p);
                    return Color.White; 
                }
            } else if (dir.z < 0) {
                t = (zMin - ray.position.z) / dir.z;
                Vec3 p = ray.position + dir * t;
                if (p.x <= xMax && p.x >= xMin && p.y >= yMin && p.y <= yMax) {
                    Console.WriteLine("Collision at: " + p);
                    return Color.White; 
                }
            }

            throw new Exception("No valid direction for the ray specified.");

           
        }


    }
}
