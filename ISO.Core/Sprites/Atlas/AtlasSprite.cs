using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace ISO.Core.Sprites.Atlas
{
    public class AtlasSprite : Sprite
    {

        public Rectangle sourceRectangle { get; set; }
        public Rectangle destinationRectangle { get; set; }
        public int PositionX { get; }
        public int PositionY { get; }


        public AtlasSprite(Texture2D texture, int positionX, int positionY, int width, int height) : base(texture)
        {
            PositionX = positionX;
            PositionY = positionY;

            sourceRectangle = new Rectangle(width * positionX, height * positionY, width, height);
            destinationRectangle = new Rectangle((int)Position.X, (int)Position.Y, width, height);            
        }


        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, destinationRectangle, sourceRectangle , Color);
        }


    }
}
