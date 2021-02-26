using ISO.Core.DataLoader.SqliteClient;
using ISO.Core.DataLoader.SqliteClient.Contracts;
using ISO.Core.Logging;
using ISO.Core.UI.Elements;
using ISO.Core.UI.JSONModels;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ISO.Core.UI
{
    public class UILoader
    {
        private UIManager Manager { get; }
        private string Dbpath { get; }

        public UILoader(UIManager manager, string dbpath)
        {
            Log.Info("UILoader loaded.");

            Manager = manager;
            Dbpath = dbpath;
        }


        /// <summary>
        /// Loads UI json and create UI elements from it
        /// </summary>
        /// <param name="name"></param>
        public void LoadJson(int mapID)
        {
            ISOUI uiData = null;

            Log.Info("Loading UI file " + mapID);
            using (var context = new ISODbContext(Dbpath))
            {
                uiData = context.LoadTForMap<ISOUI>(mapID);
            }

            if (uiData == null)
            {
                Log.Warning("UI file was not found. Continuing without UI.");
                return;
            }

            var jsonString = uiData.DATA;

            var deserializedJSON = JsonConvert.DeserializeObject<UIRoot>(jsonString);

            foreach (var item in deserializedJSON.Texts)
            {
                CreateText(item);
            }

            //TODO: clean uiData to null            
        }

        /// <summary>
        /// Creates text elements
        /// </summary>
        /// <param name="text"></param>
        private void CreateText(ISOUIText text)
        {
            var ISOTextitem = new ISOText(text.Name, text.Text);
            ISOTextitem.Position = new Vector2(text.X, text.Y);
            ISOTextitem.Color = new Color(text.R, text.G, text.B);

            Manager.AddUI(ISOTextitem);

        }


    }

}
