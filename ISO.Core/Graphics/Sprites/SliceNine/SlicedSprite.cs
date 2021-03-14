using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace ISO.Core.Graphics.Sprites.SliceNine
{
    public class SlicedSprite : Sprite
    {
        private Rectangle destinationRectangle;

        public Rectangle DestinationRectangle
        {
            get { return destinationRectangle; }
            set
            {
                destinationRectangle = value;
                destinationPatches = createPatches(value, Padding);
            }
        }

        public Padding Padding { get; set; }

        private Rectangle[] sourcePatches { get; set; } // patches on texture
        private Rectangle[] destinationPatches { get; set; } // destination rectangle on screen

        public SlicedSprite(Color color, Texture2D texture, Padding padding) : base(texture)
        {
            Color = color;
            Padding = padding;

            sourcePatches = createPatches(texture.Bounds, Padding);
        }

        private Rectangle[] createPatches(Rectangle rectangle, Padding padding)
        {
            var x = rectangle.X;
            var y = rectangle.Y;
            var w = rectangle.Width;
            var h = rectangle.Height;
            var middleWidth = w - padding.LeftPadding - padding.RightPadding;
            var middleHeight = h - padding.TopPadding - padding.BottomPadding;
            var bottomY = y + h - padding.BottomPadding;
            var rightX = x + w - padding.RightPadding;
            var leftX = x + padding.LeftPadding;
            var topY = y + padding.TopPadding;
            var patches = new[]
            {
                 new Rectangle(x,      y,        padding.LeftPadding,  padding.TopPadding),      // top left
                 new Rectangle(leftX,  y,        middleWidth,  padding.TopPadding),              // top middle
                 new Rectangle(rightX, y,        padding.RightPadding, padding.TopPadding),      // top right
                 new Rectangle(x,      topY,     padding.LeftPadding,  middleHeight),            // left middle
                 new Rectangle(leftX,  topY,     middleWidth,  middleHeight),                    // middle
                 new Rectangle(rightX, topY,     padding.RightPadding, middleHeight),            // right middle
                 new Rectangle(x,      bottomY,  padding.LeftPadding,  padding.BottomPadding),   // bottom left
                 new Rectangle(leftX,  bottomY,  middleWidth,  padding.BottomPadding),           // bottom middle
                 new Rectangle(rightX, bottomY,  padding.RightPadding, padding.BottomPadding)    // bottom right
            };
            return patches;
        }


        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (int i = 0; i < sourcePatches.Length; i++)
            {
                spriteBatch.Draw(Texture, destinationPatches[i], sourcePatches[i], Color);
            }
        }
    }

    public class Padding
    {

        public static Padding DefaultPadding = new Padding(0, 0, 0, 0);

        public Padding(int leftPadding = 0, int rightPadding = 0, int topPadding = 0, int bottomPadding = 0)
        {
            LeftPadding = leftPadding;
            RightPadding = rightPadding;
            TopPadding = topPadding;
            BottomPadding = bottomPadding;
        }

        public int LeftPadding { get; set; }
        public int RightPadding { get; set; }
        public int TopPadding { get; set; }
        public int BottomPadding { get; set; }
    }

}
