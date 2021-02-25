using ISO.Core.Sprites.Atlas;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace ISO.Core.Sprites.Tile
{
    public class TileSprite : AtlasSprite
    {
        public int Row { get; set; }
        public int Column { get; set; }

        public TileSprite(Texture2D texture, int positionX, int positionY, int width, int height, int row, int column) : base (texture, positionX, positionY, width, height)
        {
            Row = row;  
            Column = column;
        }

    }
}
