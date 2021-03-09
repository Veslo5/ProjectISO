using SQLite;

namespace ISO.Core.Data.DataLoader.SqliteClient.Contracts
{
    public class MAP
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public int LOCATION { get; set; }
        public string NAME { get; set; }


    }
}
