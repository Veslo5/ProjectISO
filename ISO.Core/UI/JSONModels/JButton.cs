using ISO.Core.UI.JSONModels.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace ISO.Core.UI.JSONModels
{
    public class JButton : JControlBase
    {
        public string Text { get; set; }

        public string ResourcePath { get; set; }
    }
}
