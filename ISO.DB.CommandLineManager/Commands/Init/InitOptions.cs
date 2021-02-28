using CommandLine;
using System;
using System.Collections.Generic;
using System.Text;

namespace ISO.DB.CLM.Commands.Init
{
    [Verb("init", HelpText = "Initialize sqlite database with tables.")]
    public class InitOptions
    {
        [Option('c', HelpText = "Create blank sqlite database.")]
        public bool Create { get; set; }

        [Option('f', HelpText = "Force recreating database.")]
        public bool Force { get; set; }

        [Value(0, HelpText = "Path to database, if not specified current directory will be used.")]
        public string Path { get; set; }
    }
}
