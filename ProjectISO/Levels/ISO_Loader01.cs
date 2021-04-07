using ISO.Core.Engine.Logging;
using ISO.Core.Graphics.Particles;
using ISO.Core.Graphics.Sprites.Animation;
using ISO.Core.Graphics.Sprites.Primitives;
using ISO.Core.Loading;
using ISO.Core.Loading.Assets;
using ISO.Core.Scenes;
using ISO.Core.Scenes.SceneTypes;
using ISO.Core.UI.Elements.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections;

namespace ProjectISO.Levels
{
    public class ISO_Loader01 : MapScene
    {
        public ISO_Loader01(string name, int id, ISOGame game, bool enableLuaScripting) : base(name, id, game, enableLuaScripting)
        {
        }        

        AnimatedSprite sprite;
        EmitterHolder particle;

        public override void Initialize()
        {
            base.Initialize();
            Game.Window.AllowUserResizing = true;
            LoadingManager.AfterLoadCallback = AfterLoadContent;
        }



        public override void LoadContent()
        {
            base.LoadContent();

            Particles.PreloadParticles("particle");
            particle = Particles.GetParticleHolder("particle");            
            
            LoadingManager.Load<TextureAsset>("Animation", System.IO.Path.Combine("MAP", "OBJECT", "Pylon"));

            LoadingManager.StartLoadingAsync();            

        }

        public override void AfterLoadContent(LoadingController manager)
        {
            sprite = new AnimatedSprite(manager.GetTexture("Animation").Texture, 5, 4, 100f);
            sprite.DestinationRectangle = new Rectangle(50, 800, 64, 96);

            base.AfterLoadContent(manager);
                        
            particle.Emmit();

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

        public override void Update(GameTime gameTime)
        {
            if (LoadingManager.IsLoading)
                return;
           
            particle.SetEmitterPosition(Camera.ScreenToWorldSpace(Game.Input.MousePosition.ToVector2()).ToPoint());            


            sprite.Update(gameTime);

            if (Game.Input.IsLeftMouseButtonPressed())
            {
                var controls = UI.GetUIOnPosition(Game.Input.MousePosition);

                if (controls == null)
                    Log.Write("none");
                else
                    Log.Write(controls.Name);

            }

            if (Game.Input.IsVirtualButtonDown("ZoomIn"))
            {
                Camera.Zooom += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else if (Game.Input.IsVirtualButtonDown("ZoomOut"))
            {
                Camera.Zooom -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else if (Game.Input.IsVirtualButtonDown("RotateLeft"))
            {
                Camera.Rotation += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else if (Game.Input.IsVirtualButtonDown("RotateRight"))
            {
                Camera.Rotation -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else if (Game.Input.IsVirtualButtonPressed("DefaultView"))
            {
                //TODO: Testing
                Camera.Reset();
            }
            else if (Game.Input.IsVirtualButtonPressed("ResolutionTest"))
            {
                Game.Graphics.ChangeResolution(1366, 768, false);
            }
            else if (Game.Input.IsVirtualButtonDown("Up")) // Up
            {
                Camera.Position -= Vector2.UnitY * ((float)gameTime.ElapsedGameTime.TotalSeconds * 1000);
            }
            else if (Game.Input.IsVirtualButtonDown("Down")) // Down
            {
                Camera.Position += Vector2.UnitY * ((float)gameTime.ElapsedGameTime.TotalSeconds * 1000);
            }
            else if (Game.Input.IsVirtualButtonDown("Left")) // Left
            {
                Camera.Position -= Vector2.UnitX * ((float)gameTime.ElapsedGameTime.TotalSeconds * 1000);
            }
            else if (Game.Input.IsVirtualButtonDown("Right")) // Right
            {
                Camera.Position += Vector2.UnitX * ((float)gameTime.ElapsedGameTime.TotalSeconds * 1000);
            }
            else if (Game.Input.IsVirtualButtonPressed("Exit"))
            {
                this.Game.Exit();
            }
            else if (Game.Input.IsVirtualButtonPressed("NextScene"))
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
