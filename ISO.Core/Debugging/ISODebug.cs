using ISO.Core.Corountines;
using ISO.Core.Engine.Logging;
using Microsoft.Xna.Framework.Graphics;
using System.Collections;

namespace ISO.Core.Debugging
{
    public class ISODebug
    {

        private static bool run = false;

        /// <summary>
        /// Start debug
        /// </summary>
        /// <param name="gd"></param>
        /// <param name="corountineManager"></param>
        public static void Start(GraphicsDevice gd, CorountineManager corountineManager)
        {
            run = true;
            corountineManager.StartCoroutine(WriteLog(gd));
        }


        /// <summary>
        /// Stop debug
        /// </summary>
        public static void Stop()
        {
            run = false;
        }


        /// <summary>
        /// Write log
        /// </summary>
        /// <param name="gd"></param>
        /// <returns></returns>
        private static IEnumerator WriteLog(GraphicsDevice gd)
        {
            while (run == true)
            {
                Log.Info("------------------DEBUG---------------");
                Log.Info("Draw count: " + gd.Metrics.DrawCount);
                Log.Info("Texture count: " + gd.Metrics.TextureCount);
                Log.Info("Sprite count: " + gd.Metrics.SpriteCount);
                Log.Info("Clear count: " + gd.Metrics.ClearCount);
                Log.Info("Target count: " + gd.Metrics.TargetCount);
                Log.Info("Primitive count: " + gd.Metrics.PrimitiveCount);
                Log.Info("Shader count: " + gd.Metrics.PixelShaderCount);
                Log.Info("-------------------END----------------");

                yield return 5000.0;
            }

        }


    }
}
