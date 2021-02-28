using ISO.Core.DataLoader.SqliteClient;
using ISO.Core.DataLoader.SqliteClient.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace ISO.DB.CLM.Commands.Map
{
    public class MapCode
    {

        public static void Map(MapOptions options)
        {
            if (string.IsNullOrEmpty(options.DbPath))
            {
                options.DbPath = Constants.GetCurrentDirectory(true);
            }

            using (var context = new ISODbContext(options.DbPath))
            {
                var newMap = new MAP() { LOCATION = options.LocationID, NAME = options.MapName };
                context.Insert(newMap);

                Console.WriteLine("New map created with ID " + newMap.ID);
            }
        }
    }
}
