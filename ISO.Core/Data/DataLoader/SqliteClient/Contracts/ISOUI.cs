using SQLite;

namespace ISO.Core.Data.DataLoader.SqliteClient.Contracts
{
    [Table("UI")]
    public class ISOUI : IMapRelated
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public int MAP { get; set; }
        public string NAME { get; set; }
        public string DATA { get; set; }

    }
}
