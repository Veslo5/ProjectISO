using SQLite;

namespace ISO.Core.Data.DataLoader.SqliteClient.Contracts
{
    public class LOCATION
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string NAME { get; set; }

    }
}
