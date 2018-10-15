using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Battleships.Libraries
{
    public static class FontLibrary
    {
        private static Dictionary<string, SpriteFont> fonts;
        private static KeyValuePair<string, string> fontsToLoad = new KeyValuePair<string, string>
        {

        };

        public static SpriteFont GetFont(string name)
        {
            throw new System.NotImplementedException();
        }

        public static void LoadFonts(ContentManager contentManager)
        {
            throw new System.NotImplementedException();
        }
    }
}