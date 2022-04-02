/**
 * Starting code is based on Sprite Example assignment created by Nathan Bean
 */

using System;
using System.Collections.Generic;
using System.Text;
using TilemapGame.Collisions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace TilemapGame
{
    /// <summary>
    /// A class for the player character
    /// </summary>
    public class Player
    {
        private KeyboardState keyboardState;
        private Texture2D texture;
        private bool flipped;
        private bool jump;
        private Vector2 position = new Vector2(200, 150);
        private BoundingRectangle bounds;
        private Game game;
        private Vector2 direction;
        private double timer;
        private int frameCount;
        private bool encounterWall;
        private Vector2 lastEncounter = new Vector2(-100, -100);

        /// <summary>
        /// The position of the player sprite
        /// </summary>
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public bool EncounterWall
        {
            get { return encounterWall; }
            set { encounterWall = value; }
        }

        /// <summary>
        /// The collision bounds of the sprite
        /// </summary>
        public BoundingRectangle Bounds => bounds;

        /// <summary>
        /// Vector for the center of the space ship
        /// </summary>
        public Vector2 Center { get; set; }

        public Vector2 Direction
        {
            get { return direction; }
            set { direction = value; }
        }

        /// <summary>
        /// Vector for the velocity of the space ship
        /// </summary>
        public Vector2 Velocity { get; set; }

        /// <summary>
        /// Constructor for the player sprite
        /// </summary>
        /// <param name="game">The game being player</param>
        /// <param name="bounds">The collision bounds of the player</param>
        public Player(Game game)
        {
            this.game = game;
            this.direction = new Vector2(0, 0);
            this.bounds = new BoundingRectangle(position.X - 8, position.Y - 8, 15, 15);
        }

        /// <summary>
        /// Loads the sprite texture using the provided ContentManager
        /// </summary>
        /// <param name="content">The ContentManager to load with</param>
        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("Player_Black");
        }

        /// <summary>
        /// Updates the sprite's position based on user input
        /// </summary>
        /// <param name="gameTime">The GameTime</param>
        public void Update(GameTime gameTime)
        {
            keyboardState = Keyboard.GetState();
            timer += gameTime.ElapsedGameTime.TotalSeconds;
            double limit = 0.2;
            // Apply keyboard movement
            if (!encounterWall)
            {
                if (keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W))
                {
                    jump = true;
                    position += new Vector2(0, -1) * 100 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                if (keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.S))
                {
                    jump = false;
                    position += new Vector2(0, 1) * 100 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                if (keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A))
                {
                    position += new Vector2(-1, 0) * 100 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    flipped = true;

                    if (timer > limit)
                    {
                        timer -= limit;
                        frameCount++;
                        if (frameCount > 3) frameCount = 0;
                    }
                }
                if (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D))
                {
                    position += new Vector2(1, 0) * 100 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    flipped = false;
                    if (timer > limit)
                    {
                        timer -= limit;
                        frameCount++;
                        if (frameCount > 3) frameCount = 0;
                    }
                }
            }
            else
            {
                if(flipped) position -= new Vector2(-1, 0) * 100 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                if(!flipped) position -= new Vector2(1, 0) * 100 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                if(jump) position -= new Vector2(0, -1) * 100 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (!jump) position -= new Vector2(0, 1) * 100 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            // Wrap the player to keep it on-screen
            var viewport = game.GraphicsDevice.Viewport;
            if (position.Y < 0) position.Y = viewport.Height;
            if (position.Y > viewport.Height) position.Y = 0;
            if (position.X < 0) position.X = viewport.Width;
            if (position.X > viewport.Width) position.X = 0;

            bounds.X = position.X - 8;
            bounds.Y = position.Y - 8;
        }

        /// <summary>
        /// Draws the sprite using the supplied SpriteBatch
        /// </summary>
        /// <param name="gameTime">The game time</param>
        /// <param name="spriteBatch">The spritebatch to render with</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            
            var source = new Rectangle(frameCount * 15, 0, 15, 15);
            SpriteEffects spriteEffects = (flipped) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            spriteBatch.Draw(texture, position, source, Color.White, 0, new Vector2(16, 16), 2.0f, spriteEffects, 0);
        }

        /// <summary>
        /// Detects if there has been a collision, particularly with an candy sprite
        /// </summary>
        /// <param name="circle">The bounding circle being detected</param>
        /// <returns>If the ship has collided with the circle</returns>
        public bool CollidesWith(BoundingCircle circle)
        {
            return this.bounds.CollidesWith(circle);
        }
    }
}
