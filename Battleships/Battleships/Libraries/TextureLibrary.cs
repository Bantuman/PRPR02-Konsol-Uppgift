using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Battleships.Libraries
{
    static class TextureLibrary
    {
        private static string TexturePath => Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\Textures\\";

        private static Dictionary<string, Texture> textures;

        // Format: { Location, Key }
        private static string[,] texturesToLoad = new string[,]
        {
            { "Ship.png", "Ship" }
        };

        public static Texture GetTexture(string key)
        {
            return textures[key];
        }

        public static void LoadTextures(ContentManager contentManager)
        {
            for(int i = 0; i < texturesToLoad.GetLength(1); ++i)
            {
                textures[texturesToLoad[1, i]] = contentManager.Load<Texture2D>(TexturePath + texturesToLoad[0, i]);
            }
        }
    }
}