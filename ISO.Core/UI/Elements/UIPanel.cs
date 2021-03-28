using ISO.Core.Engine.Logging;
using ISO.Core.Graphics.Sprites;
using ISO.Core.Graphics.Sprites.Primitives;
using ISO.Core.Graphics.Sprites.SliceNine;
using ISO.Core.Loading;
using ISO.Core.Loading.Assets;
using ISO.Core.UI.Elements.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISO.Core.UI.Elements
{
    public class UIPanel : UIControl, IUI
    {
        public const string CONTENT_NAME = "UIPANEL_";

        private Sprite background { get; set; }
        private GraphicsDevice device { get; }

        public string ResourcePath { get; set; }

        public List<UIControl> Childs { get; set; } = new List<UIControl>();

        public UIPanel(string name, GraphicsDevice device)
        {
            Log.Info("Creating UI Panel " + name, LogModule.UI);
            Name = name;
            this.device = device;
        }

        public void LoadContent(LoadingController manager)
        {
            DimensionsRectangle = new Rectangle(Position.X, Position.Y, Size.X, Size.Y);

            if (!string.IsNullOrEmpty(ResourcePath))
                manager.Load<TextureAsset>(CONTENT_NAME + Name, ResourcePath);

            Childs = Childs.OrderBy(x => x.ZIndex).ToList(); // Make Sure that ZIndex is sorted by the right way
            foreach (var child in Childs)
            {
                var childUI = child as IUI;
                childUI.LoadContent(manager);
            }
        }

        public void AfterLoad(LoadingController manager)
        {
            if (string.IsNullOrEmpty(ResourcePath))
            {
                background = new RectagleSprite(Color, device, DimensionsRectangle);
            }
            else
            {
                if (manager.GetTexture(CONTENT_NAME + Name).Texture != null)
                {
                    var slicedSprite = new SlicedSprite(Color, manager.GetTexture(CONTENT_NAME + Name).Texture, new Padding(6, 6, 6, 6));
                    slicedSprite.DestinationRectangle = DimensionsRectangle;
                    background = slicedSprite;
                }
                else
                {
                    background = new RectagleSprite(Color.Magenta, device, DimensionsRectangle);
                }

            }

            foreach (var child in Childs)
            {
                var childUI = child as IUI;
                childUI.AfterLoad(manager);
            }

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
