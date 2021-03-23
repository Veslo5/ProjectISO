using ISO.Core.Data.DataLoader.SqliteClient;
using ISO.Core.Engine.Logging;
using ISO.Core.Engine.Logging.LogTypes;
using ISO.Core.Scenes;
using ISO.Core.Settings;
using ISO.Core.UI.JSONModels;
using ISO.Core.UI.JSONModels.Base;
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
            Log.SetLogger(new ConsoleLogger());

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
        
    }
}
