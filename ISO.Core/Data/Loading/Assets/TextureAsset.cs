using Microsoft.Xna.Framework.Graphics;

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
