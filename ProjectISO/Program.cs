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
            Log.Write("Starting engine...");
            var config = ConfigLoader.LoadConfig();

            initData(config.DataInit, config.DataPath);
          

            using (var game = new ISOGame(config))
            {
                var initScene = new ISO_Loader01("GROUND", 1, game, false);
                game.SceneManager.AddNew(initScene);

                game.Run();
            }
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
                Controls = new System.Collections.Generic.List<object>()
            };

            var panel2 = new JPanel()
            {
                Name = "Panel2",
                R = 0,
                G = 255,
                B = 0,
                Type = ISO.Core.UI.JSONModels.Base.JType.PANEL,
                X = 100,
                Y = 100,
                Width = 100,
                Height = 100
            };

            panel.Controls.Add(panel2);

            json.Controls.Add(panel);

            var y = JsonConvert.SerializeObject(json);
        }
    }
}
