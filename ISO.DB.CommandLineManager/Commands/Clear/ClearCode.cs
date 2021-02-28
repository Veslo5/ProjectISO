using ISO.Core.DataLoader.SqliteClient;
using ISO.Core.DataLoader.SqliteClient.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ISO.DB.CLM.Commands.Clear
{
    public class ClearCode
    {
        public static void Clear(ClearOptions options)
        {
            if (options.Silent == false)
            {
                Console.WriteLine("WARNING! This operation can lead to data corruption. Continue? [Y/N]");
                var key = Console.ReadKey();
                if (key.Key != ConsoleKey.Y)
                {
                    return;
                }
            }

            if (options.Location == false &&
                options.Map == false &&
                options.Script == false &&
                options.Data == false &&
                options.UI == false
                )
            {
                Console.WriteLine("At least one operation has to be selected");
                return;
            }


            if (!string.IsNullOrEmpty(options.Path)) // check if path is supplied
            {
                if (!Directory.Exists(options.Path)) // check if path exists
                {
                    Console.WriteLine("Invalid path!");
                    return;
                }
            }
            else // if not use current directory
            {
                options.Path = Constants.GetCurrentDirectory();
            }


            using (var context = new ISODbContext(options.Path))
            {

                var isIDNull = options.ID.HasValue;

                if (options.Location == true)
                {
                    if (isIDNull)
                    {
                        context.DeleteAll<LOCATION>();
                    }
                    else
                    {
                        context.Delete(new LOCATION { ID = options.ID.Value });
                    }

                }

                if (options.Map == true)
                {
                    if (isIDNull)
                    {
                        context.DeleteAll<MAP>();
                    }
                    else
                    {
                        context.Delete(new MAP { ID = options.ID.Value });
                    }

                }

                if (options.Script == true)
                {
                    if (isIDNull)
                    {
                        context.DeleteAll<SCRIPT>();
                    }
                    else
                    {
                        context.Delete(new SCRIPT { ID = options.ID.Value });
                    }
                }

                if (options.Data == true)
                {
                    if (isIDNull)
                    {
                        context.DeleteAll<MAP_DATA>();
                    }
                    else
                    {
                        context.Delete(new MAP_DATA { ID = options.ID.Value });
                    }
                }

                if (options.UI == true)
                {
                    if (isIDNull)
                    {
                        context.DeleteAll<ISOUI>();
                    }
                    else
                    {
                        context.Delete(new ISOUI { ID = options.ID.Value });
                    }
                }

            }

        }
    }
}
