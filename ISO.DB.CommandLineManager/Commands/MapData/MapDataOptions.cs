using CommandLine;

namespace ISO.DB.CLM.Commands.MapData
{

    [Verb("mapdata", HelpText = "Adds data for map")]
    public class MapDataOptions
    {
        [Value(0, Required = true, HelpText = "Directory of map files")]
        public string Directory { get; set; }

        [Value(1, Required = true, HelpText = "Database path")]
        public string DbPath { get; set; }

    }
}
