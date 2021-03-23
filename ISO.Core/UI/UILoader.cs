using ISO.Core.Data.DataLoader.SqliteClient;
using ISO.Core.Data.DataLoader.SqliteClient.Contracts;
using ISO.Core.Engine.Helpers.Extensions.ErrorHandling;
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
            Log.Info("Loading UI file with ID: " + Manager.MapID, LogModule.LO); // LO because it is callback from loading
            using (var context = new ISODbContext(Dbpath))
            {
                uiData = context.LoadTForMap<ISOUI>(Manager.MapID);
            }

            if (uiData == null)
            {
                Log.Warning("UI file was not found. Continuing without UI.", LogModule.UI);
                return;
            }


            BuildUI(GenerateTestJSON());

            //BuildUI(uiData.DATA); // build UI


            Manager.LoadContentForUIS();

            //TODO: clean uiData to null            
        }

        public void BuildUI(string inputJson)
        {
            Log.Info("Building UI", LogModule.UI);

            var jsonString = inputJson;
            var deserializedJSON = JsonConvert.DeserializeObject<UIRoot>(jsonString);

            foreach (var control in deserializedJSON.Controls)
            {
                AddControl(control as JObject, null); // Root does not have parents
            }

        }

        private void AddControl(JObject control, UIControl parent)
        {
            try
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
                    case JType.BUTTON:
                        CreateButton(control.ToObject<JButton>(), parent);
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.Error("Definition of UI Object is corrupted! Continuing on next element.", LogModule.UI);
                Log.Error(ex.GetAllInnerExceptions());
            }
        }

        private void CreateButton(JButton jButton, UIControl parent)
        {
            var isoButton = new UIButton(jButton.Name, Manager.Device);

            if (parent != null)
                isoButton.Parent = parent;
            else
                Manager.AddUI(isoButton);

            isoButton.Position = new Point(jButton.Position.X, jButton.Position.Y);
            isoButton.Color = new Color(jButton.Color.R, jButton.Color.G, jButton.Color.B, jButton.Color.A);
            isoButton.ZIndex = jButton.ZIndex;
            isoButton.ResourcePath = jButton.ResourcePath;

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

            ISOTextitem.Position = new Point(text.Position.X, text.Position.Y);
            ISOTextitem.Color = new Color(text.Color.R, text.Color.G, text.Color.B, text.Color.A);
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

            isoPanel.Position = new Point(panel.Position.X, panel.Position.Y);
            isoPanel.Size = new Point(panel.Scale.Width, panel.Scale.Height);
            isoPanel.Color = new Color(panel.Color.R, panel.Color.G, panel.Color.B, panel.Color.A);
            isoPanel.ZIndex = panel.ZIndex;
            isoPanel.ResourcePath = panel.ResourcePath;

            if (panel.Controls != null)
            {
                foreach (var control in panel.Controls) // Recursive add every UI in panel 
                {
                    AddControl(control as JObject, isoPanel);
                }
            }

        }

        private string GenerateTestJSON()
        {
            Log.Warning("UI is being builded from code, not from data!", LogModule.UI);

            var json = new UIRoot() { Controls = new System.Collections.Generic.List<object>() };
            var panel = new JPanel()
            {
                Name = "Panel",
                Color = new ISO.Core.UI.JSONModels.Base.JColor()
                {
                    R = 255,
                    G = 255,
                    B = 255,
                    A = 255
                },
                ResourcePath = "UI/Slice",
                Type = ISO.Core.UI.JSONModels.Base.JType.PANEL,
                Position = new ISO.Core.UI.JSONModels.Base.JPosition()
                {
                    X = 100,
                    Y = 100
                },
                Scale = new ISO.Core.UI.JSONModels.Base.JScale()
                {
                    Width = 100,
                    Height = 100,
                },
                Controls = new System.Collections.Generic.List<object>(),
                ZIndex = 1

            };

            var panel2 = new JPanel()
            {
                Name = "Panel2",
                Color = new ISO.Core.UI.JSONModels.Base.JColor()
                {
                    R = 255,
                    G = 255,
                    B = 255,
                    A = 255
                },
                ResourcePath = "UI/Slice9",
                Type = ISO.Core.UI.JSONModels.Base.JType.PANEL,
                Scale = new ISO.Core.UI.JSONModels.Base.JScale()
                {
                    Width = 80,
                    Height = 80,
                },
                Position = new ISO.Core.UI.JSONModels.Base.JPosition()
                {
                    X = 10,
                    Y = 10,
                },

                ZIndex = 0
            };

            var panel3 = new JPanel()
            {
                Name = "Panel3",
                Color = new ISO.Core.UI.JSONModels.Base.JColor()
                {
                    R = 255,
                    G = 255,
                    B = 255,
                    A = 255
                },
                ResourcePath = "UI/Slice9",
                Type = ISO.Core.UI.JSONModels.Base.JType.PANEL,
                Position = new ISO.Core.UI.JSONModels.Base.JPosition()
                {
                    X = 10,
                    Y = 10,
                },
                Scale = new ISO.Core.UI.JSONModels.Base.JScale()
                {
                    Width = 60,
                    Height = 60,
                },

                ZIndex = 1

            };



            var panel4 = new JPanel()
            {
                Name = "panel4",
                Color = new ISO.Core.UI.JSONModels.Base.JColor()
                {
                    R = 255,
                    G = 255,
                    B = 255,
                    A = 255
                },
                Type = ISO.Core.UI.JSONModels.Base.JType.PANEL,
                Position = new ISO.Core.UI.JSONModels.Base.JPosition()
                {
                    X = 150,
                    Y = 150,
                },
                Scale = new ISO.Core.UI.JSONModels.Base.JScale()
                {
                    Width = 100,
                    Height = 100,
                },
                ZIndex = 0,
                ResourcePath = "UI/Slice9",
                Controls = new System.Collections.Generic.List<object>(),
            };

            var panel5 = new JPanel()
            {
                Name = "panel5",
                Color = new ISO.Core.UI.JSONModels.Base.JColor()
                {
                    R = 255,
                    G = 255,
                    B = 255,
                    A = 255
                },
                Type = ISO.Core.UI.JSONModels.Base.JType.PANEL,
                Position = new ISO.Core.UI.JSONModels.Base.JPosition()
                {
                    X = 10,
                    Y = 10,
                },
                Scale = new ISO.Core.UI.JSONModels.Base.JScale()
                {
                    Width = 80,
                    Height = 80,
                },
                ZIndex = 1,
                ResourcePath = "UI/Slice9",
                Controls = new System.Collections.Generic.List<object>(),
            };

            panel.Controls.Add(panel2);
            panel.Controls.Add(panel3);
            panel4.Controls.Add(panel5);

            json.Controls.Add(panel);
            json.Controls.Add(panel4);

            var jsonString = JsonConvert.SerializeObject(json);

            return jsonString;
        }
    }

}
