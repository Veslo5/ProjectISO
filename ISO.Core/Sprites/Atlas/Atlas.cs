using ISO.Core.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace ISO.Core.Sprites.Atlas
{
    public class Atlas
    {
        private int Rows { get; set; }
        private int Columns { get; set; }

        public int TileWidth { get; set; }
        public int TileHeight { get; set; }

        public List<AtlasSprite> AtlasSprites { get; set; } = new List<AtlasSprite>();

        public Atlas(Texture2D texture, int columns, int rows)
        {
            Columns = columns;
            Rows = rows;

            cutTiles(texture, columns, rows);
        }

        private void cutTiles(Texture2D texture, int columns, int rows)
        {
            // Set tile informations
            TileWidth = texture.Width / Columns;
            TileHeight = texture.Height / Rows;

            for (int y = 0; y < rows; y++) // cuts all sprites in atlas
            {
                for (int x = 0; x < columns; x++)
                {
                    AtlasSprites.Add(new AtlasSprite(texture, x, y, TileWidth, TileHeight));
                }
            }
        }

    }
}
