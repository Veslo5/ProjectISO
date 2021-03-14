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
        public const string CONTENT_NAME = "UIPANEL";

        private Sprite background { get; set; }
        private GraphicsDevice device { get; }

        public string Path { get; set; }

        public List<UIControl> Childs { get; set; } = new List<UIControl>();

        public UIPanel(string name, GraphicsDevice device)
        {
            Log.Info("Creating UI Panel " + name);
            Name = name;
            this.device = device;
        }

        public void LoadContent(LoadingManager manager)
        {
            DimensionsRectangle = new Rectangle(Position.X, Position.Y, Size.X, Size.Y);

            if (!string.IsNullOrEmpty(Path))
                manager.Load<TextureAsset>(CONTENT_NAME, Path);

            Childs = Childs.OrderBy(x => x.ZIndex).ToList();
            foreach (var child in Childs)
            {
                var childUI = child as IUI;
                childUI.LoadContent(manager);
            }
        }

        public void AfterLoad(LoadingManager manager)
        {
            if (string.IsNullOrEmpty(Path))
            {
                background = new RectagleSprite(Color, device, DimensionsRectangle);
            }
            else
            {
                var slicedSprite = new SlicedSprite(Color, manager.GetTexture(CONTENT_NAME).Texture, new Padding(6, 6, 6, 6));
                slicedSprite.DestinationRectangle = DimensionsRectangle;
                background = slicedSprite;
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
