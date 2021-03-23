using ISO.Core.Engine.Logging;
using ISO.Core.Loading;
using ISO.Core.Scenes;
using ISO.Core.UI.Elements;
using ISO.Core.UI.Elements.Base;
using ISO.Core.UI.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace ISO.Core.UI
{
    public class UIManager
    {
        private List<IUI> UIHolder { get; set; }

        public UILoader UILoader { get; set; }
        public LoadingManager Loader { get; set; }
        public GraphicsDevice Device { get; }
        public int MapID { get; }
        private UIHelper uIHelper { get; set; }

        public UIManager(int ID, string dbPath, LoadingManager loader, GraphicsDevice device)
        {
            Log.Info("Creating UI manager", LogModule.CR);
            MapID = ID;
            Loader = loader;
            Device = device;
            UIHolder = new List<IUI>();
            UILoader = new UILoader(this, dbPath);
        }

        internal void LoadContentForUIS()
        {
            foreach (var element in UIHolder)
            {
                element.LoadContent(Loader);
            }
        }


        /// <summary>
        /// Returns 
        /// </summary>
        public UIControl GetUIOnPosition(Point position)
        {
            return uIHelper.GetUIOnPosition(position);
        }


        public void AddUI(IUI uiElement)
        {
            UIHolder.Add(uiElement);
        }

        public void RemoveUI(string name)
        {
            var element = UIHolder.FirstOrDefault(x => x.Name == name);
            UIHolder.Remove(element);
        }

        internal void Update(GameTime gameTime)
        {
            foreach (var element in UIHolder)
            {
                element.Update(gameTime);
            }
        }

        internal void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (var element in UIHolder)
            {
                element.Draw(gameTime, spriteBatch);
            }
        }

        internal void LoadContent(ISOGame game)
        {
            UILoader.LoadUIContent();
        }

        internal void AfterLoad(LoadingManager manager)
        {
            UILoader.AfterLoad();
            foreach (var element in UIHolder)
            {
                element.AfterLoad(Loader);
            }
            UIHolder.Reverse();


            uIHelper = new UIHelper(UIHolder);
        }

    }
}
