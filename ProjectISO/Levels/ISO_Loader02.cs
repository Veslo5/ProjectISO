using ISO.Core.Loading;
using ISO.Core.Scenes;
using ISO.Core.Scenes.SceneTypes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectISO.Levels
{
    public class ISO_Loader02 : BlankScene
    {

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
            base.LoadContent();

            LoadingManager.StartLoadingAsync();

        }

        public override void AfterLoadContent(LoadingController manager)
        {
            base.AfterLoadContent(manager);
        }

        public override void Draw(GameTime gameTime)
        {
            Game.GraphicsDevice.Clear(Color.Red);

            if (!LoadingManager.IsLoading)
                base.Draw(gameTime);
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

            if (Game.Input.IsVirtualButtonPressed("NextScene"))
            {
                this.Game.SceneManager.NextScene("GROUND");
            }

            base.Update(gameTime);
        }
    }
}
