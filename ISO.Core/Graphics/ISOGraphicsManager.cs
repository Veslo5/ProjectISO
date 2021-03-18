using ISO.Core.Engine.Logging;
using Microsoft.Xna.Framework;
using System;

namespace ISO.Core.Graphics
{
    /// <summary>
    /// ISO Graphics manager 
    /// </summary>
    public class ISOGraphicsManager : GraphicsDeviceManager
    {

        /// <summary>
        /// Current window Width
        /// </summary>
        public int CurrentWidth { get; set; }

        /// <summary>
        /// Current window height
        /// </summary>
        public int CurrentHeight { get; set; }

        public bool Vsync { get; set; }

        public int FrameCap { get; }

        public ISOGraphicsManager(Game game, int width, int height, bool vsync, int frameCap) : base(game)
        {
            CurrentWidth = width;
            CurrentHeight = height;
            Vsync = vsync;
            FrameCap = frameCap;

            this.GraphicsProfile = Microsoft.Xna.Framework.Graphics.GraphicsProfile.Reach;
            this.PreferMultiSampling = false;                        
            
            if (vsync == false)
            {
                this.SynchronizeWithVerticalRetrace = false;
            }
            else
            {
                this.SynchronizeWithVerticalRetrace = true;
            }


            if (frameCap > 0)
            {
                game.IsFixedTimeStep = true;
                game.TargetElapsedTime = TimeSpan.FromSeconds(1d / FrameCap);

            }
            else
            {
                game.IsFixedTimeStep = false;
            }
        }


        /// <summary>
        /// Change resolution by parameteres and apply changes
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="fullscreen"></param>
        public void ChangeResolution(int width, int height, bool fullscreen)
        {
            Log.Info("Changing resolution to " + width + " " + height);

            this.PreferredBackBufferWidth = width;
            this.PreferredBackBufferHeight = height;
            this.SynchronizeWithVerticalRetrace = Vsync;

            CurrentWidth = width;
            CurrentHeight = height;

            float scaleX = this.PreferredBackBufferWidth / width;
            float scaleY = this.PreferredBackBufferHeight / height;

            this.IsFullScreen = fullscreen;

            this.ApplyChanges();

        }
    }
}
