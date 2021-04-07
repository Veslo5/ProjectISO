using ISO.Core.Graphics.Sprites.Animation;
using ISO.Core.Loading;
using ISO.Core.Loading.Assets;
using ISO.Core.Scenes;
using ISO.Core.Scenes.SceneTypes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectISO.Levels
{
    public class ISO_Loader02 : BlankScene
    {
        private AnimatedSprite sprite;

        public ISO_Loader02(string name, int id, ISOGame game, bool luaScripting) : base(name, id, game, luaScripting)
        {

        }
        public override void Initialize()
        {
            base.Initialize();

            Game.Window.AllowUserResizing = true;
            LoadingManager.AfterLoadCallback = AfterLoadContent;
        }
        public override void LoadContent()
        {
            LoadingManager.Load<TextureAsset>("Animation", System.IO.Path.Combine("MAP", "OBJECT", "Pylon"));


            base.LoadContent();

            LoadingManager.StartLoadingAsync();

        }

        public override void AfterLoadContent(LoadingController manager)
        {
            sprite = new AnimatedSprite(manager.GetTexture("Animation").Texture, 5, 4, 100f);
            sprite.DestinationRectangle = new Rectangle(0, 0, 64, 96);

            base.AfterLoadContent(manager);
        }

        public override void Draw(GameTime gameTime)
        {
            Game.GraphicsDevice.Clear(Color.Black);

            if (!LoadingManager.IsLoading)
            {
                base.Draw(gameTime);

                SpriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: Camera.Projection);
                sprite.Draw(gameTime, SpriteBatch);
                SpriteBatch.End();
            }
        }

        public override void UnloadContent()
        {
            LoadingManager.MarkAllAsUnload();
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (LoadingManager.IsLoading)
                return;            

            sprite.Update(gameTime);

            if (Game.Input.IsVirtualButtonPressed("NextScene"))
            {
                this.Game.SceneManager.NextScene("GROUND");
            }

            base.Update(gameTime);
        }
    }
}
