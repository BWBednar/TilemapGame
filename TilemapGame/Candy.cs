using System;
using System.Collections.Generic;
using System.Text;
using TilemapGame.Collisions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TilemapGame
{
    /// <summary>
    /// Class representing a candy collectable
    /// </summary>
    public class Candy
    {
        private Texture2D _texture;
        private Vector2 origin;
        private float radius;
        private float scale;
        private double timer;
        private int rotationCount;
        private BoundingCircle bounds;
        private int colorChoice = new Random().Next(0, 2);

        /// <summary>
        /// Bounding circle for collision detection
        /// </summary>
        public BoundingCircle Bounds => bounds;

        /// <summary>
        /// If the asteroid has been destroyed
        /// </summary>
        public bool Collected = false;

        /// <summary>
        /// Indicates if the candy has collided with anything
        /// </summary>
        public bool Colliding { get; set; }

        /// <summary>
        /// Vector for the center of the candy
        /// </summary>
        public Vector2 Center { get; set; }

        /// <summary>
        /// Vector for the velocity of the candy
        /// </summary>
        public Vector2 Velocity { get; set; }

        /// <summary>
        /// The mass of the candy sprite
        /// </summary>
        public float Mass
        {
            get => radius;
            set
            {
                radius = value;
                scale = radius / 20;
                origin = new Vector2(20, 20); // candy sprite is 40 by 40 pixels
            }
        }

        /// <summary>
        /// Constructor for the candy sprite, establishes bounding circle
        /// </summary>
        public Candy()
        {
            this.bounds = new BoundingCircle(Center + new Vector2(-20, -20), 40);
        }

        /// <summary>
        /// Loads the image the candy sprite is contained in
        /// </summary>
        /// <param name="contentManager">The game's content manager</param>
        public void LoadContent(ContentManager contentManager)
        {
            if (colorChoice == 0) _texture = contentManager.Load<Texture2D>("lollipop_blue");
            else _texture = contentManager.Load<Texture2D>("lollipop_red");
        }

        /// <summary>
        /// Updates the movement of the candy
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            if (Collected) return;
            Center += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

            //Keeps the asteroid on screen, will eventually want it to travel to the opposite end of the screen
            /*
            if (Center.X < radius || Center.X > Constants.GAME_WIDTH - radius) Velocity *= -Vector2.UnitX;
            if (Center.Y < radius || Center.Y > Constants.GAME_HEIGHT - radius) Velocity *= -Vector2.UnitY;
            bounds.Center.X = this.Center.X;
            bounds.Center.Y = this.Center.Y;
            Colliding = false;
            */
        }

        /// <summary>
        /// Draws the candy sprite, rotates sprite in a circle slightly
        /// </summary>
        /// <param name="gameTime">The game time</param>
        /// <param name="spriteBatch">The sprite batch</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (Collected) return; //Do not draw if the candy has been collected

            timer += gameTime.ElapsedGameTime.TotalSeconds;
            if (timer > 0.1)
            {
                timer -= 0.1;
                if (colorChoice == 0) rotationCount++;
                else rotationCount--;
            }
            Rectangle source = new Rectangle(0, 0, 40, 40);
            double roation = (float)rotationCount * 0.1;
            spriteBatch.Draw(_texture, Center, source, Color.White, (float)roation, origin, scale, SpriteEffects.None, 0);
        }
    }
}
