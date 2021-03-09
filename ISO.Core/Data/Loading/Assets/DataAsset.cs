using ISO.Core.Data.DataLoader.SqliteClient;
using System;
using System.Linq.Expressions;

namespace ISO.Core.Loading.Assets
{
    public class DataAsset : AssetBase
    {
        public DataAsset(string path)
        {
            base.Path = path;
        }

    }

    public class DataAsset<T> : DataAsset where T : new()
    {
        public DataAsset(string path) : base(path)
        {
        }

        public T Result { get; set; }

        public void FirstOrDefault(ISODbContext context, Expression<Func<T, bool>> predicate)
        {
            Result = context.Table<T>().FirstOrDefault(predicate);
        }

    }
}
