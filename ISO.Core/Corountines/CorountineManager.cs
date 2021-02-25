using ISO.Core.Logging;
using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ISO.Core.Corountines
{

    /// <summary>
    /// Do something very xxxx seconds (in ms)
    /// </summary>
    public class CorountineManager
    {
        private List<Coroutine> coroutines = new List<Coroutine>();

        public CorountineManager()
        {
            Log.Info("Creating corountine manager");
        }
        
        public Coroutine StartCoroutine(IEnumerator routine)
        {            
            var cr = new Coroutine(routine);
            coroutines.Add(cr);
            return cr;
        }

        private void UpdateCoroutines(GameTime gameTime)
        {
            var coroutinesToUpdate = coroutines.ToArray(); // Copying list in case of change

            foreach (var coroutine in coroutinesToUpdate)
                coroutine.Update(gameTime);

            coroutines.RemoveAll(c => c.IsFinished);
        }

        public void Update(GameTime gameTime)
        {
            UpdateCoroutines(gameTime);

        }
    }
}
