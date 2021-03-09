using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISO.Core.StateManager
{
    /// <summary>
    /// Manager for scenes
    /// </summary>
    public class SceneManager
    {
        /// <summary>
        /// Current scene
        /// </summary>
        public IScene CurrentScene {get; set; }

        /// <summary>
        /// Scenes holder
        /// </summary>
        private List<IScene> Scenes = new List<IScene>();

        /// <summary>
        /// Game reference
        /// </summary>
        private ISOGame _game;


        public SceneManager(ISOGame game)
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
            var upperName = name.ToUpper();

            CurrentScene.UnloadContent();
            CurrentScene = Scenes.FirstOrDefault(x => x.Name == upperName);
            CurrentScene.LoadContent();
        }

    }
}
