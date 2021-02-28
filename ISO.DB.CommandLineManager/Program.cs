using CommandLine;
using ISO.Core.DataLoader.SqliteClient;
using ISO.DB.CLM.Commands.Clear;
using ISO.DB.CLM.Commands.Init;
using ISO.DB.CLM.Commands.IsoUI;
using ISO.DB.CLM.Commands.Locations;
using ISO.DB.CLM.Commands.Map;
using ISO.DB.CLM.Commands.MapData;
using ISO.DB.CLM.Commands.Script;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;

namespace ISO.DB.CLM
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<InitOptions, MapOptions, LocationOptions, MapDataOptions, ScriptOptions, ISOUIOptions, ClearOptions>(args)
                                        .WithParsed<InitOptions>(initopt => InitCode.InitDb(initopt))
                                        .WithParsed<MapOptions>(mapopt => MapCode.Map(mapopt))
                                        .WithParsed<LocationOptions>(locopt => LocationCode.Location(locopt))
                                        .WithParsed<MapDataOptions>(mapdataopt => MapDataCode.MapData(mapdataopt))
                                        .WithParsed<ScriptOptions>(scriptopt => ScriptCode.Script(scriptopt))
                                        .WithParsed<ISOUIOptions>(uiopt => IsoUICode.UI(uiopt))
                                        .WithParsed<ClearOptions>(clearopt => ClearCode.Clear(clearopt));


        }


    }
}
