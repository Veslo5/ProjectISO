using ISO.Core.Loading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace ISO.Core.Engine
{
    public class Manager
    {
        internal virtual void LoadContent(LoadingController loadingManager) { }

        internal virtual void AfterLoad(LoadingController loadingManager) { }

        internal virtual void Update(GameTime gameTime) { }

        internal virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch) { }

        internal virtual void OnResolutionChanged(Viewport viewport) { }


    }
}
