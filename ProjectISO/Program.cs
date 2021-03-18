using ISO.Core.Data.DataLoader.SqliteClient;
using ISO.Core.Engine.Logging;
using ISO.Core.Scenes;
using ISO.Core.Settings;
using ISO.Core.UI.JSONModels;
using Newtonsoft.Json;
using ProjectISO.Levels;
using System;

namespace ProjectISO
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            //GenerateTestJSON();

            Log.Write("Starting engine...");
            var config = ConfigLoader.LoadConfig();

            initData(config.DataInit, config.DataPath);
          

            using (var game = new ISOGame(config))
            {
                var initScene = new ISO_Loader01("GROUND", 1, game, false);
                game.SceneManager.AddNew(initScene);

                game.Run();
            }

           Environment.ExitCode = 0; // success for linux or mac - windows do not require it but support it

        }

        private static void initData(bool init, string path)
        {
            if (init == true)
            {
                Log.Write("Migrating tables.");

                using (var context = new ISODbContext(path))
                {
                    context.MigrateTables();
                }

                Log.Write("Done.");

                return;
            }
        }

        private static void GenerateTestJSON()
        {
            var json = new UIRoot() { Controls = new System.Collections.Generic.List<object>() };
            var panel = new JPanel()
            {
                Name = "Panel",
                R = 255,
                G = 255,
                B = 255,
                Type = ISO.Core.UI.JSONModels.Base.JType.PANEL,
                X = 100,
                Y = 100,
                Width = 100,
                Height = 100,
                Controls = new System.Collections.Generic.List<object>(),
                ZIndex = 1

            };

            var panel2 = new JPanel()
            {
                Name = "Panel2",
                R = 0,
                G = 255,
                B = 0,
                Type = ISO.Core.UI.JSONModels.Base.JType.PANEL,
                X = 10,
                Y = 10,
                Width = 80,
                Height = 80,
                ZIndex = 0
            };

            var panel3 = new JPanel()
            {
                Name = "Panel3",
                R = 0,
                G = 0,
                B = 255,
                Type = ISO.Core.UI.JSONModels.Base.JType.PANEL,
                X = 10,
                Y = 10,
                Width = 60,
                Height = 60,
                ZIndex = 1

            };



            var panel4 = new JPanel()
            {
                Name = "panel4",
                R = 0,
                G = 100,
                B = 0,
                Type = ISO.Core.UI.JSONModels.Base.JType.PANEL,
                X = 150,
                Y = 150,
                Width = 100,
                Height = 100,
                ZIndex = 0,
                Controls = new System.Collections.Generic.List<object>(),
            };

            var panel5 = new JPanel()
            {
                Name = "panel5",
                R = 0,
                G = 0,
                B = 100,
                Type = ISO.Core.UI.JSONModels.Base.JType.PANEL,
                X = 10,
                Y = 10,
                Width = 80,
                Height = 80,
                ZIndex = 1,
                Controls = new System.Collections.Generic.List<object>(),
            };

            panel.Controls.Add(panel2);
            panel.Controls.Add(panel3);
            panel4.Controls.Add(panel5);

            json.Controls.Add(panel);
            json.Controls.Add(panel4);

            var y = JsonConvert.SerializeObject(json);
        }
    }
}
