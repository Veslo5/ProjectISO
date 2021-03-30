using ISO.Core.Data.DataLoader.SqliteClient.Contracts;
using SQLite;
using System;
using System.Collections.Generic;
using ISOUI = ISO.Core.Data.DataLoader.SqliteClient.Contracts.ISOUI;

namespace ISO.Core.Data.DataLoader.SqliteClient
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
            CreateTable<LOCATION>();
            CreateTable<MAP_DATA>();
            CreateTable<MAP>();
            CreateTable<ISOUI>();
            CreateTable<SCRIPT>();
            CreateTable<PARTICLE>();
        }

        public List<MAP> LoadAllMapsForLocation(int mapID)
        {
            var query = Table<MAP>().Where(x => x.LOCATION == mapID);

            return query.ToList();
        }

        public List<MAP_DATA> LoadDataForMap(int mapID, MapDataTypes type)
        {
            var typeString = Enum.GetName(typeof(MapDataTypes), type);
            return Table<MAP_DATA>().Where(x => x.ID == mapID && x.TYPE == typeString).ToList();
        }

        public MAP_DATA LoadMapDataByType(int mapID, MapDataTypes type)
        {
            var typeString = Enum.GetName(typeof(MapDataTypes), type);
            return Table<MAP_DATA>().FirstOrDefault(x => x.MAP == mapID && x.TYPE == typeString);
        }

        public List<MAP_DATA> LoadMapDataByTypes(int mapID, MapDataTypes type)
        {
            var typeString = Enum.GetName(typeof(MapDataTypes), type);
            return Table<MAP_DATA>().Where(x => x.MAP == mapID && x.TYPE == typeString).ToList();
        }

        public T LoadTForMap<T>(int mapID) where T : IMapRelated, new()
        {
            return Table<T>().FirstOrDefault(x => x.MAP == mapID);
        }

        public PARTICLE LoadParticle(string name)
        {
            return Table<PARTICLE>().FirstOrDefault(x => x.NAME == name);
        }

    }


}
