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

            using (var game = new ISOGame(config))
            {
                var initScene = new ISO_Loader01("MainMenu", game, false);
                game.SceneManager.AddNew(initScene);

                game.Run();
            }
        }
    }
}
