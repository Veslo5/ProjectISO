using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace ISO.Core.Loading.Assets
{
    public class FontAsset : AssetBase
    {
        public FontAsset(string path)
        {
            base.Path = path;
        }

        public SpriteFont Font { get; set; }
    }
}
