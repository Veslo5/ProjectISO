using ISO.Core.Data.DataLoader.SqliteClient;
using SQLite;
using System;
using System.IO;

namespace ISO.DB.CLM.Commands.Init
{
    public class InitCode
    {
        public static void InitDb(InitOptions input)
        {
            if (!string.IsNullOrEmpty(input.Path)) // check if path is supplied
            {
                if (!Directory.Exists(input.Path)) // check if path exists
                {
                    Console.WriteLine("Invalid path!");
                    return;
                }
            }
            else // if not use current directory
            {
                input.Path = Constants.GetCurrentDirectory();
            }

            var pathWithName = Path.Combine(input.Path, Constants.dbName);

            if (input.Create == true) // -c command
            {
                if (File.Exists(pathWithName)) // -f command
                {
                    if (input.Force == true)
                    {
                        File.Delete(pathWithName);
                    }
                    else
                    {
                        Console.WriteLine("There is already database!");
                        return;

                    }
                }

                migrateTables(pathWithName);

            }

        }

        private static void migrateTables(string path)
        {
            using (var context = new ISODbContext(path,
                SQLiteOpenFlags.Create |
                SQLiteOpenFlags.FullMutex |
                SQLiteOpenFlags.ReadWrite))
            {
                context.MigrateTables();
                Console.WriteLine("Database initialized.");

            }
        }


    }
}
