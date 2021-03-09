using CommandLine;

namespace ISO.DB.CLM.Commands.Clear
{
    [Verb("clear", HelpText = "Clear game metadata by parameters")]
    public class ClearOptions
    {

        [Option('d', HelpText = "Remove map_data metadadata")]
        public bool Data { get; set; }

        [Option('s', HelpText = "Remove scripts metadata")]
        public bool Script { get; set; }

        [Option('u', HelpText = "Remmove ui metadata")]
        public bool UI { get; set; }

        [Option('m', HelpText = "Remove map metadata")]
        public bool Map { get; set; }

        [Option('l', HelpText = "Remove location metadata")]
        public bool Location { get; set; }

        [Option('s', HelpText = "silent mode")]
        public bool Silent { get; set; }


        [Value(0, HelpText = "Key ID of any object, if not specified, whole table will be cleared")]
        public int? ID { get; set; }

        [Value(1, HelpText = "Path to database, if not specified, current directory will be used")]
        public string Path { get; set; }
    }
}
