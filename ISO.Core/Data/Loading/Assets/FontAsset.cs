using Microsoft.Xna.Framework.Graphics;

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
