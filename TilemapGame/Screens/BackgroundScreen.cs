/**
 * Starting Code from Nathan Bean's GameArchitectureExample project
 */

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TilemapGame.StateManagement;
using TilemapGame.ParticleSystem;

namespace TilemapGame.Screens
{
    public class BackgroundScreen : GameScreen, IParticleEmitter
    {
        private ContentManager _content;

        private SnowParticleSystem _snowFall;
        private Texture2D _background;

        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }

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

            _snowFall = new SnowParticleSystem(ScreenManager.Game, new Rectangle(-100, -20, Constants.GAME_WIDTH + 200, Constants.GAME_HEIGHT + 20));
            ScreenManager.Game.Components.Add(_snowFall);
            _background = _content.Load<Texture2D>("landscape");
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
            //ScreenManager.Game.GraphicsDevice.Clear(Color.CornflowerBlue);
            var spriteBatch = ScreenManager.SpriteBatch;
            spriteBatch.Begin();
            spriteBatch.Draw(_background, new Rectangle(0, 0, Constants.GAME_WIDTH, Constants.GAME_HEIGHT), Color.White);
            _snowFall.Draw(gameTime);
            spriteBatch.End();
        }
    }
}
