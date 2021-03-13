using ISO.Core.Engine.Logging;
using ISO.Core.Graphics.Sprites.Primitives;
using ISO.Core.Loading;
using ISO.Core.UI.Elements.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace ISO.Core.UI.Elements
{
    public class ISOPanel : UIControl, IUI
    {
        private ISORectagle background { get; set; }
        private GraphicsDevice device { get; }

        public ISOPanel(string name, GraphicsDevice device)
        {
            Log.Info("Creating UI Panel " + name);
            Name = name;
            this.device = device;
        }

        public void LoadContent(LoadingManager manager)
        {
            background = new ISORectagle(Color, device, new Rectangle(Position.X, Position.Y, Size.X, Size.Y));
        }

        public void AfterLoad(LoadingManager manager)
        {

        }
        public void Update(GameTime gameTime)
        {

        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            background.Draw(gameTime, spriteBatch);
        }


    }
}
