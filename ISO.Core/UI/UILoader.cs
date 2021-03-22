using ISO.Core.Data.DataLoader.SqliteClient;
using ISO.Core.Data.DataLoader.SqliteClient.Contracts;
using ISO.Core.Engine.Logging;
using ISO.Core.UI.Elements;
using ISO.Core.UI.Elements.Base;
using ISO.Core.UI.JSONModels;
using ISO.Core.UI.JSONModels.Base;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace ISO.Core.UI
{
    public class UILoader
    {
        private UIManager Manager { get; }
        private string Dbpath { get; }

        private ISOUI uiData { get; set; }

        public UILoader(UIManager manager, string dbpath)
        {            
            Manager = manager;
            Dbpath = dbpath;
        }

        public void LoadUIContent()
        {
            Manager.Loader.LoadCallback("UI", LoadJson);
        }

        public void AfterLoad()
        {
        }


        /// <summary>
        /// Loads UI json and create UI elements from it
        /// </summary>
        /// <param name="name"></param>
        public void LoadJson()
        {
            Log.Info("Loading UI file " + Manager.MapID, LogModule.UI);
            using (var context = new ISODbContext(Dbpath))
            {
                uiData = context.LoadTForMap<ISOUI>(Manager.MapID);
            }

            if (uiData == null)
            {
                Log.Warning("UI file was not found. Continuing without UI.");
                return;
            }

            BuildUI();

            Manager.LoadContentForUIS();

            //TODO: clean uiData to null            
        }



        public void BuildUI()
        {
            var jsonString = uiData.DATA;

            var deserializedJSON = JsonConvert.DeserializeObject<UIRoot>(jsonString);

            foreach (var control in deserializedJSON.Controls)
            {
                AddControl(control as JObject, null); // Root does not have parents
            }

        }


        private void AddControl(JObject control, UIControl parent)
        {
            var typeInt = control.GetValue("Type").Value<int>();
            var type = (JType)typeInt;

            switch (type)
            {
                case JType.TEXT:
                    CreateText(control.ToObject<JText>(), parent);
                    break;
                case JType.PANEL:
                    CreatePanel(control.ToObject<JPanel>(), parent);
                    break;
            }
        }

        /// <summary>
        /// Creates text elements
        /// </summary>
        /// <param name="text"></param>
        private void CreateText(JText text, UIControl parent)
        {
            var ISOTextitem = new UIText(text.Name, text.Text);

            if (parent != null)
                ISOTextitem.Parent = parent;
            else
                Manager.AddUI(ISOTextitem);

            ISOTextitem.Position = new Point(text.X, text.Y);
            ISOTextitem.Color = new Color(text.R, text.G, text.B);
            ISOTextitem.ZIndex = text.ZIndex;

            Manager.AddUI(ISOTextitem);

        }

        private void CreatePanel(JPanel panel, UIControl parent)
        {

            var isoPanel = new UIPanel(panel.Name, Manager.Device);

            if (parent != null)
            {
                isoPanel.Parent = parent;               
                (parent as UIPanel).Childs.Add(isoPanel);
            }
            else
            {
                Manager.AddUI(isoPanel);
            }

            isoPanel.Position = new Point(panel.X, panel.Y);
            isoPanel.Size = new Point(panel.Width, panel.Height);
            isoPanel.Color = new Color(panel.R, panel.G, panel.B);
            isoPanel.ZIndex = panel.ZIndex;
            isoPanel.Path = panel.Path;

            if (panel.Controls != null)
            {
                foreach (var control in panel.Controls) // Recursive add every UI in panel 
                {
                    AddControl(control as JObject, isoPanel);
                }
            }

        }

    }

}
