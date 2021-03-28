using Microsoft.Xna.Framework.Content;
using System;
using System.IO;
using System.IO.Compression;

namespace ISO.Core.Data.DataLoader
{
    public class ISOContentProvider : ContentManager
    {
        public ISOContentProvider(IServiceProvider provider) : base(provider)
        {

        }

        /// <summary>
        /// Possibility to open stream from ZIP
        /// </summary>
        /// <param name="assetName">asset name with path</param>
        /// <returns></returns>
        protected override Stream OpenStream(string assetName)
        {
            MemoryStream stream = null;

            // Replace because of this: https://docs.microsoft.com/en-us/dotnet/framework/migration-guide/mitigation-ziparchiveentry-fullname-path-separator
            var xnbName = Path.Combine(RootDirectory, assetName + ".xnb").Replace('\\', '/');

            using (ZipArchive zip = ZipFile.Open(RootDirectory + ".zip", ZipArchiveMode.Read))
            {
                foreach (ZipArchiveEntry entry in zip.Entries)
                {
                    if (entry.FullName == xnbName)
                    {
                        using (var entryStream = entry.Open())
                        {
                            stream = new MemoryStream();

                            entryStream.CopyTo(stream);

                        }

                        stream.Seek(0, SeekOrigin.Begin);

                    }
                }

            }

            return stream;

        }

        /// <summary>
        /// Remove cached file from content manager
        /// </summary>
        /// <param name="assetName"></param>
        public void RemoveCachedAsset(string assetName)
        {
            this.LoadedAssets.Remove(assetName);
        }

        /// <summary>
        /// Check if asset is already loaded in memory
        /// </summary>
        /// <param name="assetName"></param>
        /// <returns></returns>
        public bool IsAssetCached(string assetName) => this.LoadedAssets.ContainsKey(assetName);

    }
}
