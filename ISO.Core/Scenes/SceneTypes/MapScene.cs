using ISO.Core.Corountines;
using ISO.Core.Engine.Camera;
using ISO.Core.Engine.Logging;
using ISO.Core.Loading;
using ISO.Core.Scripting;
using ISO.Core.Tiled;
using ISO.Core.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;

namespace ISO.Core.Scenes.SceneTypes
{

    /// <summary>
    /// Scene
    /// </summary>
    public class MapScene : Scene, IScene
    {
        #region Managers

        /// <summary>
        /// Map manager
        /// </summary>
        public ISOTiledManager Map { get; set; }

        #endregion

        #region Constructor
        public MapScene(string name, int id, ISOGame game, bool enableLuaScripting) : base(game, name, id)
        {

        }
        #endregion

        #region LoopEvents
        public virtual void Initialize()
        {
            Log.Info("Initializing scene " + Name, LogModule.CR);

            SpriteBatch = new SpriteBatch(Game.GraphicsDevice);

            Camera = new OrthographicCamera(Game.GraphicsDevice.Viewport, Game.Config.VirtualResolution);
            UICamera = new OrthographicCamera(Game.GraphicsDevice.Viewport);

            LuaProvider = new LuaManager(Game.Config.DataPath, ID, false);
            LuaProvider.AddScript(Name);

            LoadingManager = new LoadingController(Game.Content);
            Map = new ISOTiledManager(ID, Game.Config.DataPath, Camera, LoadingManager);
            UI = new UIManager(ID, Game.Config.DataPath, LoadingManager, Game.GraphicsDevice);
            Corountines = new CorountineManager();

            LuaProvider.InvokeInit(Name);

        }

        public virtual void LoadContent()
        {
            Log.Info("Loading content from scene " + Name, LogModule.LO);

            Map.LoadContent(LoadingManager);
            UI.LoadContent(LoadingManager);
            LuaProvider.InvokeLoad(Name);

        }

        public virtual void AfterLoadContent(LoadingController manager)
        {
            Map.AfterLoad(manager);
            UI.AfterLoad(manager);
        }

        /// <summary>
        /// WARNING: Do not make this overrideable!
        /// </summary>
        /// <param name="gameTime"></param>
        public void BeforeUpdate(GameTime gameTime)
        {            
            Game.Input.BeforeUpdate();
        }

        public virtual void Update(GameTime gameTime)
        {
            Camera.Update(); //TODO: Optimize and cache vector (change only when device change resolution)            
            UICamera.Update(); //TODO: Optimize and cache vector (change only when device change resolution)

            Map.Update(gameTime);

            Corountines.Update(gameTime);
            UI.Update(gameTime);

            LuaProvider.InvokeUpdate(Name);
        }

        /// <summary>
        /// WARNING: Do not make this overrideable!
        /// </summary>
        /// <param name="gameTime"></param>
        public void AfterUpdate(GameTime gameTime)
        {
            Game.Input.AfterUpdate();
            Game.Window.Title = "FPS " + 1 / gameTime.ElapsedGameTime.TotalSeconds;
        }

        public virtual void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin(sortMode: SpriteSortMode.FrontToBack, blendState: BlendState.AlphaBlend, samplerState: SamplerState.PointClamp, transformMatrix: Camera.Projection, rasterizerState: RasterizerState.CullNone);
            Map.Draw(gameTime, SpriteBatch);
            SpriteBatch.End();

            SpriteBatch.Begin(transformMatrix: UICamera.Projection);
            UI.Draw(gameTime, SpriteBatch);
            SpriteBatch.End();

            LuaProvider.InvokeDraw(Name);

        }

        public virtual void UnloadContent()
        {
            using (Process proc = Process.GetCurrentProcess())
            {
                Log.Unique("Total process memory:  " + proc.PagedMemorySize64 / (1024f * 1024f) + " MB");
            }

            Log.Unique("Heap memory before Unloading: " + GC.GetTotalMemory(false) / (1024f * 1024f) + " MB");
            LoadingManager.StartUnloading();
            GC.Collect();
            Log.Unique("Heap Memory after Unloading: " + GC.GetTotalMemory(false) / (1024f * 1024f) + " MB");

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
            Log.Info("Resolution changed on scene " + Name, LogModule.CR);
            Camera.OnResolutionChange(Game.GraphicsDevice.Viewport);
            UICamera.OnResolutionChange(Game.GraphicsDevice.Viewport);
            Map.OnResolutionChanged(Game.GraphicsDevice.Viewport);

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
            Map.OnResolutionChanged(Game.GraphicsDevice.Viewport);

        }


        #endregion

    }
}
