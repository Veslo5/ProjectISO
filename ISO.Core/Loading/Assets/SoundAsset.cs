using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Text;

namespace ISO.Core.Loading.Assets
{
    public class SoundAsset : AssetBase
    {
        public SoundAsset(string path)
        {
            base.Path = path;
        }

        public SoundEffect Sound { get; set; }
    }
}
