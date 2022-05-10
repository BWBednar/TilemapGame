/**
 * Starting Code from Nathan Bean's GameArchitectureExample project
 */

using System;
using System.Threading;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using TilemapGame.StateManagement;
using TilemapGame.Sprites;
using TilemapGame.Collisions;

namespace TilemapGame.Screens
{
    /// <summary>
    /// Implements the main game logic
    /// </summary>
    public class GamePlayScreen : GameScreen
    {
        private Tilemap _tilemap;
        private Player _player;
        private List<Candy> _candies;
        private int _totalCollected = 0;
        private SoundEffect _candyCollectedSound;
        private SpriteFont _textFont;
        private CubeCandy cubeCandy;
        private SoundEffect _powerUpSound;
        private double powerUpTimer;
        private double powerUpTimeLimit = 8.0;
        private InputAction _pauseAction;
        private InputAction _restartAction;


        /// <summary>
        /// Constructor for the gameplay screen
        /// </summary>
        public GamePlayScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            _pauseAction = new InputAction(
                new[] { Buttons.Start, Buttons.Back },
                new[] { Keys.Back, Keys.Escape }, true);

            _restartAction = new InputAction(
                new[] { Buttons.Back },
                new[] { Keys.Enter }, true);
        }

        /// <summary>
        /// Activates the necessary assets and their content
        /// </summary>
        public override void Activate()
        {
            _tilemap = new Tilemap("TilemapDetails.txt");
            _player = new Player(ScreenManager.Game);
            _candies = new List<Candy>();
            CandySetup();
            _tilemap.LoadContent(ScreenManager.Game.Content);
            _player.LoadContent(ScreenManager.Game.Content);
            foreach (Candy c in _candies) c.LoadContent(ScreenManager.Game.Content);
            _candyCollectedSound = ScreenManager.Game.Content.Load<SoundEffect>("Candy_Pickup");
            _textFont = ScreenManager.Game.Content.Load<SpriteFont>("PressStart2P");
            cubeCandy = new CubeCandy(ScreenManager.Game, Matrix.Identity, new BoundingRectangle(400 + 15, 240 + 15, 30, 30));
            _powerUpSound = ScreenManager.Game.Content.Load<SoundEffect>("PowerUp");
            
            

            //ScreenManager.Game.ResetElapsedTime();
        }

        /// <summary>
        /// Deactivates the game play screen
        /// </summary>
        public override void Deactivate()
        {
            base.Deactivate();
        }

        /// <summary>
        /// Unloads the content present in the gameplay screen
        /// </summary>
        public override void Unload()
        {
            ScreenManager.Game.Content.Unload();
        }

        /// <summary>
        /// Updates the game play screen
        /// </summary>
        /// <param name="gameTime">The game time</param>
        /// <param name="otherScreenHasFocus">If a different screen has focus</param>
        /// <param name="coveredByOtherScreen">If this screen is covered by a different screen</param>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                ScreenManager.Game.Exit();

            // TODO: Add your update logic here
            _player.Update(gameTime);
            foreach (Candy c in _candies) c.Update(gameTime);
            for (int i = 0; i < _candies.Count; i++)
            {
                if (_player.CollidesWith(_candies[i].Bounds) && !_candies[i].Collected)
                {
                    _candies[i].Collected = true;
                    _candyCollectedSound.Play();
                    _totalCollected++;
                }
            }

            if (_tilemap.CollidesWith(_player.Bounds))
            {
                _player.EncounterWall = true;
            }
            else
            {
                _player.EncounterWall = false;
            }

