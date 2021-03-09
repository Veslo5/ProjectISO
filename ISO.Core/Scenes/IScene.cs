using Microsoft.Xna.Framework;

namespace ISO.Core.Scenes
{
    public interface IScene
    {
        public string Name { get; }
        void Initialize();
        void Update(GameTime gameTime);
        void Draw(GameTime gameTime);
        void UnloadContent();
        void LoadContent();
        void GraphicsDevice_DeviceReset();
        void Window_ClientSizeChanged();
    }
}
