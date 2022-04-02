﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using TilemapGame.Collisions;

namespace TilemapGame
{
    public class TilemapGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Tilemap _tilemap;
        private Player _player;
        private List<Candy> _candies;

        public TilemapGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.PreferredBackBufferWidth = 800;
            _graphics.PreferredBackBufferHeight = 480;
            _graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _tilemap = new Tilemap("TilemapDetails.txt");
            _player = new Player(this);
            _candies = new List<Candy>();
            CandySetup(); 

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            _tilemap.LoadContent(Content);
            _player.LoadContent(Content);
            foreach (Candy c in _candies) c.LoadContent(Content);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            _player.Update(gameTime);
            foreach (Candy c in _candies) c.Update(gameTime);
            for(int i = 0; i < _candies.Count; i++)
            {
                if (_player.CollidesWith(_candies[i].Bounds) && !_candies[i].Collected)
                {
                    _candies[i].Collected = true;
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

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            _tilemap.Draw(gameTime, _spriteBatch);
            _player.Draw(gameTime, _spriteBatch);
            foreach (Candy c in _candies) c.Draw(gameTime, _spriteBatch);
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
                Bounds = new BoundingCircle(new Vector2(400 + (size /2), 50 + (size / 2)), radius)
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
    }
}