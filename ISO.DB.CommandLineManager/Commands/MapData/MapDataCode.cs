using ISO.Core.Data.DataLoader.SqliteClient;
using ISO.Core.Data.DataLoader.SqliteClient.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ISO.DB.CLM.Commands.MapData
{
    public class MapDataCode
    {
        public static void MapData(MapDataOptions options)
        {
            if (!Directory.Exists(options.Directory))
            {
                Console.WriteLine("Invalid directory for map data files!");
                return;
            }

            if (!Directory.Exists(options.DbPath))
            {
                Console.WriteLine("Invalid directory for database!");
                return;
            }

            //1GROUD.METAMAP - file with map metadata
            //1GROUD.METAPIC - file with picture metadata
            //1GROUND.METAPIC - file with picture metadata
            //1GROUND.METAPIC - file with picture metadta
            var files = Directory.GetFiles(options.Directory, "*.METAMAP|*.METAPIC", SearchOption.TopDirectoryOnly);


            List<MAP_DATA> mapData = new List<MAP_DATA>();

            for (int i = 0; i < files.Length; i++)
            {
                var filePath = files[i];

                var fileName = Path.GetFileNameWithoutExtension(filePath);
                var extension = Path.GetExtension(filePath);
                var mapidFromFileName = Convert.ToInt32(new string(fileName.TakeWhile(char.IsDigit).ToArray()));
                var data = File.ReadAllText(filePath);

                if (extension.ToUpper() == ".METAMAP")
                {
                    extension = Enum.GetName(typeof(MapDataTypes), MapDataTypes.MAP);
                }
                else if (extension.ToUpper() == ".METAPIC")
                {
                    extension = Enum.GetName(typeof(MapDataTypes), MapDataTypes.PICTURE);
                }

                var mapDataToInsert = new MAP_DATA() { DATA = data, MAP = mapidFromFileName, TYPE = extension };
                mapData.Add(mapDataToInsert);

            }


            using (var context = new ISODbContext(options.DbPath))
            {
                context.InsertAll(mapData);
            }


            Console.WriteLine("Inserted " + mapData.Count() + " data rows");

        }
    }
}
