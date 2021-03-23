using ISO.Core.Engine.Logging;
using ISO.Core.Graphics.Sprites.Primitives;
using ISO.Core.Loading;
using ISO.Core.Scenes;
using ISO.Core.Scenes.SceneTypes;
using ISO.Core.UI.Elements.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ProjectISO.Levels
{
    public class ISO_Loader01 : MapScene
    {
        public ISO_Loader01(string name, int id, ISOGame game, bool enableLuaScripting) : base(name, id, game, enableLuaScripting)
        {
        }

        RectagleSprite testRectangle;


        public override void Initialize()
        {
            base.Initialize();
            Game.Window.AllowUserResizing = true;
            LoadingManager.AfterLoadCallback = AfterLoadContent;
        }

        public override void LoadContent()
        {
            base.LoadContent();

            LoadingManager.StartLoadingAsync();
            testRectangle = new RectagleSprite(Color.Red, Game.GraphicsDevice, new Rectangle(0, 0, 100, 100));

        }

        public override void AfterLoadContent(LoadingManager manager)
        {
            base.AfterLoadContent(manager);
        }

        public override void Draw(GameTime gameTime)
        {
            Game.GraphicsDevice.Clear(Color.Black);

            if (!LoadingManager.IsLoading)
                base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            if (LoadingManager.IsLoading)
                return;

            var state = Keyboard.GetState();
            var mouse = Mouse.GetState();

            var world = Camera.ScreenToWorldSpace(mouse.Position.ToVector2());

            //Game.Window.Title = "Screen " + mouse.X + "x" + mouse.Y + "World " + world.X + "x" + world.Y ;


            if (mouse.LeftButton == ButtonState.Pressed)
            {
                var controls = UI.GetUIOnPosition(mouse.Position);

                if (controls == null)
                    Log.Write("none");
                else
                    Log.Write(controls.Name);

            }




            if (state.IsKeyDown(Keys.W))
            {
                Camera.Zooom += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else if (state.IsKeyDown(Keys.S))
            {
                Camera.Zooom -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else if (state.IsKeyDown(Keys.Q))
            {
                Camera.Rotation += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else if (state.IsKeyDown(Keys.E))
            {
                Camera.Rotation -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else if (state.IsKeyDown(Keys.Space))
            {
                //TODO: Testing
                Camera.Reset();
            }
            else if (state.IsKeyDown(Keys.Y))
            {
                Game.Graphics.ChangeResolution(1366, 768, false);
            }
            else if (state.IsKeyDown(Keys.Up))
            {
                Camera.Position -= Vector2.UnitY * ((float)gameTime.ElapsedGameTime.TotalSeconds * 1000);
            }
            else if (state.IsKeyDown(Keys.Down))
            {
                Camera.Position += Vector2.UnitY * ((float)gameTime.ElapsedGameTime.TotalSeconds * 1000);
            }
            else if (state.IsKeyDown(Keys.Left))
            {
                Camera.Position -= Vector2.UnitX * ((float)gameTime.ElapsedGameTime.TotalSeconds * 1000);
            }
            else if (state.IsKeyDown(Keys.Right))
            {
                Camera.Position += Vector2.UnitX * ((float)gameTime.ElapsedGameTime.TotalSeconds * 1000);
            }
            else if (state.IsKeyDown(Keys.Escape))
            {
                this.Game.Exit();
            }
            else if (state.IsKeyDown(Keys.PageUp))
            {
                this.Game.SceneManager.NextScene("MENU");
            }

            base.Update(gameTime);
        }

        public override void UnloadContent()
        {
            LoadingManager.MarkAllAsUnload();
            base.UnloadContent();
        }
    }
}
