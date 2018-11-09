using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Battleships.Libraries
{
    public static class TextureLibrary
    {
        private static Dictionary<string, Texture2D> textures;
        
        private static Color ColorFromFloat(float value)
        {
            return new Color(value, value, value);
        }

        // Format: { Location, Key }
        private static string[,] texturesToLoad = new string[,]
        {
            { "ShipBody", "Ship" },
            { "Explosion", "Explosion" },
            { "Turret", "Turret" },
            { "Bullet", "Bullet" }
        };

        public static Texture2D GetTexture(string key)
        {
            return textures[key];
        }

        public static void BuildTextures(GraphicsDevice graphicsDevice, Point viewport)
        {
            // Generates galaxy texture
            Random randomNumberGenerator = new Random();
            textures["background"] = new Texture2D(graphicsDevice, viewport.X * 3, viewport.Y * 3);
            textures["background"].SetData(Enumerable.Range(0, (viewport.X * 3) * (viewport.Y * 3)).Select(i => randomNumberGenerator.NextDouble() < 0.005f ? ColorFromFloat((float)randomNumberGenerator.NextDouble()) : Color.Black).ToArray());
        }

        public static void LoadTextures(ContentManager contentManager)
        {
            if (textures == null)
            {
                textures = new Dictionary<string, Texture2D>();
            }
            for(int i = 0; i < texturesToLoad.GetLength(0); ++i)
            {
                textures[texturesToLoad[i, 1]] = contentManager.Load<Texture2D>(texturesToLoad[i, 0]);
            }
        }
    }
}