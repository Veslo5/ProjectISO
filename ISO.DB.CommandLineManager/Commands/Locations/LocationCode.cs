using ISO.Core.DataLoader.SqliteClient;
using ISO.Core.DataLoader.SqliteClient.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace ISO.DB.CLM.Commands.Locations
{
    public class LocationCode
    {
        public static void Location(LocationOptions options)
        {

            if (string.IsNullOrEmpty(options.DbPath))
            {
                options.DbPath = Constants.GetCurrentDirectory(true);
            }

            using (var context = new ISODbContext(options.DbPath))
            {
                var newLocation = new LOCATION() { NAME = options.Name };
                context.Insert(newLocation);

                Console.WriteLine("New location created with ID " + newLocation.ID);
            }
        }
    }
}
