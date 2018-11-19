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

        // Format: { Location, Key }
        private static string[,] fontsToLoad = new string[,]
        {
            { "Pixeled", "Pixel" }
        };

        public static SpriteFont GetFont(string key)
        {
            return fonts[key];
        }

        public static void LoadFonts(ContentManager contentManager)
        {
            if (fonts == null)
            {
                fonts = new Dictionary<string, SpriteFont>();
            }
            for (int i = 0; i < fontsToLoad.GetLength(0); ++i)
            {
                fonts[fontsToLoad[i, 1]] = contentManager.Load<SpriteFont>(fontsToLoad[i, 0]);
            }
        }
    }
}