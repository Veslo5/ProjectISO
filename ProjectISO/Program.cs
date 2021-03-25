using ISO.Core.Data.DataLoader.SqliteClient;
using ISO.Core.Engine.Logging;
using ISO.Core.Engine.Logging.LogTypes;
using ISO.Core.Input;
using ISO.Core.Input.VirtualButtons;
using ISO.Core.Scenes;
using ISO.Core.Settings;
using ISO.Core.UI.JSONModels;
using ISO.Core.UI.JSONModels.Base;
using Microsoft.Xna.Framework.Input;
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
                prepareVirtualBindings(game.Input);

                var initScene = new ISO_Loader01("GROUND", 1, game, false);
                game.SceneManager.AddNew(initScene);

                var dummyScene = new ISO_Loader02("MENU", 2, game, false);
                game.SceneManager.AddNew(dummyScene);

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
        
        private static void prepareVirtualBindings(InputManager manager)
        {
            manager.VirtualButtonConfig.AddBindingToAction("Left", new ButtonBinding(Keys.A));
            manager.VirtualButtonConfig.AddBindingToAction("Left", new ButtonBinding(Keys.Left));
            
            manager.VirtualButtonConfig.AddBindingToAction("Right", new ButtonBinding(Keys.D));
            manager.VirtualButtonConfig.AddBindingToAction("Right", new ButtonBinding(Keys.Right));
            
            manager.VirtualButtonConfig.AddBindingToAction("Down", new ButtonBinding(Keys.S));
            manager.VirtualButtonConfig.AddBindingToAction("Down", new ButtonBinding(Keys.Down));
            
            manager.VirtualButtonConfig.AddBindingToAction("Up", new ButtonBinding(Keys.W));
            manager.VirtualButtonConfig.AddBindingToAction("Up", new ButtonBinding(Keys.Up));

            manager.VirtualButtonConfig.AddBindingToAction("ZoomIn", new ButtonBinding(Keys.Add));
            manager.VirtualButtonConfig.AddBindingToAction("ZoomOut", new ButtonBinding(Keys.Subtract));

            manager.VirtualButtonConfig.AddBindingToAction("RotateRight", new ButtonBinding(Keys.E));
            manager.VirtualButtonConfig.AddBindingToAction("RotateLeft", new ButtonBinding(Keys.Q));
            
            manager.VirtualButtonConfig.AddBindingToAction("DefaultView", new ButtonBinding(Keys.Space));

            manager.VirtualButtonConfig.AddBindingToAction("ResolutionTest", new ButtonBinding(Keys.Y));

            manager.VirtualButtonConfig.AddBindingToAction("Exit", new ButtonBinding(Keys.Escape));

            manager.VirtualButtonConfig.AddBindingToAction("NextScene", new ButtonBinding(Keys.PageUp));



        }

    }
}
