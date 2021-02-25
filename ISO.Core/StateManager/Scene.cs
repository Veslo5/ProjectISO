using ISO.Core.Camera;
using ISO.Core.Corountines;
using ISO.Core.Logging;
using ISO.Core.Scripting;
using ISO.Core.Tiled;
using ISO.Core.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace ISO.Core.StateManager
{

    /// <summary>
    /// Scene
    /// </summary>
    public class Scene
    {
        /// <summary>
        /// Init constant for script function
        /// </summary>
        private const string INIT_NAME = "Initialize";

        /// <summary>
        /// LoadContent constant for script function
        /// </summary>
        private const string LOADCONTENT_NAME = "LoadContent";

        /// <summary>
        /// Update constant for script function
        /// </summary>
        private const string UPDATE_NAME = "Update";

        /// <summary>
        /// Draw constant for script function
        /// </summary>
        private const string DRAW_NAME = "Draw";

        /// <summary>
        /// Scene & Script Name
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Spritebatch for rendering
        /// </summary>
        public SpriteBatch SpriteBatch { get; set; }

        /// <summary>
        /// Game reference
        /// </summary>
        public ISOGame Game { get; }

        /// <summary>
        /// Lua Scripting provider
        /// </summary>
        public LuaProvider LuaProvider { get; set; }

        /// <summary>
        /// Camera
        /// </summary>
        public OrthographicCamera Camera { get; set; }

        /// <summary>
        /// Camera for UI
        /// </summary>
        private OrthographicCamera UICamera { get; set; }

        /// <summary>
        /// UI manager
        /// </summary>
        public UIManager UI { get; set; }

        /// <summary>
        /// Corountines Manager
        /// </summary>
        public CorountineManager Corountines { get; set; }

        /// <summary>
        /// Map manager
        /// </summary>
        public ISOTiledManager Map { get; set; }

        public Scene(string name, ISOGame game, bool enableLuaScripting)
        {
            Name = name;
            Game = game;

            LuaProvider = new LuaProvider(game.ScriptRoothPath, enableLuaScripting);
            LuaProvider.AddScript(Name);

            Map = new ISOTiledManager("sdExport");
            UI = new UIManager(game.UIRootPath);
            Corountines = new CorountineManager();
        }

        public virtual void Initialize()
        {
            Log.Info("Initializing scene " + Name);
            this.Game.GraphicsDevice.DeviceReset += GraphicsDevice_DeviceReset;
            LuaProvider.InvokeFunctionFromScript(Name, INIT_NAME);
        }


        public virtual void LoadContent()
        {
            Log.Info("Loading content from scene " + Name);

            SpriteBatch = new SpriteBatch(Game.GraphicsDevice);
            Camera = new OrthographicCamera(Game.GraphicsDevice.Viewport);
            UICamera = new OrthographicCamera(Game.GraphicsDevice.Viewport);
            Map.LoadContent(Game.GraphicsDevice, Camera);
            UI.LoadContent(Game);
            LuaProvider.InvokeFunctionFromScript(Name, LOADCONTENT_NAME);

        }

        public virtual void Update(GameTime gameTime)
        {            
            Camera.Update(); //TODO: Optimize and cache vector (change only when device change resolution)

            UICamera.Position = new Vector2(Game.Graphics.CurrentWidth / 2, Game.Graphics.CurrentHeight / 2);
            UICamera.Update(); //TODO: Optimize and cache vector (change only when device change resolution)

            Map.Update();

            Corountines.Update(gameTime);
            UI.Update(gameTime);

            LuaProvider.InvokeFunctionFromScript(Name, UPDATE_NAME);
        }

        public virtual void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin(sortMode: SpriteSortMode.Deferred, blendState: BlendState.AlphaBlend, samplerState: SamplerState.PointClamp, transformMatrix: Camera.Projection);
            Map.Draw(gameTime, SpriteBatch);
            SpriteBatch.End();

            SpriteBatch.Begin(transformMatrix: UICamera.Projection);
            UI.Draw(gameTime, SpriteBatch);
            SpriteBatch.End();
            
            LuaProvider.InvokeFunctionFromScript(Name, DRAW_NAME);

            Game.Window.Title = "FPS " + (1 / gameTime.ElapsedGameTime.TotalSeconds);

        }

        public virtual void UnloadContent()
        {
            Log.Info("Unloading content from scene " + Name);

        }

        #region Events

        /// <summary>
        /// Resolution changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GraphicsDevice_DeviceReset(object sender, EventArgs e)
        {
            Log.Info("Resolution changed on scene " + Name);
            Camera.OnResolutionChange(Game.GraphicsDevice.Viewport);
            UICamera.OnResolutionChange(Game.GraphicsDevice.Viewport);

        }

        #endregion

    }
}
