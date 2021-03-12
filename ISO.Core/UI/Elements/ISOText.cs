using ISO.Core.Engine.Logging;
using ISO.Core.Loading;
using ISO.Core.Loading.Assets;
using ISO.Core.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;

namespace ISO.Core.UI.Elements
{
    public class ISOText : IUI
    {
        /// <summary>
        /// Name of UI element
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Text color
        /// </summary>
        public Color Color { get; set; } = Color.White;

        /// <summary>
        /// Text position
        /// </summary>
        public Vector2 Position { get; set; } = new Vector2(0, 0);

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


        public ISOText(string name, string text = "", string fontName = "Default", int maxLineWidth = 1000)
        {
            Log.Info("Creating UI Text " + name);
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
            spriteBatch.DrawString(font, Text, Position, Color);
        }

        public void Update(GameTime gameTime)
        {
        }

        public void LoadContent(LoadingManager manager)
        {           
            manager.Load<FontAsset>(fontName, fontName);
        }

        public void AfterLoad(LoadingManager manager)
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
