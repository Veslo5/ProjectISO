using ISO.Core.Loading;
using ISO.Core.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ISO.Core.UI
{
    public interface IUI
    {
        string Name { get; set; }

        void Draw(GameTime gameTime, SpriteBatch spriteBatch);
        void Update(GameTime gameTime);
        void LoadContent(LoadingController manager);
        void AfterLoad(LoadingController manager);
    }
}
