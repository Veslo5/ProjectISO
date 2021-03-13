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
        public int R { get; set; }

        [JsonProperty(Order = 5)]
        public int G { get; set; }

        [JsonProperty(Order = 6)]
        public int B { get; set; }

        [JsonProperty(Order = 7)]
        public int X { get; set; }

        [JsonProperty(Order = 8)]
        public int Y { get; set; }

        [JsonProperty(Order = 9)]
        public int Width { get; set; }

        [JsonProperty(Order = 10)]
        public int Height { get; set; }

        [JsonProperty(Order = 11)]
        public int ZIndex { get; set; }

        public bool IsVisible { get; set; }

        public int Rotation { get; set; }

        public double ScaleX { get; set; }
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
        PANEL = 1
    }
}
