﻿using ISO.Core.Engine.Logging;
using ISO.Core.Loading;
using ISO.Core.Loading.Assets;
using ISO.Core.Scenes;
using ISO.Core.UI.Elements.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;

namespace ISO.Core.UI.Elements
{
    public class UIText : UIControl, IUI
    {
        /// <summary>
        /// maxline width
        /// </summary>
        public int MaxLineWidth { get; set; }


        /// <summary>
        /// Text of element
        /// </summary>
        private string text;
        public string Text
        {
            get { return text; }
        }

        /// <summary>
        /// Name of font
        /// </summary>
        private string fontName { get; set; }

        /// <summary>
        /// Loaded font 
        /// </summary>
        private SpriteFont font { get; set; }


        public UIText(string name, string text = "", string fontName = "Default", int maxLineWidth = 1000)
        {
            Log.Info("Creating UI Text " + name, LogModule.UI);   
            Name = name;
            this.fontName = "Fonts/" + fontName;
            this.text = text;
            this.MaxLineWidth = maxLineWidth;
        }


        /// <summary>
        /// Set text 
        /// </summary>
        /// <param name="text"></param>
        public void SetText(string text)
        {
            this.text = WrapText(text);
        }


        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, Text, Position.ToVector2(), Color);
        }

        public void Update(GameTime gameTime)
        {
        }

        public void LoadContent(LoadingController manager)
        {           
            manager.Load<FontAsset>(fontName, fontName);
        }

        public void AfterLoad(LoadingController manager)
        {
            this.font = manager.GetFont(this.fontName).Font;
            this.SetText(Text);
        }


        /// <summary>
        /// Text wrapping
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private string WrapText(string text)
        {
            if (font.MeasureString(text).X < MaxLineWidth)
            {
                return text;
            }

            string[] words = text.Split(' ');
            StringBuilder wrappedText = new StringBuilder();
            float linewidth = 0f;
            float spaceWidth = font.MeasureString(" ").X;
            for (int i = 0; i < words.Length; ++i)
            {
                Vector2 size = font.MeasureString(words[i]);
                if (linewidth + size.X < MaxLineWidth)
                {
                    linewidth += size.X + spaceWidth;
                }
                else
                {
                    wrappedText.Append("\n");
                    linewidth = size.X + spaceWidth;
                }
                wrappedText.Append(words[i]);
                wrappedText.Append(" ");
            }

            return wrappedText.ToString();
        }

    }
}
