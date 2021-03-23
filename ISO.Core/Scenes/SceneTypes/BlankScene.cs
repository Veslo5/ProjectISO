using ISO.Core.Corountines;
using ISO.Core.Engine.Camera;
using ISO.Core.Engine.Logging;
using ISO.Core.Loading;
using ISO.Core.Scripting;
using ISO.Core.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ISO.Core.Scenes.SceneTypes
{
    public class BlankScene : Scene, IScene
    {
        #region Constructor
        public BlankScene(string name, int id, ISOGame game, bool enableLuaScripting) : base(game, name, id)
        {

        }
        #endregion

        #region LoopEvents

        public virtual void Initialize()
        {
            Log.Info("Initializing scene " + Name, LogModule.CR);

            SpriteBatch = new SpriteBatch(Game.GraphicsDevice);

            Camera = new OrthographicCamera(Game.GraphicsDevice.Viewport);
            UICamera = new OrthographicCamera(Game.GraphicsDevice.Viewport);

            LuaProvider = new LuaManager(Game.Config.DataPath, ID, false);
            LuaProvider.AddScript(Name);

            LoadingManager = new LoadingManager(Game.Content);
            UI = new UIManager(ID, Game.Config.DataPath, LoadingManager, Game.GraphicsDevice);
            Corountines = new CorountineManager();

        }

        public virtual void LoadContent()
        {
            Log.Info("Loading content from scene " + Name, LogModule.LO);

            UI.LoadContent(Game);
            LuaProvider.InvokeLoad(Name);
        }
        public virtual void AfterLoadContent(LoadingManager manager)
        {
            UI.AfterLoad(manager);
        }

        public virtual void Update(GameTime gameTime)
        {
            Camera.Update(); //TODO: Optimize and cache vector (change only when device change resolution)            
            UICamera.Update(); //TODO: Optimize and cache vector (change only when device change resolution)

            Corountines.Update(gameTime);
            UI.Update(gameTime);

            LuaProvider.InvokeUpdate(Name);
        }

        public virtual void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin(transformMatrix: UICamera.Projection);
            UI.Draw(gameTime, SpriteBatch);
            SpriteBatch.End();

            LuaProvider.InvokeDraw(Name);

            Game.Window.Title = "FPS " + 1 / gameTime.ElapsedGameTime.TotalSeconds;
        }

        public virtual void UnloadContent()
        {
            using (Process proc = Process.GetCurrentProcess())
            {
                Log.Unique("Total process memory:  " + proc.PrivateMemorySize64 / (1024f * 1024f) + " MB");
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

        }

        /// <summary>
        /// Window size changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Window_ClientSizeChanged()
        {
            Log.Info("Client size changed on scene " + Name, LogModule.CR);

            Camera.OnResolutionChange(Game.GraphicsDevice.Viewport);
            UICamera.OnResolutionChange(Game.GraphicsDevice.Viewport);
        }

        #endregion
    }
}
