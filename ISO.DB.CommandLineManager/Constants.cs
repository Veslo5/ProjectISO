using System;
using System.IO;

namespace ISO.DB.CLM
{
    public class Constants
    {
        public const string dbName = "galaxy_data.DAT";

        /// <summary>
        /// Returns path of the EXE
        /// </summary>
        /// <param name="withDbName"></param>
        /// <returns></returns>
        public static string GetCurrentDirectory(bool withDbName = false)
        {
            if (withDbName == true)
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, dbName);
            }
            else
            {
                return AppDomain.CurrentDomain.BaseDirectory;
            }
        }

    }
}
