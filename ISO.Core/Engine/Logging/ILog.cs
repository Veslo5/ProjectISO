using System;
using System.Collections.Generic;
using System.Text;

namespace ISO.Core.Engine.Logging
{
    public interface ILog
    {
        void Write(string text);
        void Info(string text);
        void Warning(string text);
        void Error(string text);
        void Unique(string text);
    }
}
