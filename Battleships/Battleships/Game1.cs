using Battleships.Libraries;
using Battleships.Objects;
using Battleships.Objects.Pickup;
using Battleships.Objects.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Battleships
{
    /// <summary>
    /// Main game class.
    /// </summary>
    public class Game1 : Game, IGame1
    {
        public Vector2                BaseDimensions         { get; }

        private const float           FINISH_TIME           = 5f;    // Time left before the game exits after one player dies.
        private const float           PICKUP_INTERVAL       = 10f;   // Interval between pickup spawns.
        private const float           ACTION_INTERVAL       = 0.01f; // Interval between ship actions.

        private Type                  shipTypeOne;
        private Type                  shipTypeTwo;
        private Ship                  playerOne;
        private Ship                  playerTwo;

        private GraphicsDeviceManager graphics;
        private SpriteBatch           spriteBatch;
        private List<IObject>         objects;
        private List<IObject>         userInterface;

        private Camera                camera;
        private Texture2D             backgroundTexture;
        private float                 gameTimeMultiplier;
        private Action<Ship, Ship>    setWinnerAndLoser; // Function for the game to send the winning and losing ship to the program.

        private bool                  finish;
        private float                 finishTimeCount;
        private Type                  winningPlayer;
        private float                 timeLeft;
        
        private float                 elapsedPickupTime;
        private float                 elapsedActionTime;

        /// <param name="shipTypeOne">Type of first ship.</param>
        /// <param name="shipTypeTwo">Type of second ship.</param>
        /// <param name="setWinnerAndLoser">Function for the game to send the winning and losing ship to the program.</param>
        /// <param name="roundDuration">Duration of a round.</param>
        public Game1(Type shipTypeOne, Type shipTypeTwo, Action<Ship, Ship> setWinnerAndLoser, float roundDuration)
        {
            Window.AllowUserResizing  = true;
            IsMouseVisible            = true;
            graphics                  = new GraphicsDeviceManager(this);
            Content.RootDirectory     = "Content";

            this.shipTypeOne          = shipTypeOne;
            this.shipTypeTwo          = shipTypeTwo;

            objects                   = new List<IObject>();
            userInterface             = new List<IObject>();
                                      
            BaseDimensions             = new Vector2(800, 480);
            camera                    = new Camera((int)BaseDimensions.X, (int)BaseDimensions.Y);

            gameTimeMultiplier        = 1f;
            timeLeft                  = roundDuration;

            this.setWinnerAndLoser    = setWinnerAndLoser;

            Window.ClientSizeChanged += UpdateCamera;
        }

        /// <summary>
        /// Updates camera when the size of the window has changed.
        /// </summary>
        private void UpdateCamera(object sender, EventArgs e)
        {
            camera.UpdateViewport(Window.ClientBounds.Width, Window.ClientBounds.Height);
            camera.Zoom = new Vector2(Window.ClientBounds.Width / BaseDimensions.X, Window.ClientBounds.Height / BaseDimensions.Y);
        }

        /// <summary>
        /// Initializes game.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
            backgroundTexture = TextureLibrary.GetTexture("background");

            camera.ShakeMagnitude = 14;

            Vector2 playerOneStartPosition = new Vector2(-242, 0);
            Vector2 playerTwoStartPosition = new Vector2(242, 0);

            playerOne = (Ship)Activator.CreateInstance(shipTypeOne, playerOneStartPosition, Color.White);
            playerTwo = (Ship)Activator.CreateInstance(shipTypeTwo, playerTwoStartPosition, Color.Magenta);

            playerOne.Layer = playerTwo.Layer = 0.01f;
            playerOne.Game  = playerTwo.Game = this;

            playerOne.Initialize(playerTwo, GetGameInformation);
            playerTwo.Initialize(playerOne, GetGameInformation);

            objects.Add(playerOne);
            objects.Add(playerTwo);

            userInterface.Add(new TextLabel(playerOne.Name, new Vector2(45, 10), 0.12f));
            userInterface.Add(new TextLabel(playerTwo.Name, new Vector2(650, 10), 0.12f));

            userInterface.Add(new HealthBar(this, playerOne, new Point(100, 10), new Point(45, 30))  { Layer = 0.99f });
            userInterface.Add(new HealthBar(this, playerTwo, new Point(100, 10), new Point(650, 30)) { Layer = 0.99f });

            userInterface.Add(new EnergyBar(playerOne, new Point(100, 10), new Point(45, 60))  { Layer = 0.99f });
            userInterface.Add(new EnergyBar(playerTwo, new Point(100, 10), new Point(650, 60)) { Layer = 0.99f });

            userInterface.Add(new Slider(this, new Point((int)BaseDimensions.X / 2, 450), new Point(265, 20), (float value) => { gameTimeMultiplier = value; }, new Vector2(0.1f, 4f), 1, "Game time", GetUIScale));
        }

        /// <summary>
        /// Supplies game data to ship.
        /// </summary>
        /// <param name="ship">Ship to supply for.</param>
        /// <returns>Game data.</returns>
        public GameInformation GetGameInformation(Ship ship)
        {
            return new GameInformation(new Pickup[3], timeLeft, Math.Abs(ship.Position.X) > BaseDimensions.X * 0.6f || Math.Abs(ship.Position.Y) > BaseDimensions.Y * 0.6f);
        }

        /// <summary>
        /// Loads content files.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            TextureLibrary.LoadTextures(Content);
            TextureLibrary.BuildTextures(GraphicsDevice, Window.ClientBounds.Size);
            FontLibrary.LoadFonts(Content);
        }

        /// <summary>
        /// Unloads content files.
        /// </summary>
        protected override void UnloadContent()
        {
            TextureLibrary.UnloadTextures();
            FontLibrary.UnloadFonts();
            Content.Unload();
            Content.Dispose();
        }

        /// <summary>
        /// Updates game.
        /// </summary>
        /// <param name="gameTime">Container for time data such as elapsed time since last update.</param>
        protected override void Update(GameTime gameTime)
        {
            // Updates the game time to account for change in time speed with the time multiplier.
            gameTime = new GameTime(new TimeSpan((long)(gameTime.TotalGameTime.Ticks * gameTimeMultiplier)), new TimeSpan((long)(gameTime.ElapsedGameTime.Ticks * gameTimeMultiplier)));

            base.Update(gameTime);

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            camera.Update(gameTime);
            if (!finish)
            {
                if (timeLeft > 0)
                {
                    timeLeft -= deltaTime;
                    if (timeLeft < 0)
                    {
                        timeLeft = 0;
                    }
                }
                else
                {
                    Exit();
                }
            }

            SpawnPickups(deltaTime);

            UpdateObjects(gameTime);

            if (Keyboard.GetState().IsKeyDown(Keys.F))
            {
                camera.ShakeIntensity = 1;
            }

            camera.ShakeIntensity = MathHelper.Lerp(camera.ShakeIntensity, 0, 0.1f);

            if (!objects.Contains(playerOne))
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
                finishTimeCount += deltaTime;
                if (finishTimeCount >= FINISH_TIME)
                {
                    Exit();
                }
            }
        }

        /// <summary>
        /// Calculates if to spawn pickups then spawns them accordingly.
        /// </summary>
        /// <param name="deltaTime">Seconds elapsed since last update call.</param>
        private void SpawnPickups(float deltaTime)
        {
            elapsedPickupTime += deltaTime;
            if (elapsedPickupTime >= PICKUP_INTERVAL)
            {
                elapsedPickupTime    -= PICKUP_INTERVAL;
                Random random         = new Random();
                Vector2 pickupPositon = new Vector2(0, -80);

                switch (random.Next(3))
                {
                    case 0:
                        objects.Add(new EnergyPickup(pickupPositon, 15, this));
                        break;
                    case 1:
                        objects.Add(new HealthPickup(pickupPositon, 15, this));
                        break;
                    case 2:
                        objects.Add(new MissilePickup(pickupPositon, 15, this));
                        break;
                }
            }
        }

        /// <summary>
        /// Updates all objects.
        /// </summary>
        /// <param name="gameTime">Container for time data such as elapsed time since last update.</param>
        private void UpdateObjects(GameTime gameTime)
        {
            float deltaTime    = (float)gameTime.ElapsedGameTime.TotalSeconds;

            elapsedActionTime += deltaTime;
            bool runAction     = elapsedActionTime >= ACTION_INTERVAL;
            if (runAction)
            {
                elapsedActionTime -= 0;
            }

            for (int i = objects.Count - 1; i >= 0; --i)
            {
                IObject obj = objects[i];

                if (Math.Abs(obj.Position.X) > BaseDimensions.X * 0.6f || Math.Abs(obj.Position.Y) > BaseDimensions.Y * 0.6f)
                {
                    if (!(obj is Ship))
                    {
                        Destroy(obj);
                    }
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
        }

        /// <summary>
        /// Draws the game.
        /// </summary>
        /// <param name="gameTime">Container for time data such as elapsed time since last update.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Magenta);

            spriteBatch.Begin();
            spriteBatch.Draw(backgroundTexture, Vector2.Zero, new Rectangle(Point.Zero, Window.ClientBounds.Size), Color.White, 0, Vector2.Zero, Vector2.One, SpriteEffects.None, 0);
            spriteBatch.End();

            DrawObjects();
            DrawUI();
            
            base.Draw(gameTime);
        }

        /// <summary>
        /// Draws all objects.
        /// </summary>
        private void DrawObjects()
        {
            spriteBatch.Begin(transformMatrix: camera.TranslationMatrix, sortMode: SpriteSortMode.Immediate, blendState: BlendState.Additive);
            
            for (int i = objects.Count - 1; i >= 0; --i)
            {
                objects[i].Draw(spriteBatch);
            }
            spriteBatch.End();
        }

        /// <summary>
        /// Draws the user interface.
        /// </summary>
        private void DrawUI()
        {
            spriteBatch.Begin(transformMatrix: Matrix.CreateScale(new Vector3(GetUIScale(), 1)));
            for (int i = userInterface.Count - 1; i >= 0; --i)
            {
                userInterface[i].Draw(spriteBatch);
            }
            if (finish)
            {
                DrawFinishMessage();
            }
            else
            {
                DrawTimer();
            }
            spriteBatch.End();
        }

        /// <summary>
        /// Draws finish message (winner and time left).
        /// </summary>
        private void DrawFinishMessage()
        {
            SpriteFont font = FontLibrary.GetFont("fixedsys");
            string text     = $"{winningPlayer.Name} wins!";

            Vector2 center = BaseDimensions * 0.5f;
            Vector2 offset = Vector2.UnitY * 10f;

            int tLeft       = (int)Math.Round(FINISH_TIME - finishTimeCount);
            string timeLeft = $"Exiting in: {tLeft} second{(tLeft != 1 ? "s" : "")}";

            spriteBatch.DrawString(font, text, center - offset, Color.White, 0, font.MeasureString(text) * 0.5f, 0.2f, SpriteEffects.None, 0);
            spriteBatch.DrawString(font, timeLeft, center + offset, Color.White, 0, font.MeasureString(timeLeft) * 0.5f, 0.2f, SpriteEffects.None, 0);
        }

        /// <summary>
        /// Draws game timer.
        /// </summary>
        private void DrawTimer()
        {
            SpriteFont font = FontLibrary.GetFont("fixedsys");
            int tLeft = (int)Math.Round(timeLeft);
            string text = $"Time left: {tLeft} second{(tLeft != 1 ? "s" : "")}";
            Vector2 center = BaseDimensions * 0.5f;
            spriteBatch.DrawString(font, text, center + Vector2.UnitY * -200, Color.White, 0, font.MeasureString(text) * 0.5f, 0.2f, SpriteEffects.None, 0);
        }

        /// <summary>
        /// Calculates the scale of the UI.
        /// </summary>
        /// <returns>Vector of UI scale in relation to window size.</returns>
        public Vector2 GetUIScale()
        {
            return new Vector2(Window.ClientBounds.Width / BaseDimensions.X, Window.ClientBounds.Height / BaseDimensions.Y);
        }

        /// <summary>
        /// Shakes camera.
        /// </summary>
        /// <param name="amount">Amount to shake with.</param>
        public void ShakeCamera(float amount)
        {
            camera.ShakeIntensity += amount;
        }

        /// <summary>
        /// Destroys object.
        /// </summary>
        /// <param name="obj">Object to destroy.</param>
        public void Destroy(IObject obj)
        {
            if(!objects.Remove(obj))
            {
                userInterface.Remove(obj);
            }
        }

        /// <summary>
        /// Instantiates object.
        /// </summary>
        /// <param name="obj">Object to instantiate.</param>
        /// <returns>The instantiated object.</returns>
        public IObject Instantiate(IObject obj)
        {
            objects.Add(obj);
            return obj;
        }
    }
}