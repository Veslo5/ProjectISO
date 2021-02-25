using ISO.Core.Logging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace ISO.Core.Sprites
{
    /// <summary>
    /// Sprite
    /// </summary>
    public class Sprite
    {

        /// <summary>
        /// Position
        /// </summary>
        public Vector2 Position { get; set; } = new Vector2(0, 0);

        /// <summary>
        /// Color tint
        /// </summary>
        public Color Color { get; set; } = Color.White;

        /// <summary>
        /// Monogame Texture
        /// </summary>
        public Texture2D Texture { get; set; }

        public Sprite(Texture2D texture)
        {
            Texture = texture;       
            //Log.Info("New Sprite loaded with texture " + texture.Width + "x" + texture.Height);            
        }

        public Sprite()
        {
            Log.Warning("New Sprite loaded without texture");
        }


        /// <summary>
        /// Draw function
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, Color);
        }

    }
}
