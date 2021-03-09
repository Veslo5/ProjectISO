using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ISO.Core.StateManager
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
