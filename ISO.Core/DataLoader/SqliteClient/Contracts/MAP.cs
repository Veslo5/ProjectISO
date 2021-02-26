using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace ISO.Core.DataLoader.SqliteClient.Contracts
{
    public class MAP
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public int LOCATION { get; set; }
        public string NAME { get; set; }


    }
}
