using ISO.Core.Corountines;
using ISO.Core.Engine.Camera;
using ISO.Core.Engine.Logging;
using ISO.Core.Loading;
using ISO.Core.Scripting;
using ISO.Core.Tiled;
using ISO.Core.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ISO.Core.Scenes.SceneTypes
{

    /// <summary>
    /// Scene
    /// </summary>
    public class MapScene : Scene,  IScene
    {
        #region Managers

        /// <summary>
        /// Map manager
        /// </summary>
        public ISOTiledManager Map { get; set; }

        #endregion

        #region Constructor
        public MapScene(string name, int id, ISOGame game, bool enableLuaScripting) : base(game ,name, id)
        {                    

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
            UI = new UIManager(ID, Game.Config.DataPath, LoadingManager, Game.GraphicsDevice);
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

        public virtual void AfterLoadContent(LoadingManager manager)
        {
            Map.AfterLoad(manager);
            UI.AfterLoad(manager);
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
            SpriteBatch.Begin(sortMode: SpriteSortMode.Deferred, blendState: BlendState.AlphaBlend, samplerState: SamplerState.PointClamp, transformMatrix: Camera.Projection, rasterizerState: RasterizerState.CullNone);
            Map.Draw(gameTime, SpriteBatch);
            SpriteBatch.End();

            SpriteBatch.Begin(transformMatrix: UICamera.Projection);
            UI.Draw(gameTime, SpriteBatch);
            SpriteBatch.End();

            LuaProvider.InvokeDraw(Name);

            Game.Window.Title = "FPS " + 1 / gameTime.ElapsedGameTime.TotalSeconds;
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
