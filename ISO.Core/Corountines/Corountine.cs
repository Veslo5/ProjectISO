using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ISO.Core.Corountines
{
    public class Coroutine
    {
        private IEnumerator routine;
        private double? wait;

        public Coroutine(IEnumerator routine)
        {
            this.routine = routine;
        }

        public bool IsFinished { get; set; }

        public void Update(GameTime gameTime)
        {
            if (IsFinished) return;

            if (wait.HasValue)
            {
                var timeRemaining = wait.Value - gameTime.ElapsedGameTime.TotalMilliseconds;                

                if (timeRemaining < 0)
                {
                    wait = null;
                }
                else
                {
                    wait = timeRemaining;
                }

                // If wait has a value we still have time to burn before the
                // the next increment, so we return here.
                if (wait.HasValue) return;
            }

            if (!routine.MoveNext())
            {
                IsFinished = true;
            }
            else
            {
                wait = routine.Current as double?;
            }
        }
    }
}
