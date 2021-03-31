namespace ISO.Core.Settings
{
    public class Config
    {
        /// <summary>
        /// Resolution width
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Resolution height
        /// </summary>
        public int Height { get; set; }
        
        /// <summary>
        /// Fullscreen
        /// </summary>
        public bool Fullscreen { get; set; }
        
        /// <summary>
        /// Default path to Data
        /// </summary>
        public string DataPath { get; set; }

        /// <summary>
        /// Vsync
        /// </summary>
        public bool Vsync { get; set; }

        /// <summary>
        /// Initialize default data
        /// </summary>
        public bool DataInit { get; set; }

        /// <summary>
        /// Max FPS cap
        /// </summary>
        public int FrameCap { get; set; }

        /// <summary>
        /// Use virtual resolution
        /// </summary>
        public bool VirtualResolution { get; set; }

        /// <summary>
        /// Lower fps when window not fully focused
        /// </summary>
        public bool SleepInBackground { get; set; }

    }
}
