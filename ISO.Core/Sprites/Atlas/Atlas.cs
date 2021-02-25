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

        public int Width { get; set; }
        public int Height { get; set; }

        private int CurrentRow { get; set; }

        public List<AtlasSprite> AtlasSprites { get; set; } = new List<AtlasSprite>();

        public Atlas(Texture2D texture, int columns, int rows)
        {
            Columns = columns;
            Rows = rows;

            Width = texture.Width / Columns;
            Height = texture.Height / Rows;

            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < columns; x++)
                {
                    AtlasSprites.Add(new AtlasSprite(texture, x, y, Width, Height));
                }
            }
        }

    }
}
