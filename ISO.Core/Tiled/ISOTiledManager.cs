using ISO.Core.Camera;
using ISO.Core.DataLoader;
using ISO.Core.DataLoader.SqliteClient;
using ISO.Core.Logging;
using ISO.Core.Sprites.Atlas;
using ISO.Core.Sprites.Tile;
using ISO.Core.Tiled.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ISO.Core.Tiled
{
    public class ISOTiledManager
    {

        private ISOTiledMap MapMetadata { get; set; }
        private ISOTiledPicture MapImage { get; set; }

        private List<TileSprite[][]> AtlasSprites { get; set; } = new List<TileSprite[][]>(); // [row][column]

        OrthographicCamera Camera { get; set; }

        private Atlas ImageAtlas { get; set; }
        private int ID { get; }
        private string Path { get; }

        Stopwatch stopWatch = new Stopwatch();

        public ISOTiledManager(int ID, string path)
        {
            this.ID = ID;
            Path = path;
        }

        public void LoadContent(GraphicsDevice device, OrthographicCamera camera, ISOContentManager content)
        {
            Camera = camera;

            string filemapJson = null;
            string filepicJson = null;

            using (var context = new ISODbContext(Path))
            {
                filemapJson = context.LoadMapDataByType(ID, MapDataTypes.MAP).DATA;
                MapMetadata = JsonConvert.DeserializeObject<ISOTiledMap>(filemapJson);

                filepicJson = context.LoadMapDataByType(ID, MapDataTypes.PICTURE).DATA;
                MapImage = JsonConvert.DeserializeObject<ISOTiledPicture>(filepicJson);
            }

            var image = content.Load<Texture2D>("MAP" + "/GROUND/" + System.IO.Path.GetFileNameWithoutExtension(MapImage.image));

            ImageAtlas = new Atlas(image, MapImage.columns, MapImage.imageheight / MapImage.tileheight);


            foreach (var layer in MapMetadata.layers)
            {
                var rows = new TileSprite[layer.height][]; // All maps will be 1:1 so...

                for (int r = 0; r < layer.height; r++)
                {
                    var columns = new TileSprite[layer.width];

                    for (int c = 0; c < layer.width; c++)
                    {
                        var id = layer.data[(r * layer.height) + c];

                        if (id != 0)
                        {
                            var asprite = ImageAtlas.AtlasSprites[id - 1];

                            var newasprite = new TileSprite(asprite.Texture, asprite.PositionX, asprite.PositionY, ImageAtlas.Width, ImageAtlas.Height, r, c);
                            var x = ((c * 64) / 2) - ((r * 64) / 2);
                            var y = ((r * 32) / 2) + ((c * 32) / 2);
                            newasprite.destinationRectangle = new Rectangle(x, y, 64, 32);

                            columns[c] = newasprite;

                            if (c == layer.width - 1) // last item
                            {
                                rows[r] = columns;
                            }
                        }
                    }
                }

                AtlasSprites.Add(rows);

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
                    var tile = GetTileOnPosition(point.X, point.Y);
                    if (tile != null)
                    {
                        tile.Draw(gameTime, batch);
                    }

                }
            }
        }


        public void GetRectangle(Point topLeft, int rows, int columns)
        {
            var selectedRow = GetTileOnPosition(topLeft.X, topLeft.Y);
            var selected = GetTileOnPosition(topLeft.X, topLeft.Y);
            //selected.Color = Color.Red;


            for (int r = 0; r < rows; r++)
            {
                selectedRow = GetTileOnPosition(selectedRow.Row + 1, selectedRow.Column + 1); // rows
                selected = selectedRow;

                for (int c = 0; c < columns; c++) // zig zag
                {

                    selected = GetTileOnPosition(selected.Row, selected.Column - 1);
                    selected.Color = Color.Red;

                    selected = GetTileOnPosition(selected.Row + 1, selected.Column);
                    selected.Color = Color.Red;

                }
            }

        }

        public void Draw(GameTime time, SpriteBatch batch)
        {
            //stopWatch.Start();

            //var layer = AtlasSprites[0];
            //for (int row = 0; row < layer.Length; row++)
            //{
            //    for (int column = 0; column < layer[row].Length; column++)
            //    {
            //        layer[row][column].Draw(time, batch);
            //    }
            //}

            var cameraPosOnWorldTopLeft = Camera.ScreenToWorldSpace(new Vector2(-128,-64));
            var tileOnTopLeft = GetTileOnWorld((int)cameraPosOnWorldTopLeft.X, (int)cameraPosOnWorldTopLeft.Y);

            var cameraPosOnWorldBottomRight = Camera.ScreenToWorldSpace(new Vector2(Camera.viewport.Width + 128, Camera.viewport.Height + 64));
            var tileOnPosBottomRight = GetTileOnWorld((int)cameraPosOnWorldBottomRight.X, (int)cameraPosOnWorldBottomRight.Y);


            //Log.Write(tileOnTopLeft.ToString() + " " + cameraPosOnWorldTopLeft.ToString());

            DrawTiles(tileOnTopLeft, tileOnPosBottomRight, time, batch);



            //Task.Run(() =>
            //{
            //    Log.Write(stopWatch.ElapsedMilliseconds.ToString());
            //    stopWatch.Reset();
            //});


        }


        public void Update()
        {
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

            var column = (int)Math.Floor((y / 32f) + (x / 64f));
            var row = (int)Math.Floor((-x / 64f) + (y / 32f));

            return new Point(row, column);

        }

    }
}

