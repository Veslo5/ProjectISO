using ISO.Core.Camera;
using ISO.Core.Corountines;
using ISO.Core.Loading;
using ISO.Core.Logging;
using ISO.Core.Scripting;
using ISO.Core.Tiled;
using ISO.Core.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ISO.Core.StateManager.Scene
{

    /// <summary>
    /// Scene
    /// </summary>
    public class MapScene : IScene
    {
        #region SceneInfo

        /// <summary>
        /// Scene & Script Name,
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Scene ID from db
        /// </summary>
        public int ID { get; }

        #endregion

        /// <summary>
        /// Spritebatch for rendering
        /// </summary>
        public SpriteBatch SpriteBatch { get; set; }

        /// <summary>
        /// Game reference
        /// </summary>
        public ISOGame Game { get; }

        #region Managers

        /// <summary>
        /// Lua Scripting provider
        /// </summary>
        public LuaManager LuaProvider { get; set; }

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

        /// <summary>
        /// Loading manager
        /// </summary>
        public LoadingManager LoadingManager { get; set; }

        #endregion

        #region Constructor
        public MapScene(string name, int id, ISOGame game, bool enableLuaScripting)
        {
            Name = name;
            ID = id;
            Game = game;


        }
        #endregion

        #region LoopEvents
        public virtual void Initialize()
        {
            SpriteBatch = new SpriteBatch(Game.GraphicsDevice);

            Camera = new OrthographicCamera(Game.GraphicsDevice.Viewport);
            UICamera = new OrthographicCamera(Game.GraphicsDevice.Viewport);

            LuaProvider = new LuaManager(Game.Config.DataPath, ID, false);
            LuaProvider.AddScript(Name);

            LoadingManager = new LoadingManager(Game.Content);
            Map = new ISOTiledManager(ID, Game.Config.DataPath, Camera, LoadingManager);
            UI = new UIManager(ID, Game.Config.DataPath);
            Corountines = new CorountineManager();

            Log.Info("Initializing scene " + Name);
            LuaProvider.InvokeInit(Name);

        }

        public virtual void LoadContent()
        {
            Log.Info("Loading content from scene " + Name);

            Map.LoadContent();
            UI.LoadContent(Game);
            LuaProvider.InvokeLoad(Name);

        }

        public virtual void AfterLoadContent()
        {
            Map.AfterLoad();
            UI.AfterLoad();
        }

        public virtual void Update(GameTime gameTime)
        {
            Camera.Update(); //TODO: Optimize and cache vector (change only when device change resolution)            

            UICamera.Update(); //TODO: Optimize and cache vector (change only when device change resolution)

            Map.Update();

            Corountines.Update(gameTime);
            UI.Update(gameTime);

            LuaProvider.InvokeUpdate(Name);
        }

        public virtual void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin(sortMode: SpriteSortMode.Deferred, blendState: BlendState.AlphaBlend, samplerState: SamplerState.PointClamp, transformMatrix: Camera.Projection);
            Map.Draw(gameTime, SpriteBatch);
            SpriteBatch.End();

            SpriteBatch.Begin(transformMatrix: UICamera.Projection);
            UI.Draw(gameTime, SpriteBatch);
            SpriteBatch.End();

            LuaProvider.InvokeDraw(Name);

            Game.Window.Title = "FPS " + (1 / gameTime.ElapsedGameTime.TotalSeconds);
        }

        public virtual void UnloadContent()
        {
            Log.Info("Unloading content from scene " + Name);


        }

        #endregion

        #region Events

        /// <summary>
        /// Resolution changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void GraphicsDevice_DeviceReset()
        {
            Log.Info("Resolution changed on scene " + Name);
            Camera.OnResolutionChange(Game.GraphicsDevice.Viewport);
            UICamera.OnResolutionChange(Game.GraphicsDevice.Viewport);

        }

        /// <summary>
        /// Window size changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Window_ClientSizeChanged()
        {
            Camera.OnResolutionChange(Game.GraphicsDevice.Viewport);
            UICamera.OnResolutionChange(Game.GraphicsDevice.Viewport);
        }

        #endregion

    }
}
