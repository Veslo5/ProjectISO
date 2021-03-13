using ISO.Core.UI.JSONModels.Base;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ISO.Core.UI.JSONModels
{
    public class JPanel : JControlBase
    {
        public string Path { get; set; }
        public bool IsEnabled { get; set; }
        
        [JsonProperty(Order = 999)]
        public List<object> Controls { get; set; }
    }
}
