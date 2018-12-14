using Battleships.Libraries;
using Battleships.Objects;
using Battleships.Objects.UI;
using Battleships.Objects.Pickup;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Objects.UI;
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
        public IObject[]              Objects { get => objects.ToArray(); }

        private Type                  shipTypeOne;
        private Type                  shipTypeTwo;
        private Ship                  playerOne;
        private Ship                  playerTwo;
        private GraphicsDeviceManager graphics;
        private SpriteBatch           spriteBatch;
        private List<IObject>         objects;
        private List<IObject>         userInterface;
        private Camera                camera;
        private Vector2               baseDimension;
        private Texture2D             backgroundTexture;
        private float                 gameTimeMultiplier;
        private Action<Ship, Ship>    setWinnerAndLoser;

        private const float pickupInterval = 10f;
        private float elapsedPickupTime;

        private const float actionInterval = 0.01f;
        private float elapsedActionTime;

        public Game1(Type shipTypeOne, Type shipTypeTwo, Action<Ship, Ship> setWinnerAndLoser)
        {
            Window.AllowUserResizing = true;
            IsMouseVisible           = true;
            graphics                 = new GraphicsDeviceManager(this);
            Content.RootDirectory    = "Content";

            this.shipTypeOne   = shipTypeOne;
            this.shipTypeTwo   = shipTypeTwo;
            objects            = new List<IObject>();
            userInterface      = new List<IObject>();
            camera             = new Camera(Window.ClientBounds.Width, Window.ClientBounds.Height);
            baseDimension      = new Vector2(Window.ClientBounds.Width, Window.ClientBounds.Height);
            gameTimeMultiplier = 1f;

            this.setWinnerAndLoser = setWinnerAndLoser;

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
                    playerTwoStartPosition = new Vector2(242, 0);

            playerOne = (Ship)Activator.CreateInstance(shipTypeOne, playerOneStartPosition, Color.White);
            playerTwo = (Ship)Activator.CreateInstance(shipTypeTwo, playerTwoStartPosition, Color.Magenta);

            playerOne.Layer = playerTwo.Layer = 0.01f;
            playerOne.Game  = playerTwo.Game = this;

            playerOne.Initialize(playerTwo);
            playerTwo.Initialize(playerOne);


            objects.Add(playerOne);
            objects.Add(playerTwo);

            userInterface.Add(new TextLabel(playerOne.Name, new Vector2(45, 10), 0.12f));
            userInterface.Add(new TextLabel(playerTwo.Name, new Vector2(650, 10), 0.12f));

            userInterface.Add(new HealthBar(this, playerOne, new Point(100, 10), new Point(45, 30)) { Layer = 0.99f });
            userInterface.Add(new HealthBar(this, playerTwo, new Point(100, 10), new Point(650, 30)) { Layer = 0.99f });
            userInterface.Add(new EnergyBar(this, playerOne, new Point(100, 10), new Point(45, 60)) { Layer = 0.99f });
            userInterface.Add(new EnergyBar(this, playerTwo, new Point(100, 10), new Point(650, 60)) { Layer = 0.99f });

            userInterface.Add(new Slider(this, new Vector2(baseDimension.X / 2, 450), new Point(265, 20), (float value) => { gameTimeMultiplier = value; }, new Vector2(0.1f, 4f), 1, "Game time", GetUIScale));
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
            gameTime = new GameTime(new TimeSpan((long)(gameTime.TotalGameTime.Ticks * gameTimeMultiplier)), new TimeSpan((long)(gameTime.ElapsedGameTime.Ticks * gameTimeMultiplier)));
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            camera.Update(gameTime);

            // Spawns pickups.
            elapsedPickupTime += deltaTime;
            if (elapsedPickupTime >= pickupInterval)
            {
                elapsedPickupTime -= pickupInterval;
                Random random = new Random();
                Vector2 randomPosition = new Vector2((float)random.NextDouble() * 200 - 100, (float)random.NextDouble() * 200 - 100);
                /*do
                {
                    randomPosition = new Vector2((float)random.NextDouble() * 200 - 100, (float)random.NextDouble() * 200 - 100);
                }  while (objects.Where(x => ((x is Pickup) ? (x.Position - randomPosition).Length() < 10 : false)) != null);
                */

                switch(random.Next(2))
                {
                    case 0:
                        objects.Add(new EnergyPickup(randomPosition, 15, this, TextureLibrary.GetTexture("EnergyPickup")));
                        break;
                    case 1:
                        objects.Add(new HealthPickup(randomPosition, 15, this, TextureLibrary.GetTexture("HealthPickup")));
                        break;
                    case 2:
                        objects.Add(new MissilePickup(randomPosition, 15, this, TextureLibrary.GetTexture("EnergyPickup")));
                        break;
                }
            }

            // Calculates when to run the ships' action.
            elapsedActionTime += deltaTime;
            bool runAction = elapsedActionTime >= actionInterval;
            if (runAction)
            {
                elapsedActionTime -= actionInterval;
            }
            // Updates all active objects.
            for (int i = objects.Count - 1; i >= 0; --i)
            {
                IObject obj = objects[i];
                if (Math.Abs(obj.Position.X) > baseDimension.X * 0.6f || Math.Abs(obj.Position.Y) > baseDimension.Y * 0.6f)
                {
                    Destroy(obj);
                }
                obj.Update(gameTime);
                if (runAction && obj is Ship ship)
                {
                    ship.Act();
                }
            }

            for (int i = 0; i < userInterface.Count; ++i)
            {
                userInterface[i].Update(gameTime);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.F))
                camera.ShakeIntensity = 1;

            camera.ShakeIntensity = MathHelper.Lerp(camera.ShakeIntensity, 0, 0.1f);
            base.Update(gameTime);

            if (!objects.Contains(playerTwo))
            {
                winningPlayer = playerTwo.GetType();
                setWinnerAndLoser(playerTwo, playerOne);
                finish = true;
            }
            if (!objects.Contains(playerTwo))
            {
                winningPlayer = playerOne.GetType();
                setWinnerAndLoser(playerOne, playerTwo);
                finish = true;
            }

            if (finish)
            {
                finishTimeCount += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (finishTimeCount >= finishTime)
                {
                    Exit();
                }
            }
        }

        bool finish = false;
        float finishTimeCount = 0.0f;
        const float finishTime = 5f;
        Type winningPlayer;

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

            spriteBatch.Begin(transformMatrix: Matrix.CreateScale(new Vector3(GetUIScale(), 1)));
            for (int i = userInterface.Count - 1; i >= 0; --i)
            {
                userInterface[i].Draw(spriteBatch);
            }
            if (finish)
            {
                SpriteFont font = FontLibrary.GetFont("fixedsys");
                string text = $"{winningPlayer.Name} wins!";
                Vector2 center = baseDimension * 0.5f;
                float offset = 10f;
                int tLeft = (int)Math.Round(finishTime - finishTimeCount);
                string timeLeft = $"Exiting in: {tLeft} second{(tLeft != 1 ? "s" : "")}";
                spriteBatch.DrawString(font, text, center - Vector2.UnitY * offset, Color.White, 0, font.MeasureString(text) * 0.5f, 0.2f, SpriteEffects.None, 0);
                spriteBatch.DrawString(font, timeLeft, center + Vector2.UnitY * offset, Color.White, 0, font.MeasureString(timeLeft) * 0.5f, 0.2f, SpriteEffects.None, 0);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }

        public Vector2 GetUIScale()
        {
            return new Vector2(Window.ClientBounds.Width / baseDimension.X, Window.ClientBounds.Height / baseDimension.Y);
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
