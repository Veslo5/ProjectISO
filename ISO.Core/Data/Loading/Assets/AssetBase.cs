namespace ISO.Core.Loading.Assets
{
    public abstract class AssetBase
    {
        public string Path { get; set; }
        internal bool Unload { get; set; }

        internal bool IsResourceCached { get; set; }
    }
}
