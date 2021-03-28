using ISO.Core.Engine.Logging;
using System.Collections.Generic;
using System.Linq;

namespace ISO.Core.Scenes
{
    /// <summary>
    /// Manager for scenes
    /// </summary>
    public class SceneController
    {
        /// <summary>
        /// Current scene
        /// </summary>
        public IScene CurrentScene { get; set; }

        /// <summary>
        /// Scenes holder
        /// </summary>
        private List<IScene> Scenes = new List<IScene>();

        /// <summary>
        /// Game reference
        /// </summary>
        private ISOGame _game;


        public SceneController(ISOGame game)
        {
            _game = game;
        }

        /// <summary>
        /// Add new scene into list
        /// </summary>
        /// <param name="scene"></param>
        public void AddNew(IScene scene)
        {
            Scenes.Add(scene);
            if (CurrentScene == null)
            {
                CurrentScene = scene;
            }
        }

        /// <summary>
        /// Change current scene
        /// </summary>
        /// <param name="name"></param>
        public void NextScene(string name)
        {
            Log.Info("---------------------------SCENE CHANGE---------------------------", LogModule.LO);
            var upperName = name.ToUpper();

            CurrentScene.UnloadContent();
            CurrentScene = Scenes.FirstOrDefault(x => x.Name == upperName);
            CurrentScene.Initialize();
            CurrentScene.LoadContent();
        }

    }
}
