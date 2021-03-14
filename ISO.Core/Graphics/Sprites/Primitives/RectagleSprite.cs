using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ISO.Core.Graphics.Sprites.Primitives
{
    public class RectagleSprite : Sprite
    {
        /// <summary>
        /// Rectangle instead Vector2
        /// </summary>
        public Rectangle RectangleDestination { get; set; } = new Rectangle(0, 0, 1, 1);


        public RectagleSprite(Color color, GraphicsDevice device, Rectangle destinationRectangle)
        {
            Texture = new Texture2D(device, 1, 1);
            Texture.SetData(new Color[] { color });
            RectangleDestination = destinationRectangle;
        }


        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, RectangleDestination, Color);
        }

    }
}
