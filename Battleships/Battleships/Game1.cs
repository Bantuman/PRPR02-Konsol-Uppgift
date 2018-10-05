using Battleships.Libraries;
using Battleships.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace Battleships
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch           spriteBatch;
        private List<IObject>         objects;
        private Camera                camera;
        private Vector2               baseDimension;

        private const float actionInterval = 2;
        private float elapsedActionTime;

        public Game1()
        {
            Window.AllowUserResizing = true;
            IsMouseVisible           = true;
            graphics                 = new GraphicsDeviceManager(this);
            Content.RootDirectory    = "Content";

            objects = new List<IObject>();
            camera = new Camera(Window.ClientBounds.Width, Window.ClientBounds.Height);
            baseDimension = new Vector2(Window.ClientBounds.Width, Window.ClientBounds.Height);

            Window.ClientSizeChanged += Window_ClientSizeChanged;
        }

        private void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            camera.UpdateViewport(Window.ClientBounds.Width, Window.ClientBounds.Height);
            camera.Zoom = new Vector2(Window.ClientBounds.Width / baseDimension.X, Window.ClientBounds.Height / baseDimension.Y);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            Vector2 playerOneStartPosition = new Vector2(0, 0),
                    playerTwoStartPosition = new Vector2(400, 400);

            objects.Add(new AIPlayer(playerOneStartPosition));
            objects.Add(new AIPlayer(playerTwoStartPosition));
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            TextureLibrary.LoadTextures(Content);
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            camera.Update(gameTime);

            // Calculates when to run the ships' action.
            elapsedActionTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            bool runAction = elapsedActionTime >= actionInterval;
            if (runAction)
            {
                elapsedActionTime -= actionInterval;
            }
            // Updates all active objects.
            for (int i = objects.Count - 1; i >= 0; --i)
            {
                IObject obj = objects[i];
                obj.Update(gameTime);
                if (runAction && obj is Ship ship)
                {
                    ship.Act();
                }
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Magenta);
            spriteBatch.Begin(transformMatrix: camera.TranslationMatrix);

            // Draws all active objects.
            for (int i = objects.Count - 1; i >= 0; --i)
            {
                objects[i].Draw(spriteBatch);
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
