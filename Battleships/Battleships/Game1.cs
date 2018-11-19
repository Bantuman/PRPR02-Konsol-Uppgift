using Battleships.Libraries;
using Battleships.Objects;
using Battleships.Objects.UI;
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
    public class Game1 : Game, IGame1
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch           spriteBatch;
        private List<IObject>         objects;
        private List<IObject>         userInterface;
        private Camera                camera;
        private Vector2               baseDimension;
        private Texture2D             backgroundTexture;

        private const float actionInterval = 0.01f;
        private float elapsedActionTime;

        public Game1()
        {
            Window.AllowUserResizing = true;
            IsMouseVisible           = true;
            graphics                 = new GraphicsDeviceManager(this);
            Content.RootDirectory    = "Content";
            

            objects       = new List<IObject>();
            userInterface = new List<IObject>();
            camera        = new Camera(Window.ClientBounds.Width, Window.ClientBounds.Height);
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
            backgroundTexture = TextureLibrary.GetTexture("background");

            camera.ShakeMagnitude = 14;

            Vector2 playerOneStartPosition = new Vector2(-242, 0),
                    playerTwoStartPosition = new Vector2( 242, 0);

            Ship playerOne = new AIPlayer(playerOneStartPosition, "Nemo", Color.Red) { Game = this, Layer = 0.01f };
            Ship playerTwo = new AIPlayer(playerTwoStartPosition, "Alex", Color.White) { Game = this, Layer = 0.01f };
            playerOne.Initialize();
            playerTwo.Initialize();

            objects.Add(playerOne);
            objects.Add(playerTwo);

            userInterface.Add(new HealthBar(this, playerOne, new Point(100, 10), new Point(45, 50))  { Layer = 0.99f });
            userInterface.Add(new HealthBar(this, playerTwo, new Point(100, 10), new Point(650, 50)) { Layer = 0.99f });
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
            TextureLibrary.BuildTextures(GraphicsDevice, Window.ClientBounds.Size);
            FontLibrary.LoadFonts(Content);
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
            if (Keyboard.GetState().IsKeyDown(Keys.F))
                camera.ShakeIntensity = 1;

            camera.ShakeIntensity = MathHelper.Lerp(camera.ShakeIntensity, 0, 0.1f);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Magenta);

            spriteBatch.Begin();
            spriteBatch.Draw(backgroundTexture, Vector2.Zero, new Rectangle(Point.Zero, Window.ClientBounds.Size), Color.White, 0, Vector2.Zero, Vector2.One, SpriteEffects.None, 0);
            spriteBatch.End();

            spriteBatch.Begin(transformMatrix: camera.TranslationMatrix, sortMode: SpriteSortMode.Immediate, blendState: BlendState.Additive);
            // Draws all active objects.
            for (int i = objects.Count - 1; i >= 0; --i)
            {
                objects[i].Draw(spriteBatch);
            }
            spriteBatch.End();

            spriteBatch.Begin(transformMatrix: Matrix.CreateScale(new Vector3(Window.ClientBounds.Width / baseDimension.X, Window.ClientBounds.Height / baseDimension.Y, 1)));
            for (int i = userInterface.Count - 1; i >= 0; --i)
            {
                userInterface[i].Draw(spriteBatch);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }

        public void ShakeCamera(float amount)
        {
            camera.ShakeIntensity += amount;
        }

        public void Destroy(IObject obj)
        {
            if(!objects.Remove(obj))
                userInterface.Remove(obj);
        }

        public IObject Instantiate(IObject obj)
        {
            objects.Add(obj);
            return obj;
        }
    }
}
