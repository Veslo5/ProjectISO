using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace ISO.Core.Input.VirtualButtons
{
    public class ButtonBinding
    {        
        public Keys Key { get; set; }

        public ButtonBinding(Keys key)
        {            
            Key = key;
        }
    }
}
