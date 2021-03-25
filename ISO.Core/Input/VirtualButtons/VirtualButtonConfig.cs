using System;
using System.Collections.Generic;
using System.Text;

namespace ISO.Core.Input.VirtualButtons
{
    public class VirtualButtonConfig
    {
        internal Dictionary<string, List<ButtonBinding>> virtualButtonSetHolder { get; set; }

        public VirtualButtonConfig()
        {
            virtualButtonSetHolder = new Dictionary<string, List<ButtonBinding>>();            
        }        

        public void AddBindingToAction(string actionName, ButtonBinding binding)
        {
            if (!virtualButtonSetHolder.ContainsKey(actionName))
            {
                virtualButtonSetHolder.Add(actionName, new List<ButtonBinding>() { binding });
            }
            else
            {
                var listOfBindings = virtualButtonSetHolder[actionName];
                listOfBindings.Add(binding);
            }

        }

    }
}
