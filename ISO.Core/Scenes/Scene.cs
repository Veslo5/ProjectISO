using ISO.Core.Corountines;
using ISO.Core.Engine.Camera;
using ISO.Core.Loading;
using ISO.Core.Scripting;
using ISO.Core.UI;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace ISO.Core.Scenes
{
    public abstract class Scene
    {

        /// <summary>
        /// Scene & Script Name,
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Scene ID from db
        /// </summary>
        public int ID { get; set; }

        public Scene(ISOGame game, string name, int id)
        {
            Game = game;
            ID = id;
            Name = name;
        }

        /// <summary>
        /// Default game Camera
        /// </summary>
        public OrthographicCamera Camera { get; set; }

        /// <summary>
        /// UI Camera
        /// </summary>
        public OrthographicCamera UICamera { get; set; }

        /// <summary>
        /// Game Reference
        /// </summary>
        public ISOGame Game { get; }

        /// <summary>
        /// Lua Scripting provider
        /// </summary>
        public LuaManager LuaProvider { get; set; }

        /// <summary>
        /// Loading manager
        /// </summary>
        public LoadingManager LoadingManager { get; set; }

        /// <summary>
        /// Spritebatch for rendering
        /// </summary>
        public SpriteBatch SpriteBatch { get; set; }

        /// <summary>
        /// Corountines Manager
        /// </summary>
        public CorountineManager Corountines { get; set; }

        /// <summary>
        /// UI manager
        /// </summary>
        public UIManager UI { get; set; }


    }
}
