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
using System.Text;

namespace ISO.Core.UI.Elements
{
    public class UIButton : UIControl, IUI
    {
        public const string CONTENT_NAME = "UIBUTTON_";

        private GraphicsDevice device { get; }
        private UIText text { get; }
        private Sprite background { get; set; }
        private SpriteFont font { get; set; }
        public string ResourcePath { get; set; }

        public UIButton(string name, GraphicsDevice device)
        {
            Log.Info("Creating UI Button " + name, LogModule.UI);
            this.device = device;

            this.text = new UIText(name + "_text");
            this.text.Parent = this;
        }

        public void LoadContent(LoadingController manager)
        {
            DimensionsRectangle = new Rectangle(Position.X, Position.Y, Size.X, Size.Y);

            if (!string.IsNullOrEmpty(ResourcePath))
                manager.Load<TextureAsset>(CONTENT_NAME + Name, ResourcePath);

            text.LoadContent(manager);


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

            text.AfterLoad(manager);

        }

        public void Update(GameTime gameTime)
        {
            text.Update(gameTime);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            background.Draw(gameTime, spriteBatch);
            text.Draw(gameTime, spriteBatch);
        }
    }
}
