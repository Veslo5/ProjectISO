using ISO.Core.DataLoader.SqliteClient;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

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
