using ISO.Core.Graphics.Sprites.Atlas;
using Newtonsoft.Json;

namespace ISO.Core.Tiled.Models
{
    public class ISOTiledTileSet
    {
        public int columns { get; set; }
        public string image { get; set; }
        public int imageheight { get; set; }
        public int imagewidth { get; set; }
        public int margin { get; set; }
        public string name { get; set; }
        public int spacing { get; set; }
        public int tilecount { get; set; }
        public string tiledversion { get; set; }
        public int tileheight { get; set; }
        public int tilewidth { get; set; }
        public string type { get; set; }
        public double version { get; set; }

        //Ignored
        [JsonIgnore]
        public Atlas ImageAtlas { get;set;}
    }


}
