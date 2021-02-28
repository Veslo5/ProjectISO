using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace ISO.Core.DataLoader.SqliteClient.Contracts
{
    public class MAP_DATA : IMapRelated
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public int MAP { get; set; }
        public string TYPE { get; set; }
        public string DATA { get; set; }

    }

    public enum MapDataTypes
    {
        MAP = 0,
        PICTURE = 1
    }

}
