using ISO.Core.DataLoader;
using ISO.Core.Sprites;
using ISO.Core.Sprites.Primitives;
using ISO.Core.StateManager;
using ISO.Core.UI.Elements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectISO.Levels
{
    public class ISO_Loader01 : Scene
    {
        public ISO_Loader01(string name, ISOGame game, bool enableLuaScripting) : base(name, game, enableLuaScripting)
        {
        }


        Sprite testSprite;
        ISORectagle testRectangle;


        public override void Initialize()
        {
            base.Initialize();
            UI.UILoader.LoadJson(Name);
        }

        public override void LoadContent()
        {
            base.LoadContent();


            testSprite = new Sprite(RawDataLoader.GetTextureFromFile(Game.GraphicsDevice, "test.jpg"));

            testRectangle = new ISORectagle(Color.Red, Game.GraphicsDevice, new Rectangle(0, 0, 100, 100));

        }

        public override void Draw(GameTime gameTime)
        {
            Game.GraphicsDevice.Clear(Color.Black);

            //SpriteBatch.Begin(transformMatrix: Camera.Projection);
            //testSprite.Draw(gameTime, SpriteBatch);
            //testRectangle.Draw(gameTime, SpriteBatch);
            //SpriteBatch.End();

            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            var state = Keyboard.GetState();
            var mouse = Mouse.GetState();

            var world = Camera.ScreenToWorldSpace(mouse.Position.ToVector2());

            //Game.Window.Title = "Screen " + mouse.X + "x" + mouse.Y + "World " + world.X + "x" + world.Y ;



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

            base.Update(gameTime);
        }
    }
}
