using ISO.Core.Logging;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ISO.Core.DataLoader
{
    public class RawDataLoader
    {
        /// <summary>
        /// Reads raw file and create texture
        /// </summary>
        /// <param name="device">device which reads texture</param>
        /// <param name="path">path to file</param>
        /// <returns></returns>
        public static Texture2D GetTextureFromFile(GraphicsDevice device, string path)
        {
            Log.Info("Loading texture from Raw data " + path );

            Texture2D texture;
            using (var stream = new FileStream(path, FileMode.Open))
            {
               texture = Texture2D.FromStream(device, stream);
            }

            return texture;
        }
    }
}
