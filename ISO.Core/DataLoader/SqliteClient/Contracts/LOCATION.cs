using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace ISO.Core.DataLoader.SqliteClient.Contracts
{
    public class LOCATION
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string NAME { get; set; }

    }
}
