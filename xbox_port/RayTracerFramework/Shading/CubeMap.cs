using System;
using System.Collections.Generic;
using System.Text;
using Color = RayTracerFramework.Shading.Color;
using RayTracerFramework.Geometry;
using RayTracerFramework.Utility;
using GameServiceContainer = Microsoft.Xna.Framework.GameServiceContainer;
using ContentManager = Microsoft.Xna.Framework.Content.ContentManager;
using Texture2D = Microsoft.Xna.Framework.Graphics.Texture2D;

namespace RayTracerFramework.Shading {
    class CubeMap {
        public float xMin, xMax, yMin, yMax, zMin, zMax;

        private FastBitmap xMinTexture, xMaxTexture;
        private FastBitmap yMinTexture, yMaxTexture;
        private FastBitmap zMinTexture, zMaxTexture;

        private GameServiceContainer gameServiceContainer;

        public CubeMap(
                float width, 
                float height, 
                float depth, 
                string texturesBaseName, 
                GameServiceContainer gameServiceContainer) {
            this.xMin = -(width * 0.5f);
            this.xMax = width * 0.5f;
            this.yMin = -(height * 0.5f);
            this.yMax = height * 0.5f;
            this.zMin = -(depth * 0.5f);
            this.zMax = depth * 0.5f;

            ContentManager content = new ContentManager(gameServiceContainer);

            xMinTexture = new FastBitmap(content.Load<Texture2D>("Content/Textures/" + texturesBaseName + "NX"));
            xMaxTexture = new FastBitmap(content.Load<Texture2D>("Content/Textures/" + texturesBaseName + "PX"));

            yMinTexture = new FastBitmap(content.Load<Texture2D>("Content/Textures/" + texturesBaseName + "NY"));
            yMaxTexture = new FastBitmap(content.Load<Texture2D>("Content/Textures/" + texturesBaseName + "PY"));

            zMinTexture = new FastBitmap(content.Load<Texture2D>("Content/Textures/" + texturesBaseName + "NZ"));
            zMaxTexture = new FastBitmap(content.Load<Texture2D>("Content/Textures/" + texturesBaseName + "PZ"));
        }

  
        public Color getColor(Ray ray) {
            float t;
 
            Vec3 posWS = ray.position;  // Position where the ray starts in world space
            Vec3 dirWS = ray.direction; // Direction of the ray in world space

            // Test if ray intersects right plane
            if (dirWS.x > 0) {
                t = (xMax - posWS.x) / dirWS.x;
                Vec3 p = posWS + dirWS * t;
                if (p.y <= yMax && p.y >= yMin && p.z >= zMin && p.z <= zMax) {
                    float xTex = (-p.z + zMax) / (zMax - zMin);
                    float yTex = (-p.y + yMax) / (yMax - yMin);

                    float pixelX = (xTex * (xMaxTexture.Width - 1));
                    float pixelY = (yTex * (xMaxTexture.Height - 1));

                    //int pixelX = (int)(xTex * (xMaxTexture.Width-1));
                    //int pixelY = (int)(yTex * (xMaxTexture.Height-1));
                    return xMaxTexture.GetPixel(pixelX, pixelY);
                }
            } 
            // Left Plane DONE
            else if (dirWS.x < 0) {
                t = (xMin - posWS.x) / dirWS.x;
                Vec3 p = posWS + dirWS * t;
                if (p.y <= yMax && p.y >= yMin && p.z >= zMin && p.z <= zMax) {
                    float xTex = (p.z + zMax) / (zMax - zMin);
                    float yTex = (-p.y + yMax) / (yMax - yMin);

                    float pixelX = (xTex * (xMinTexture.Width - 1));
                    float pixelY = (yTex * (xMinTexture.Height - 1));

                    //int pixelX = (int)(xTex * (xMinTexture.Width-1));
                    //int pixelY = (int)(yTex * (xMinTexture.Height-1));
                    return xMinTexture.GetPixel(pixelX, pixelY);
                }
            }

            // Upper plane todo
            if (dirWS.y > 0) {
                t = (yMax - posWS.y) / dirWS.y;
                Vec3 p = posWS + dirWS * t;
                if (p.x <= xMax && p.x >= xMin && p.z >= zMin && p.z <= zMax) {
                    float xTex = (p.x + xMax) / (xMax - xMin);
                    float yTex = (p.z + zMax) / (zMax - zMin);

                    float pixelX = (xTex * (yMaxTexture.Width - 1));
                    float pixelY = (yTex * (yMaxTexture.Height - 1));

                    //int pixelX = (int)(xTex * (yMaxTexture.Width-1));
                    //int pixelY = (int)(yTex * (yMaxTexture.Height - 1));

                    return yMaxTexture.GetPixel(pixelX, pixelY);
                }
            } 
            // lower plane todo
            else if (dirWS.y < 0) {
                t = (yMin - posWS.y) / dirWS.y;
                Vec3 p = posWS + dirWS * t;
                if (p.x <= xMax && p.x >= xMin && p.z >= zMin && p.z <= zMax) {
                    float xTex = (p.x + xMax) / (xMax - xMin);
                    float yTex = (-p.z + zMax) / (zMax - zMin);

                    float pixelX = (xTex * (yMinTexture.Width - 1));
                    float pixelY = (yTex * (yMinTexture.Height - 1));

                    //int pixelX = (int)(xTex * (yMinTexture.Width - 1));
                    //int pixelY = (int)(yTex * (yMinTexture.Height - 1));

                    return yMinTexture.GetPixel(pixelX, pixelY);
                }
            }

            // Back plane DONE
            if (dirWS.z > 0) {
                t = (zMax - posWS.z) / dirWS.z;
                Vec3 p = posWS + dirWS * t;
                if (p.x <= xMax && p.x >= xMin && p.y >= yMin && p.y <= yMax) {
                    float xTex = (p.x + xMax) / (xMax - xMin);
                    float yTex = (-p.y + yMax) / (yMax - yMin);

                    float pixelX = xTex * (zMaxTexture.Width - 1);
                    float pixelY = yTex * (zMaxTexture.Height - 1);                    

                    //int pixelX = (int) xTex * (zMaxTexture.Width - 1);
                    //int pixelY = (int) yTex * (zMaxTexture.Height - 1);

                    return zMaxTexture.GetPixel(pixelX, pixelY);
                }
            } 
            // Front Plane DONE
            else if (dirWS.z < 0) {
                t = (zMin - posWS.z) / dirWS.z;
                Vec3 p = posWS + dirWS * t;
                if (p.x <= xMax && p.x >= xMin && p.y >= yMin && p.y <= yMax) {
                    float xTex = (-p.x + xMax) / (xMax - xMin);
                    float yTex = (-p.y + yMax) / (yMax - yMin);

                    float pixelX = (xTex * (zMinTexture.Width - 1));
                    float pixelY = (yTex * (zMinTexture.Height - 1));                    

                    //int pixelX = (int)(xTex * (zMinTexture.Width-1));
                    //int pixelY = (int)(yTex * (zMinTexture.Height-1));

                    return zMinTexture.GetPixel(pixelX, pixelY);
                }
            }

            throw new Exception("No valid direction for the ray specified.");
           
        }


    }
}
