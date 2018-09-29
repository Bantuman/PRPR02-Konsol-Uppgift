using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battleships.Libraries
{
    static class TextureLibrary
    {
        private static Dictionary<string, Texture> textures;
        private static KeyValuePair<string, string> texturesToLoad = new KeyValuePair<string, string>
        {

        };

        public static Texture GetTexture(string name)
        {
            throw new System.NotImplementedException();
        }

        public static void LoadTextures(ContentManager contentManager)
        {
            throw new System.NotImplementedException();
        }
    }
}