using ISO.Core.Data.DataLoader.SqliteClient;
using ISO.Core.Data.DataLoader.SqliteClient.Contracts;
using ISO.Core.Engine.Camera;
using ISO.Core.Engine.Logging;
using ISO.Core.Graphics.Sprites.Atlas;
using ISO.Core.Graphics.Sprites.Tile;
using ISO.Core.Loading;
using ISO.Core.Loading.Assets;
using ISO.Core.Tiled.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ISO.Core.Tiled
{
    public class ISOTiledManager
    {
        private LoadingManager content { get; }
        private ISOTiledMap MapMetadata { get; set; }

        private List<TileSprite[][]> AtlasSprites { get; set; } = new List<TileSprite[][]>(); // [row][column]

        OrthographicCamera Camera { get; set; }
        private int ID { get; }
        private string Path { get; }


        public ISOTiledManager(int ID, string path, OrthographicCamera camera, LoadingManager content)
        {
            this.ID = ID;
            Path = path;
            Camera = camera;
            this.content = content;

        }

        public void LoadContent()
        {
            content.LoadCallback("MAP_DATA", LoadMapData);
        }

        public void LoadMapData()
        {
            using (var context = new ISODbContext(Path))
            {
                var filemapJson = context.LoadMapDataByType(ID, MapDataTypes.MAP);
                MapMetadata = JsonConvert.DeserializeObject<ISOTiledMap>(filemapJson.DATA); // tileset metadata are now in map metadata (use options in tiled!)                                

                Log.Info("Creating map " +  MapMetadata.width + "x" + MapMetadata.height + " with " + MapMetadata.layers.Count + " layers" , LogModule.CR);
                Log.Info("Map references " + MapMetadata.tilesets.Count + " tilesets" , LogModule.CR);
                

                foreach (var tileSet in MapMetadata.tilesets)
                {
                    // Every loaded image for tilemap should have _TILESET suffix to make it recognizable.

                    // TODO: we need to create better system to connect content data with map metadata
                    // Or at least our layers should have another names than tilesets.
                    // We could create system to separate tilesets from Map metada - because it is bloated (tiled already support this)
                    // many maps can share same tilesets, but right now every map has own tilesets in metada - so we basically copying data in json

                    // And yeah in loading callback you can still add another resources to loading queue
                    // Just for case if you read some metadata and need to load something based on reference from it
                    content.Load<TextureAsset>(tileSet.name + "_TILESET", "MAP" + "/TILESET/" + tileSet.name);
                }
            }
        }

        public void AfterLoad(LoadingManager manager)
        {
            foreach (var tileset in MapMetadata.tilesets)
            {
                tileset.ImageAtlas = new Atlas(content.GetTexture(tileset.name + "_TILESET").Texture, tileset.columns, tileset.imageheight / tileset.tileheight);
            }

            CreateMap();                                    
        }


        private void CreateMap()
        {
            int layerCount = -1;

            foreach (var layer in MapMetadata.layers)
            {
                layerCount += 1;
                var rows = new TileSprite[layer.height][]; // All maps will be 1:1 so...

                for (int r = 0; r < layer.height; r++) // r = row
                {
                    var columns = new TileSprite[layer.width];

                    for (int c = 0; c < layer.width; c++) // c = column
                    {
                        var id = layer.data[(r * layer.height) + c]; // calculating from 1D array indexes to 2D array X,Y

                        if (id != 0) // adding tiles only if have some image (0 = blank tile)
                        {

                            AtlasSprite asprite = null;

                            //TODO: We should create method and refactor this ugly thing... its kinda bloated :)!

                            foreach (var tileSet in MapMetadata.tilesets) // we can loop trough all tileset and have all informations there.
                            {
                                if (id >= tileSet.firstgid && id < (tileSet.firstgid + tileSet.tilecount)) // Check if tiled ID from array index is in this tileset indexing 
                                {
                                    int tilesetAtlasId;

                                    if (tileSet.firstgid != 1) // if it is not starting one tileset
                                    {
                                        tilesetAtlasId = id - tileSet.firstgid;
                                    }
                                    else // We need to do that everytime when tilemap indexing is on 1 (starting one)
                                    {
                                        tilesetAtlasId = id - 1;
                                    }

                                    asprite = tileSet.ImageAtlas.AtlasSprites[tilesetAtlasId]; //Get tile from atlas by ID

                                    var ImageAtlas = tileSet.ImageAtlas;

                                    // Create tile sprite with reference to Atlas image (batching purposes)
                                    var newasprite = new TileSprite(asprite.Texture, asprite.PositionX, asprite.PositionY, ImageAtlas.TileWidth, ImageAtlas.TileHeight, r, c, layerCount);

                                    //X and Y diamond position rendering (rendering right-down)
                                    var x = ((c * MapMetadata.tilewidth) / 2) - ((r * MapMetadata.tilewidth) / 2);
                                    var y = ((r * MapMetadata.tileheight) / 2) + ((c * MapMetadata.tileheight) / 2);

                                    // Walls or any 3 time higher tile need to have this offset to be rendered properly
                                    // Basic tiles which have same height as map grid does not need offset
                                    if (ImageAtlas.TileHeight - MapMetadata.tileheight != 0)
                                    {
                                        y -= ImageAtlas.TileHeight - MapMetadata.tileheight;
                                    }

                                    newasprite.destinationRectangle = new Rectangle(x, y, ImageAtlas.TileWidth, ImageAtlas.TileHeight); // destination rectangle X,Y and Size

                                    columns[c] = newasprite;
                                }
                            }

                        }
                    }

                    rows[r] = columns; // we must assing column to its array position everytime at the end 
                }

                AtlasSprites.Add(rows); // aaand finally adding our nicely cooked layer

            }
        }

        // Increases top to bottom down the screen.
        int Row(Point tile)
        {
            return tile.X + tile.Y;
        }
        // Increases left to right across the screen.
        int Column(Point tile)
        {
            return tile.X - tile.Y;
        }

        Point TileSiteAt(int row, int column)
        {
            //Debug.Assert(((row ^ column) & 1) == 0, "Both row & column must have the same parity");

            // row + column = (x + y) + (x - y) = 2 * x
            // (row + column) / 2 = x
            int x = (row + column) >> 1;

            // row - column = (x + y) - (x - y) = 2 * y
            // (row - column) / 2 = y
            int y = (row - column) >> 1;

            return new Point(x, y);
        }

        public void DrawTiles(Point start, Point end, GameTime gameTime, SpriteBatch batch)
        {

            int firstRow = Math.Min(Row(start), Row(end));
            int lastRow = Math.Max(Row(start), Row(end));

            int firstColumn = Math.Min(Column(start), Column(end));
            int lastColumn = Math.Max(Column(start), Column(end));

            for (int row = firstRow; row <= lastRow; row++)
            {
                // If the row is even and our first column is odd, pick the next even column.
                // Or if the row is odd and our first column is even, pick the next odd column.
                int shift = ((row ^ firstColumn) & 1);
                for (int column = firstColumn + shift; column <= lastColumn; column += 2)
                {
                    var point = TileSiteAt(row, column);

                    for (int layer = 0; layer < AtlasSprites.Count; layer++) // for each layer :)
                    {                       
                        var tile = GetTileOnPosition(point.X, point.Y, layer);
                        if (tile != null)
                        {
                            tile.Draw(gameTime, batch);
                        }                       
                    }

                }
            }
        }

        public void Update()
        {
            //TODO: refactor this with Game.Input reference
            var state = Mouse.GetState();
            var ks = Keyboard.GetState();

            //Log.Write(Camera.Position)


            if (state.LeftButton == ButtonState.Pressed)
            {
                var position = Camera.ScreenToWorldSpace(state.Position.ToVector2());
                var vec = GetTileOnWorld((int)position.X, (int)position.Y);
                var tile = GetTileOnWorldPosition((int)position.X, (int)position.Y);
                Log.Write(vec.ToString() + " " + position.ToString());
                if (tile != null)
                {
                    tile.Color = Color.Red;
                }
            }



        }

        public void Draw(GameTime time, SpriteBatch batch)
        {

            //TODO: We need to remove this 128 and 64 constants
            var cameraPosOnWorldTopLeft = Camera.ScreenToWorldSpace(new Vector2(-128, -64));
            var tileOnTopLeft = GetTileOnWorld((int)cameraPosOnWorldTopLeft.X, (int)cameraPosOnWorldTopLeft.Y);

            var cameraPosOnWorldBottomRight = Camera.ScreenToWorldSpace(new Vector2(Camera.viewport.Width + 128, Camera.viewport.Height + 64));
            var tileOnPosBottomRight = GetTileOnWorld((int)cameraPosOnWorldBottomRight.X, (int)cameraPosOnWorldBottomRight.Y);

            //Log.Write(tileOnTopLeft.ToString() + " " + cameraPosOnWorldTopLeft.ToString());

            DrawTiles(tileOnTopLeft, tileOnPosBottomRight, time, batch);

        }

        public TileSprite GetTileOnPosition(int row, int column, int layer = 0)
        {
            var tiles = AtlasSprites[layer];
            TileSprite tilespriteToReturn = null;

            if (row > 0 && row < tiles.Length)
            {
                if (column > 0 && column < tiles[row].Length)
                {
                    tilespriteToReturn = tiles[row][column];
                }
            }

            return tilespriteToReturn;
        }

        public TileSprite GetTileOnWorldPosition(int x, int y)
        {

            var column = (int)Math.Floor((y / 32f) + (x / 64f));
            var row = (int)Math.Floor((-x / 64f) + (y / 32f));

            return GetTileOnPosition(row, column, 0);

        }

        public Point GetTileOnWorld(int x, int y)
        {

            var column = (int)Math.Floor((y / (float)MapMetadata.tileheight) + (x / (float)MapMetadata.tilewidth));
            var row = (int)Math.Floor((-x / (float)MapMetadata.tilewidth) + (y / (float)MapMetadata.tileheight));

            return new Point(row, column);
        }

    }
}

