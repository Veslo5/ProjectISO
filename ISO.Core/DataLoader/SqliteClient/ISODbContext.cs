using ISO.Core.DataLoader.SqliteClient.Contracts;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using ISOUI = ISO.Core.DataLoader.SqliteClient.Contracts.ISOUI;

namespace ISO.Core.DataLoader.SqliteClient
{
    public class ISODbContext : SQLiteConnection
    {

        public ISODbContext(string path) : base(path, false)
        {
        }

        public ISODbContext(string path, SQLiteOpenFlags openFlags) : base(path, openFlags, false)
        {
        }

        public void MigrateTables()
        {
            this.CreateTable<LOCATION>();
            this.CreateTable<MAP_DATA>();
            this.CreateTable<MAP>();
            this.CreateTable<ISOUI>();
            this.CreateTable<SCRIPT>();
        }

        public List<MAP> LoadAllMapsForLocation(int mapID)
        {
            var query = this.Table<MAP>().Where(x => x.LOCATION == mapID);

            return query.ToList();
        }
       
        public List<MAP_DATA> LoadDataForMap(int mapID, MapDataTypes type)
        {
            var typeString = Enum.GetName(typeof(MapDataTypes), type);
            return this.Table<MAP_DATA>().Where(x => x.ID == mapID && x.TYPE == typeString).ToList();
        }

        public MAP_DATA LoadMapDataByType(int mapID, MapDataTypes type)
        {
            var typeString = Enum.GetName(typeof(MapDataTypes), type);
            return this.Table<MAP_DATA>().FirstOrDefault(x => x.MAP == mapID && x.TYPE == typeString);
        }

        public T LoadTForMap<T>(int mapID) where T : IMapRelated, new()
        {
            return this.Table<T>().FirstOrDefault(x => x.MAP == mapID);
        }


    }

   
}
