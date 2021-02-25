using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace ISO.Core.Contents
{
    public class ISOContentManager : ContentManager
    {
        public ISOContentManager(IServiceProvider provider) : base(provider)
        {

        }

        protected override Stream OpenStream(string assetName)
        {
            MemoryStream stream = null;

            var xnbName = RootDirectory + "/" + assetName + ".xnb";
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
    }
}
