using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Battleships.Libraries
{
    /// <summary>
    /// Texture library for storing textures.
    /// </summary>
    public static class TextureLibrary
    {
        private static Dictionary<string, Texture2D> textures;
        
        // Format: { Location, Key }
        private static string[,] texturesToLoad = new string[,]
        {
            { "ShipBody", "Ship" },
            { "glow", "glow" },
            { "Explosion", "Explosion" },
            { "Turret", "Turret" },
            { "Bullet", "Bullet" },
            { "HealthbarFill", "HealthbarFill" },
            { "HealthbarBorder", "HealthbarBorder" },
            { "HealthPickup", "HealthPickup" },
            { "EnergyPickup", "EnergyPickup" },
            { "SliderButton", "SliderButton" },
            { "missile", "Missile" }
        };

        /// <summary>
        /// Gets a texture with a specific key.
        /// </summary>
        /// <param name="key">The key of the texture.</param>
        /// <returns>Texture.</returns>
        public static Texture2D GetTexture(string key)
        {
            return textures[key];
        }

        /// <summary>
        /// Loads all textures from the list of textures to load.
        /// </summary>
        /// <param name="contentManager">Content manager to load from.</param>
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

        /// <summary>
        /// Unloads all textures.
        /// </summary>
        public static void UnloadTextures()
        {
            string[] keys = textures.Keys.ToArray();
            foreach (string key in keys)
            {
                textures[key].Dispose();
                textures.Remove(key);
            }
        }

        /// <summary>
        /// Builds all textures that do not get loaded from file.
        /// </summary>
        /// <param name="graphicsDevice">Graphics device to build with.</param>
        /// <param name="viewport">Viewport size of game window.</param>
        public static void BuildTextures(GraphicsDevice graphicsDevice, Point viewport)
        {
            // Generates galaxy texture
            Random randomNumberGenerator = new Random();
            textures["background"] = new Texture2D(graphicsDevice, viewport.X * 3, viewport.Y * 3);
            textures["background"].SetData(Enumerable.Range(0, (viewport.X * 3) * (viewport.Y * 3)).Select(i => randomNumberGenerator.NextDouble() < 0.005f ? ColorFromFloat((float)randomNumberGenerator.NextDouble()) : Color.Black).ToArray());
            
            // Generates pixel texture
            textures["pixel"] = new Texture2D(graphicsDevice, 1, 1);
            textures["pixel"].SetData(new Color[] { Color.White });
        }

        /// <summary>
        /// Returns a color from a floating point value.
        /// </summary>
        /// <param name="value">Color value.</param>
        /// <returns>Color.</returns>
        private static Color ColorFromFloat(float value)
        {
            return new Color(value, value, value);
        }
    }
}