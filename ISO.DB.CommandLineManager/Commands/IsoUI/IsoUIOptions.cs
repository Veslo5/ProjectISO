using CommandLine;

namespace ISO.DB.CLM.Commands.IsoUI
{
    [Verb("ui", HelpText = "Adds UI for map")]
    public class ISOUIOptions
    {
        [Value(0, Required = true, HelpText = "Directory of map files")]
        public string Directory { get; set; }

        [Value(1, Required = true, HelpText = "Database path")]
        public string DbPath { get; set; }
    }
}
