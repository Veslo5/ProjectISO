using CommandLine;

namespace ISO.DB.CLM.Commands.Locations
{

    [Verb("location", HelpText = "Adds new location into database")]
    public class LocationOptions
    {
        [Value(0, Required = true, HelpText = "Name of Location")]
        public string Name { get; set; }

        [Value(1, HelpText = "Path to database, in not supplied exe path will be used.")]
        public string DbPath { get; set; }
    }
}
