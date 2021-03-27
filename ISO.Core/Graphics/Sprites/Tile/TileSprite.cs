using ISO.Core.Graphics.Sprites.Atlas;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ISO.Core.Graphics.Sprites.Tile
{
    public class TileSprite : AtlasSprite
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public float Depth { get; set; }

        public TileSprite(Texture2D texture, int positionX, int positionY, int width, int height, int row, int column, int layer) : base(texture, positionX, positionY, width, height)
        {
            Row = row;
            Column = column;
            Depth = layer / 100f + (row + column) / 1000f;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, destinationRectangle, sourceRectangle, Color, 0 , Vector2.Zero, SpriteEffects.None, Depth);
        }
    }
}
