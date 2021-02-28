using CommandLine;
using System;
using System.Collections.Generic;
using System.Text;

namespace ISO.DB.CLM.Commands.Script
{
    [Verb("script", HelpText = "Adds scripts for map")]
    public class ScriptOptions
    {
        [Value(0, Required = true, HelpText = "Directory of script files")]
        public string Directory { get; set; }

        [Value(1, Required = true, HelpText = "Database path")]
        public string DbPath { get; set; }
    }
}
