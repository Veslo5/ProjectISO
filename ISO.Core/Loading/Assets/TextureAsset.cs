using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace ISO.Core.Loading.Assets
{
    public class TextureAsset : AssetBase
    {
        public TextureAsset(string path)
        {
            base.Path = path;
        }

        public Texture2D Texture { get; set; }
    }
}
