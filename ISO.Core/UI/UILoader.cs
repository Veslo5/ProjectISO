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
        private string DefaultPath { get; }
        private UIManager Manager { get; }

        public UILoader(string path, UIManager manager)
        {
            Log.Info("UILoader loaded.");

            if (string.IsNullOrEmpty(path))
            {
                DefaultPath = AppDomain.CurrentDomain.BaseDirectory + "\\UI";
            }
            else
            {
                DefaultPath = path;
            }

            Manager = manager;
        }


        /// <summary>
        /// Loads UI json and create UI elements from it
        /// </summary>
        /// <param name="name"></param>
        public void LoadJson(string name)
        {
            var filePath = DefaultPath + "\\" + name + ".ui";
            Log.Info("Loading UI file " + filePath);

            if (!File.Exists(filePath))
            {
                Log.Warning("UI file was not found. Continuing without UI.");
                return;
            }

            var jsonString = File.ReadAllText(filePath);

            var deserializedJSON = JsonConvert.DeserializeObject<UIRoot>(jsonString);

            foreach (var item in deserializedJSON.Texts)
            {
                CreateText(item);
            }

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
