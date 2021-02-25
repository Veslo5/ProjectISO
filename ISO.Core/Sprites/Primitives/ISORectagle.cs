using ISO.Core.StateManager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace ISO.Core.Sprites.Primitives
{
    public class ISORectagle : Sprite
    {
        /// <summary>
        /// Rectangle instead Vector2
        /// </summary>
        public Rectangle RectangleDestination { get; set; } = new Rectangle(0, 0, 1, 1);


        public ISORectagle(Color color, GraphicsDevice device,  Rectangle destinationRectangle)
        {
            Texture = new Texture2D(device, 1, 1);
            Texture.SetData(new Color[] { color });
            RectangleDestination  = destinationRectangle;
        }


        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, RectangleDestination, Color);
        }

    }
}
