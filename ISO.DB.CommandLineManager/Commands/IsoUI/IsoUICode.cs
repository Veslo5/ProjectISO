using ISO.Core.Data.DataLoader.SqliteClient;
using ISO.Core.Data.DataLoader.SqliteClient.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ISO.DB.CLM.Commands.IsoUI
{
    public class IsoUICode
    {
        public static void UI(ISOUIOptions options)
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

            //2GROUND.UI - file with map metadata

            var files = Directory.GetFiles(options.Directory, "*.UI", SearchOption.TopDirectoryOnly);


            List<ISOUI> scriptData = new List<ISOUI>();

            for (int i = 0; i < files.Length; i++)
            {
                var filePath = files[i];

                var fileName = Path.GetFileNameWithoutExtension(filePath); // GROUND

                var idLenght = fileName.TakeWhile(char.IsDigit).Count(); // 1
                var idName = fileName.Substring(idLenght, fileName.Length); // GROUND

                var mapidFromFileName = Convert.ToInt32(new string(fileName.TakeWhile(char.IsDigit).ToArray())); // 2

                var data = File.ReadAllText(filePath); // DATA

                var mapDataToInsert = new ISOUI() { DATA = data, MAP = mapidFromFileName, NAME = idName };

                scriptData.Add(mapDataToInsert);

            }

            using (var context = new ISODbContext(options.DbPath))
            {
                context.InsertAll(scriptData);
            }


            Console.WriteLine("Inserted " + scriptData.Count() + " UIs");
        }
    }
}

