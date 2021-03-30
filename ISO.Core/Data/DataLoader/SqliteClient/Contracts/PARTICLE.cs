using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace ISO.Core.Data.DataLoader.SqliteClient.Contracts
{
    public class PARTICLE
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string NAME { get; set; }
        public string DATA { get; set; }
    }
}