            //Reset the collectables if they have all been collected
            if (_totalCollected % 9 == 0 && _totalCollected > 0)
            {
                CandySetup();
                foreach (Candy c in _candies) c.LoadContent(ScreenManager.Game.Content);
                if (cubeCandy == null) cubeCandy = new CubeCandy(ScreenManager.Game, Matrix.Identity, new BoundingRectangle(400 + 7, 240 + 7, 14, 14));
            }
            //If the player has the speed boost power up active
            if (_player.PowerUpActive)
            {
                powerUpTimer += gameTime.ElapsedGameTime.TotalSeconds;
                if (powerUpTimer > powerUpTimeLimit)
                {
                    _player.PowerUpActive = false;
                    powerUpTimer -= powerUpTimeLimit;

                }
            }
            //If the power up collectable is one screen
            if (cubeCandy != null)
            {
                cubeCandy.Update(gameTime);
                if (_player.CollidesWith(cubeCandy.Bounds))
                {
                    cubeCandy.Collected = true;
                    _powerUpSound.Play();
                    cubeCandy = null;
                    _player.PowerUpActive = true;
                }
            }
        }

        /// <summary>
        /// Handles the user input for the gameplay screen.
        /// 
        /// Currently need to fix a bug with the ship not moving left or right properly
        /// </summary>
        /// <param name="gameTime">The game time</param>
        /// <param name="input">The input from the user</param>
        public override void HandleInput(GameTime gameTime, InputState input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));


            // Look up inputs for the active player profile.
            int playerIndex = 1;

            var keyboardState = input.CurrentKeyboardStates[playerIndex];

            PlayerIndex player;
            if (_pauseAction.Occurred(input, ControllingPlayer, out player))
            {
                //Implement pause screen later
                //ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
            }
            else if (_restartAction.Occurred(input, ControllingPlayer, out player))
            {
                RestartGameplayScreen();
            }
            else
            {
                //add all the movement ish here
            }
        }

        /// <summary>
        /// Draws the gameplay screen's content
        /// </summary>
        /// <param name="gameTime">The game time</param>
        public override void Draw(GameTime gameTime)
        {
            var _spriteBatch = ScreenManager.SpriteBatch;
            ScreenManager.Game.GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            _tilemap.Draw(gameTime, _spriteBatch);
            _player.Draw(gameTime, _spriteBatch);
            foreach (Candy c in _candies) c.Draw(gameTime, _spriteBatch);
            _spriteBatch.DrawString(_textFont, "Use Arrows\n to Move", new Vector2(25, 25), Color.Gold);
            _spriteBatch.DrawString(_textFont, "Collect\n Candy", new Vector2(600, 25), Color.Gold);
            if (cubeCandy != null) cubeCandy.Draw();
            _spriteBatch.End();
            base.Draw(gameTime);

        }

        /// <summary>
        /// helper method to keep the work for manually adding the candy organized
        /// </summary>
        private void CandySetup()
        {
            int mass = 15;
            Vector2 velocity = new Vector2(0, 0);
            int size = 40;
            int radius = 25;
            _candies = new List<Candy>();
            _candies.Add(new Candy()
            {
                Center = new Vector2(400, 50),
                Velocity = velocity,
                Mass = mass,
                Bounds = new BoundingCircle(new Vector2(400 + (size / 2), 50 + (size / 2)), radius)
            });
            _candies.Add(new Candy()
            {
                Center = new Vector2(125, 100),
                Velocity = velocity,
                Mass = mass,
                Bounds = new BoundingCircle(new Vector2(125 + (size / 2), 100 + (size / 2)), radius)
            });
            _candies.Add(new Candy()
            {
                Center = new Vector2(675, 100),
                Velocity = velocity,
                Mass = mass,
                Bounds = new BoundingCircle(new Vector2(675 + (size / 2), 100 + (size / 2)), radius)
            });
            _candies.Add(new Candy()
            {
                Center = new Vector2(400, 160),
                Velocity = velocity,
                Mass = mass,
                Bounds = new BoundingCircle(new Vector2(400 + (size / 2), 160 + (size / 2)), radius)
            });
            _candies.Add(new Candy()
            {
                Center = new Vector2(400, 300),
                Velocity = velocity,
                Mass = mass,
                Bounds = new BoundingCircle(new Vector2(400 + (size / 2), 300 + (size / 2)), radius)
            });
            _candies.Add(new Candy()
            {
                Center = new Vector2(25, 330),
                Velocity = velocity,
                Mass = mass,
                Bounds = new BoundingCircle(new Vector2(25 + (size / 2), 330 + (size / 2)), radius)
            });
            _candies.Add(new Candy()
            {
                Center = new Vector2(775, 330),
                Velocity = velocity,
                Mass = mass,
                Bounds = new BoundingCircle(new Vector2(775 + (size / 2), 330 + (size / 2)), radius)
            });
            _candies.Add(new Candy()
            {
                Center = new Vector2(145, 450),
                Velocity = velocity,
                Mass = mass,
                Bounds = new BoundingCircle(new Vector2(145 + (size / 2), 450 + (size / 2)), radius)
            });
            _candies.Add(new Candy()
            {
                Center = new Vector2(655, 450),
                Velocity = velocity,
                Mass = mass,
                Bounds = new BoundingCircle(new Vector2(655 + (size / 2), 450 + (size / 2)), radius)
            });
        }

        /// <summary>
        /// Helper method for resetting the gameplay screen.
        /// Includes resetting ship position, asteroids, and the boolean state variables for the game
        /// </summary>
        private void RestartGameplayScreen()
        {
            LoadingScreen.Load(ScreenManager, true, null, new BackgroundScreen(), new GamePlayScreen());
            this.Deactivate();
        }
    }
}
