using System.Collections.Generic;

namespace ISO.Core.UI.JSONModels
{
    public class Button
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string PosType { get; set; }
        public double ScaleX { get; set; }
        public double ScaleY { get; set; }
        public int Rotation { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public bool IsVisible { get; set; }
        public bool IsEnabled { get; set; }
    }

    public class ISOUIText
    {
        public string Name { get; set; }
        public int Rotation { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public double ScaleX { get; set; }
        public double ScaleY { get; set; }
        public bool IsVisible { get; set; }
        public int R { get; set; }
        public int G { get; set; }
        public int B { get; set; }
        public string Text { get; set; }
    }

    public class UIRoot
    {
        //public List<Button> Buttons { get; set; }
        public List<ISOUIText> Texts { get; set; }
    }
}
