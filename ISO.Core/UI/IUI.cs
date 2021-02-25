using ISO.Core.StateManager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace ISO.Core.UI
{
    public interface IUI
    {
        string Name { get; set; }

        void Draw(GameTime gameTime, SpriteBatch spriteBatch);
        void Update(GameTime gameTime);
        void LoadContent(ISOGame game);
    }
}
