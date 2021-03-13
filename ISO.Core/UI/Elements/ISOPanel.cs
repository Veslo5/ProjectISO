using ISO.Core.Engine.Logging;
using ISO.Core.Graphics.Sprites.Primitives;
using ISO.Core.Loading;
using ISO.Core.UI.Elements.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISO.Core.UI.Elements
{
    public class ISOPanel : UIControl, IUI
    {
        private ISORectagle background { get; set; }
        private GraphicsDevice device { get; }

        public List<UIControl> Childs { get; set; } = new List<UIControl>();

        public ISOPanel(string name, GraphicsDevice device)
        {
            Log.Info("Creating UI Panel " + name);
            Name = name;
            this.device = device;
        }

        public void LoadContent(LoadingManager manager)
        {
            DimensionsRectangle = new Rectangle(Position.X, Position.Y, Size.X, Size.Y);
            background = new ISORectagle(Color, device, DimensionsRectangle);

            Childs = Childs.OrderBy(x => x.ZIndex).ToList();

            foreach (var child in Childs)
            {
                var childUI = child as IUI;
                childUI.LoadContent(manager);
            }
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

            foreach (var child in Childs)
            {
                var childUI = child as IUI;
                childUI.Draw(gameTime, spriteBatch);
            }

        }


    }
}
