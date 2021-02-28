using CommandLine;
using System;
using System.Collections.Generic;
using System.Text;

namespace ISO.DB.CLM.Commands.Map
{

    [Verb("map", HelpText = "Adds new map into database")]
    public class MapOptions
    {
        [Value(0, Required = true,  HelpText = "Location ID")]
        public int LocationID { get; set; }

        [Value(1, Required = true, HelpText = "Map name")]
        public string MapName { get; set; }

        [Value(2, HelpText = "Path to database, if not supplied exe path will be used.")]
        public string DbPath { get; set; }
    }
}
