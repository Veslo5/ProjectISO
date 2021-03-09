using ISO.Core.Graphics.Sprites.Atlas;
using Microsoft.Xna.Framework.Graphics;

namespace ISO.Core.Graphics.Sprites.Tile
{
    public class TileSprite : AtlasSprite
    {
        public int Row { get; set; }
        public int Column { get; set; }

        public TileSprite(Texture2D texture, int positionX, int positionY, int width, int height, int row, int column) : base(texture, positionX, positionY, width, height)
        {
            Row = row;
            Column = column;
        }

    }
}
