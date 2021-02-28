﻿using ISO.Core.Camera;
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
        /// Scene ID from db
        /// </summary>
        public int ID { get; }

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

        public Scene(string name, int id, ISOGame game, bool enableLuaScripting)
        {
            Name = name;
            ID = id;
            Game = game;

            LuaProvider = new LuaProvider(game.Config.DataPath, id, enableLuaScripting);
            LuaProvider.AddScript(Name);

            Map = new ISOTiledManager(id, game.Config.DataPath);
            UI = new UIManager(ID, Game.Config.DataPath);
            Corountines = new CorountineManager();
        }

        public virtual void Initialize()
        {
            Log.Info("Initializing scene " + Name);
            this.Game.GraphicsDevice.DeviceReset += GraphicsDevice_DeviceReset;
            this.Game.Window.ClientSizeChanged += Window_ClientSizeChanged;
            LuaProvider.InvokeFunctionFromScript(Name, INIT_NAME);
        }

        

        public virtual void LoadContent()
        {
            Log.Info("Loading content from scene " + Name);

            SpriteBatch = new SpriteBatch(Game.GraphicsDevice);
            Camera = new OrthographicCamera(Game.GraphicsDevice.Viewport);
            UICamera = new OrthographicCamera(Game.GraphicsDevice.Viewport);
            Map.LoadContent(Game.GraphicsDevice, Camera, Game.Content);
            UI.LoadContent(Game);
            LuaProvider.InvokeFunctionFromScript(Name, LOADCONTENT_NAME);

        }

        public virtual void Update(GameTime gameTime)
        {
            Camera.Update(); //TODO: Optimize and cache vector (change only when device change resolution)            

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


        private void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            Camera.OnResolutionChange(Game.GraphicsDevice.Viewport);
            UICamera.OnResolutionChange(Game.GraphicsDevice.Viewport);
        }

        #endregion

    }
}