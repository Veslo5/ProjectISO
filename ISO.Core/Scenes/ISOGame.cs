using ISO.Core.Data.DataLoader;
using ISO.Core.Graphics;
using ISO.Core.Input;
using ISO.Core.Settings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace ISO.Core.Scenes
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
        /// Input manager
        /// </summary>
        public InputManager Input { get; set; }

        /// <summary>
        /// App configuration
        /// </summary>
        public Config Config { get; }

        public ISOGame(Config config)
        {
            Config = config;
            SceneManager = new SceneManager(this);
            Graphics = new ISOGraphicsManager(this, config.Width, config.Height, config.Vsync, config.FrameCap);
            Input = new InputManager();

            Content = new ISOContentManager(base.Content.ServiceProvider);
            Content.RootDirectory = "Content";

            IsMouseVisible = true;
        }

        /// <summary>
        /// Initialize
        /// </summary>
        protected override void Initialize()
        {
            Graphics.ChangeResolution(Graphics.CurrentWidth, Graphics.CurrentHeight, this.Config.Fullscreen);

            GraphicsDevice.DeviceReset += GraphicsDevice_DeviceReset;
            Window.ClientSizeChanged += Window_ClientSizeChanged;


            SceneManager.CurrentScene.Initialize();
            base.Initialize();
        }

        /// <summary>
        /// Resize of window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            SceneManager.CurrentScene.Window_ClientSizeChanged();
        }

        /// <summary>
        /// Resolution change event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GraphicsDevice_DeviceReset(object sender, EventArgs e)
        {
            SceneManager.CurrentScene.GraphicsDevice_DeviceReset();
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
            SceneManager.CurrentScene.BeforeUpdate(gameTime); // We have to ensure that our code go before overrideable update
            SceneManager.CurrentScene.Update(gameTime);            
            SceneManager.CurrentScene.AfterUpdate(gameTime); // We have to ensure that our code go after overrideable update
        }


        /// <summary>
        /// Unload
        /// </summary>
        protected override void UnloadContent()
        {
            SceneManager.CurrentScene.UnloadContent();
        }

        /// <summary>
        /// Draw
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Draw(GameTime gameTime)
        {
            SceneManager.CurrentScene.Draw(gameTime);            
        }
    }
}
