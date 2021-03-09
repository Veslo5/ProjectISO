using ISO.Core.StateManager;
using ISO.Core.UI.Elements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using ISO.Core.Logging;
using ISO.Core.Settings;

namespace ISO.Core.UI
{
    public class UIManager
    {
        private List<IUI> UIHolder { get; set; }

        public UILoader UILoader { get; set; }
        
        public int MapID { get;}

        public UIManager(int ID, string dbPath)
        {
            Log.Info("Creating UI manager");
            MapID = ID;

            UIHolder = new List<IUI>();
            UILoader = new UILoader(this, dbPath);
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
            foreach (var element in UIHolder)
            {
                element.LoadContent(game);
            }
        }

        internal void AfterLoad()
        {
            //throw new NotImplementedException();
        }
    }
}
