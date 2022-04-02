/**
 * Starting code taken from tilemap exercise created by Nathan Bean
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using TilemapGame.Collisions;

namespace TilemapGame
{
    public class Tilemap
    {
        /// <summary>
        /// The dimensions of tiles and the map
        /// </summary>
        int _tileWidth, _tileHeight, _mapWidth, _mapHeight;

        /// <summary>
        /// The tileset texture
        /// </summary>
        Texture2D _tilesetTexture;

        /// <summary>
        /// The tile info in the tileset
        /// </summary>
        Rectangle[] _tiles;

        /// <summary>
        /// The bounds for the tiles with collision
        /// </summary>
        BoundingRectangle[] _tilesBounds;

        /// <summary>
        /// The tile map data
        /// </summary>
        int[] _map;

        /// <summary>
        /// The filename of the map file
        /// </summary>
        string _filename;

        /// <summary>
        /// Constructor for TileMap
        /// </summary>
        /// <param name="file">The file the tilemap info is from</param>
        public Tilemap(string file)
        {
            _filename = file;
        }

        /// <summary>
        /// Loads the tilemap content
        /// </summary>
        /// <param name="content">The content manager being used</param>
        public void LoadContent(ContentManager content)
        {
            var test = Path.Join(content.RootDirectory, _filename);
            string data = File.ReadAllText(Path.Join(content.RootDirectory, _filename));
            var lines = data.Split('\n');

            //First line is the tileset filename
            var tilesetFilename = lines[0].Trim();
            _tilesetTexture = content.Load<Texture2D>(tilesetFilename);

            //Second line is the tile size
            var secondLine = lines[1].Split(',');
            _tileWidth = int.Parse(secondLine[0]);
            _tileHeight = int.Parse(secondLine[1]);

            //Now we can determine our tile bounds
            int tilesetColumns = _tilesetTexture.Width / _tileWidth;
            int tilesetRows = _tilesetTexture.Height / _tileHeight;
            _tiles = new Rectangle[tilesetColumns * tilesetRows];
            for (int y = 0; y < tilesetColumns; y++)
            {
                for (int x = 0; x < tilesetRows; x++)
                {
                    int index = y * tilesetColumns + x;
                    _tiles[index] = new Rectangle(
                        x * _tileWidth,
                        y * _tileHeight,
                        _tileWidth,
                        _tileHeight
                        );
                }
            }

            //Third line is the mape size
            var thirdLine = lines[2].Split(',');
            _mapWidth = int.Parse(thirdLine[0]);
            _mapHeight = int.Parse(thirdLine[1]);

            //Now we can create our map
            var fourthLine = lines[3].Split(',');
            _map = new int[_mapWidth * _mapHeight];
            for (int i = 0; i < _mapWidth * _mapHeight; i++)
            {
                _map[i] = int.Parse(fourthLine[i]);
            }

            _tilesBounds = new BoundingRectangle[_mapWidth * _mapHeight];
            for (int y = 0; y < _mapHeight; y++)
            {
                for(int x = 0; x < _mapWidth; x++)
                {
                    int index = y * _mapWidth + x;
                    _tilesBounds[index] = new BoundingRectangle(x * 16, y * 16, 16, 16);
                }
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (int y = 0; y < _mapHeight; y++)
            {
                for (int x = 0; x < _mapWidth; x++)
                {
                    int index = _map[y * _mapWidth + x];
                    if (index == -1) continue;
                    spriteBatch.Draw(
                        _tilesetTexture,
                        new Vector2(
                            x * _tileWidth,
                            y * _tileHeight
                            ),
                        _tiles[index],
                        Color.White
                        );
                }
            }
        }

        public bool CollidesWith(BoundingRectangle player)
        {
            for (int y = 0; y < _mapHeight; y++)
            {
                for (int x = 0; x < _mapWidth; x++)
                {
                    int index = y * _mapWidth + x;
                    if (_map[index] == -1) continue;
                    if (this._tilesBounds[index].CollidesWith(player)) return true;
                }
            }
            return false;
        }
    }
}
