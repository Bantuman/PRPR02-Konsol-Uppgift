using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace Battleships.Libraries
{
    /// <summary>
    /// Font library for storing sprite fonts.
    /// </summary>
    public static class FontLibrary
    {
        private static Dictionary<string, SpriteFont> fonts;

        // Format: { Location, Key }
        private static string[,] fontsToLoad = new string[,]
        {
            { "Pixeled", "Pixel" },
            { "fixedsys", "fixedsys" },
            { "Consolas", "Consolas" },
        };

        /// <summary>
        /// Gets a sprite font with a specific key.
        /// </summary>
        /// <param name="key">The key of the sprite font.</param>
        /// <returns>Sprite font.</returns>
        public static SpriteFont GetFont(string key)
        {
            return fonts[key];
        }

        /// <summary>
        /// Loads all fonts from the list of fonts to load.
        /// </summary>
        /// <param name="contentManager">Content manager to load from.</param>
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

        /// <summary>
        /// Unloads all fonts.
        /// </summary>
        public static void UnloadFonts()
        {
            string[] keys = fonts.Keys.ToArray();
            foreach (string key in keys)
            {
                fonts.Remove(key);
            }
        }
    }
}