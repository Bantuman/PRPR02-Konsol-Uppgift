using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Battleships.Libraries
{
    static class TextureLibrary
    {
        private static Dictionary<string, Texture2D> textures;

        // Format: { Location, Key }
        private static string[,] texturesToLoad = new string[,]
        {
            { "Ship", "Ship" }
        };

        public static Texture2D GetTexture(string key)
        {
            return textures[key];
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