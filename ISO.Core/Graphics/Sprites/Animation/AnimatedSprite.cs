using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace ISO.Core.Graphics.Sprites.Animation
{
    public class AnimatedSprite : ISO.Core.Graphics.Sprites.Atlas.Atlas
    {
        public float TimePerFrame { get; set; }
        private int currentFrame { get; set; }
        private int totalFrames { get; set; }

        private Rectangle _destinationRectangle;

        public Rectangle DestinationRectangle
        {
            get { return _destinationRectangle; }
            set
            {
                foreach (var image in AtlasSprites)
                {
                    image.destinationRectangle = value;
                }

                _destinationRectangle = value;
            }
        }


        public AnimatedSprite(Texture2D texture, int columns, int rows, float timePerFrame = 100f) : base(texture, columns, rows)
        {
            currentFrame = 0;
            totalFrames = columns * rows;
            TimePerFrame = timePerFrame;
        }

        public void Update(GameTime gameTime)
        {
            currentFrame = (int)(gameTime.TotalGameTime.TotalMilliseconds / TimePerFrame);
            currentFrame = currentFrame % totalFrames;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            AtlasSprites[currentFrame].Draw(gameTime, spriteBatch);
        }

    }
}
