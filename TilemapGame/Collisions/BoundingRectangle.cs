/**
 * Code taken from Collision Example assignment by Nathan Bean
 */

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;


namespace TilemapGame.Collisions
{
    /// <summary>
    /// A bounding rectangle for collision detection
    /// </summary>
    public struct BoundingRectangle
    {
        public float X;

        public float Y;

        public float Width;

        public float Height;

        public float Left => X;

        public float Right => X + Width;

        public float Top => Y;

        public float Bottom => Y + Height;

        /// <summary>
        /// Constructor for the bounding rectangle
        /// </summary>
        /// <param name="x">The center X coordinate</param>
        /// <param name="y">The center Y coordinate</param>
        /// <param name="width">The width</param>
        /// <param name="height">The height</param>
        public BoundingRectangle(float x, float y, float width, float height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Constructor for the bounding rectangle
        /// </summary>
        /// <param name="position">The starting position</param>
        /// <param name="width">The width</param>
        /// <param name="height">The height</param>
        public BoundingRectangle(Vector2 position, float width, float height)
        {
            X = position.X;
            Y = position.Y;
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Determines if the bounding rectangle has colliding with another rectangle
        /// </summary>
        /// <param name="other">The other bounding rectangle</param>
        /// <returns>True if they have collided</returns>
        public bool CollidesWith(BoundingRectangle other)
        {
            return CollisionHelper.Collides(this, other);
        }

        /// <summary>
        /// Determines if the bounding rectangle has colliding with another circle
        /// </summary>
        /// <param name="other">The other bounding circle</param>
        /// <returns>True if they have collided</returns>
        public bool CollidesWith(BoundingCircle other)
        {
            return CollisionHelper.Collides(other, this);
        }
    }
}
