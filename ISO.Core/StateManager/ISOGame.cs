using System;
using System.Collections.Generic;
using System.Text;
using ISO.Core.DataLoader;
using ISO.Core.Graphics;
using ISO.Core.Logging;
using ISO.Core.Settings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ISO.Core.StateManager
{
    /// <summary>
    /// GameState Manager
    /// </summary>
    public class ISOGame : Game
    {
        /// <summary>
        /// Graphic Management
        /// </summary>
        public ISOGraphicsManager Graphics { get; set; }

        /// <summary>
        /// Scene management
        /// </summary>
        public SceneManager SceneManager { get; set; }

        /// <summary>
        /// Content
        /// </summary>
        public new ISOContentManager Content { get; set; }

        /// <summary>
        /// App configuration
        /// </summary>
        public Config Config { get; }

        public ISOGame(Config config)
        {
            Config = config;
            SceneManager = new SceneManager(this);
            Graphics = new ISOGraphicsManager(this, config.Width, config.Height, config.Vsync);

            Content = new ISOContentManager(base.Content.ServiceProvider);
            Content.RootDirectory = "Content";

            IsMouseVisible = true;
        }

        /// <summary>
        /// Initialize
        /// </summary>
        protected override void Initialize()
        {
            Graphics.ChangeResolution(Graphics.CurrentWidth, Graphics.CurrentHeight, false);

            SceneManager.CurrentScene.Initialize();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent 
        /// </summary>
        protected override void LoadContent()
        {
            SceneManager.CurrentScene.LoadContent();
        }

        /// <summary>
        /// Update 
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Update(GameTime gameTime)
        {
            SceneManager.CurrentScene.Update(gameTime);
            base.Update(gameTime);
        }


        /// <summary>
        /// Unload
        /// </summary>
        protected override void UnloadContent()
        {
            SceneManager.CurrentScene.LoadContent();
        }

        /// <summary>
        /// Draw
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Draw(GameTime gameTime)
        {
            SceneManager.CurrentScene.Draw(gameTime);
            base.Draw(gameTime);
        }
    }
}
