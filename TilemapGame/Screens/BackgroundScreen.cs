/**
 * Starting Code from Nathan Bean's GameArchitectureExample project
 */

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TilemapGame.StateManagement;

namespace TilemapGame.Screens
{
    public class BackgroundScreen : GameScreen
    {
        private ContentManager _content;
        private Texture2D _backgroundTexture;

        private double animationTimer;
        private int animationFrame;

        /// <summary>
        /// Constructor for the background screen
        /// </summary>
        public BackgroundScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }

        /// <summary>
        /// Loads graphics content for this screen. The background texture is quite
        /// big, so we use our own local ContentManager to load it. This allows us
        /// to unload before going from the menus into the game itself, whereas if we
        /// used the shared ContentManager provided by the Game class, the content
        /// would remain loaded forever.
        /// </summary>
        public override void Activate()
        {
            if (_content == null)
                _content = new ContentManager(ScreenManager.Game.Services, "Content");

            _backgroundTexture = _content.Load<Texture2D>("space_background");
        }

        /// <summary>
        /// Unloads the display when it is no longer needed
        /// </summary>
        public override void Unload()
        {
            _content.Unload();
        }

        // Unlike most screens, this should not transition off even if
        // it has been covered by another screen: it is supposed to be
        // covered, after all! This overload forces the coveredByOtherScreen
        // parameter to false in order to stop the base Update method wanting to transition off.
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);
        }

        /// <summary>
        /// Draws the background image.
        /// This background image should have some slight motion, so the timer is needed for this animation
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            animationTimer += gameTime.ElapsedGameTime.TotalSeconds;
            if (animationTimer > 0.2)
            {
                animationTimer -= 0.2;
                animationFrame++;
                if (animationFrame == 4) animationFrame = 0;
            }
            var source = new Rectangle(animationFrame * 64, 0, 64, 64);

            var spriteBatch = ScreenManager.SpriteBatch;
            spriteBatch.Begin();
            //fill the background with the animation
            for (int i = 0; i < Constants.GAME_HEIGHT * 10; i += 64)
            {
                for (int j = 0; j < Constants.GAME_WIDTH * 10; j += 64)
                {
                    spriteBatch.Draw(_backgroundTexture, new Rectangle(j, i, 64, 64), source, Color.White);
                }
            }
            spriteBatch.End();
        }
    }
}
