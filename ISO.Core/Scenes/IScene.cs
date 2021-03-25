using ISO.Core.Loading;
using Microsoft.Xna.Framework;

namespace ISO.Core.Scenes
{
    public interface IScene
    {
        public string Name { get; }
        public int ID { get; }
        void Initialize();
        void BeforeUpdate(GameTime gameTime);
        void Update(GameTime gameTime);
        void AfterUpdate(GameTime gameTime);

        void Draw(GameTime gameTime);
        void UnloadContent();
        void LoadContent();
        void AfterLoadContent(LoadingManager manager);
        void GraphicsDevice_DeviceReset();
        void Window_ClientSizeChanged();
    }
}
