using ISO.Core.Data.DataLoader.SqliteClient;
using ISO.Core.Data.DataLoader.SqliteClient.Contracts;
using ISO.Core.Engine.Logging;
using ISO.Core.UI.Elements;
using ISO.Core.UI.JSONModels;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;

namespace ISO.Core.UI
{
    public class UILoader
    {
        private UIManager Manager { get; }
        private string Dbpath { get; }

        private ISOUI uiData { get; set; }

        public UILoader(UIManager manager, string dbpath)
        {
            Log.Info("UILoader loaded.");

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
            Log.Info("Loading UI file " + Manager.MapID);
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
