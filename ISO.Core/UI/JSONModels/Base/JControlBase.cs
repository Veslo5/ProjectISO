using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ISO.Core.UI.JSONModels.Base
{
    public class JControlBase
    {
        [JsonProperty(Order = 1)]
        public string Name { get; set; }

        [JsonProperty(Order = 2)]
        public JType Type { get; set; }

        [JsonProperty(Order = 3)]
        public PositionType PositionType { get; set; }

        [JsonProperty(Order = 4)]
        public JColor Color { get; set; }

        [JsonProperty(Order = 5)]
        public JPosition Position { get; set; }

        [JsonProperty(Order = 6)]
        public JScale Scale { get; set; }

        [JsonProperty(Order = 7)]
        public int ZIndex { get; set; }

        [JsonProperty(Order = 8)]
        public bool IsVisible { get; set; }

        [JsonProperty(Order = 9)]
        public int Rotation { get; set; }

    }

    public class JColor
    {
        [JsonProperty(Order = 0)]
        public int R { get; set; }

        [JsonProperty(Order = 1)]
        public int G { get; set; }

        [JsonProperty(Order = 2)]
        public int B { get; set; }

        [JsonProperty(Order = 3)]
        public int A { get; set; }
    }

    public class JPosition
    {
        [JsonProperty(Order = 0)]
        public int X { get; set; }

        [JsonProperty(Order = 1)]
        public int Y { get; set; }
    }

    public class JScale
    {
        [JsonProperty(Order = 0)]
        public int Width { get; set; }

        [JsonProperty(Order = 1)]
        public int Height { get; set; }

        [JsonProperty(Order = 2)]
        public double ScaleX { get; set; }

        [JsonProperty(Order = 3)]
        public double ScaleY { get; set; }
    }

    public enum PositionType
    {
        PIXEL = 0,
        PERCENT = 1
    }

    public enum JType
    {
        TEXT = 0,
        PANEL = 1,
        BUTTON = 2
    }
}
