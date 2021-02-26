using System;
using System.Collections.Generic;
using System.Text;

namespace ISO.Core.Settings
{
    public class Config
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public bool Fullscreen { get; set; }
        public string DataPath { get; set; }        
        public bool Vsync { get; set; }
        public bool DataInit { get; set; }



    }
}
