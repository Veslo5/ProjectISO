using ISO.Core.DataLoader.SqliteClient;
using ISO.Core.Logging;
using ISO.Core.Settings;
using ISO.Core.StateManager;
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
    }
}
