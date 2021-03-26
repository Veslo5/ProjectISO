using ISO.Core.Graphics.Sprites.Atlas;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace ISO.Core.Tiled.Models
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class Property
    {
        public string name { get; set; }
        public string type { get; set; }
        public object value { get; set; }
    }

    public class Layer
    {
        public List<int> data { get; set; }
        public int height { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public int opacity { get; set; }
        public string type { get; set; }
        public bool visible { get; set; }
        public int width { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public List<Property> properties { get; set; }
    }

    public class Tileset
    {
        public int columns { get; set; }
        public string image { get; set; }
        public int imageheight { get; set; }
        public int imagewidth { get; set; }
        public int margin { get; set; }
        public string name { get; set; }
        public int spacing { get; set; }
        public int tilecount { get; set; }        
        public int tileheight { get; set; }
        public int tilewidth { get; set; }
        public int firstgid { get; set; }

        //Ignored
        [JsonIgnore]
        public Atlas ImageAtlas { get; set; }
    }

    public class ISOTiledMap
    {
        public int compressionlevel { get; set; }
        public int height { get; set; }
        public bool infinite { get; set; }
        public List<Layer> layers { get; set; }
        public int nextlayerid { get; set; }
        public int nextobjectid { get; set; }
        public string orientation { get; set; }
        public string renderorder { get; set; }
        public string tiledversion { get; set; }
        public int tileheight { get; set; }
        public List<Tileset> tilesets { get; set; }
        public int tilewidth { get; set; }
        public string type { get; set; }
        public double version { get; set; }
        public int width { get; set; }
    }


}
