using System;
using System.Collections.Generic;
using System.Text;

namespace ISO.Core.Loading.Assets
{
    public abstract class AssetBase
    {
        public string Path { get; set; }
        internal bool Unload { get; set; }
    }
}
