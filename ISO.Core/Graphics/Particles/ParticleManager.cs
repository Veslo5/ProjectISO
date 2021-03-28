using ISO.Core.Corountines;
using ISO.Core.Engine;
using ISO.Core.Engine.Logging;
using ISO.Core.Loading;
using ISO.Core.Loading.Assets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json.Linq;
using P.Particles;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ISO.Core.Graphics.Particles
{
    public class ParticleManager : Manager
    {
        public ParticleManager(GraphicsDevice graphicsDevice, CorountineManager corountineManager)
        {
            GraphicsDevice = graphicsDevice;
            CorountineManager = corountineManager;
        }


        public GraphicsDevice GraphicsDevice { get; }
        public CorountineManager CorountineManager { get; }


        Texture2D particleTexture1 { get; set; }
        Container particleContainer { get; set; }
        Emitter particleEmitter { get; set; }



        private IEnumerator Emit()
        {
            while (true)
            {
                particleEmitter.Emit = true;

                yield return 1000.0;
            }
        }



        #region Overrides
        internal override void Update(GameTime gameTime)
        {
            particleEmitter.Update(gameTime.ElapsedGameTime.Milliseconds * 0.001f);

        }


        internal override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var particles = particleContainer.Children;
            spriteBatch.Begin();


            if (particles.Count > 0)
            {
                Log.Write("part count: " + particles.Count);
            }

            for (int i = 0; i < particles.Count; i++)
            {
                var particle = (Sprite)particles[i];

                if (GraphicsDevice.BlendState != particle.Material.blendState)
                    GraphicsDevice.BlendState = particle.Material.blendState;

                Vector2 origin = new Vector2(particle.TextureRegion.Size.X * particle.Anchor.X - particle.TextureRegion.Trim.X,
                    particle.TextureRegion.Size.Y * particle.Anchor.Y - particle.TextureRegion.Trim.Y);

                spriteBatch.Draw(
                    particle.TextureRegion.BaseTexture,
                    particle.Position,
                    particle.TextureRegion.Frame,
                    particle.Tint * (particle.Alpha * particleContainer.Alpha),
                    particle.Rotation,
                    origin,
                    particle.Scale,
                    SpriteEffects.None,
                    0
                );
            }
            spriteBatch.End();
        }



        internal override void LoadContent(LoadingController manager)
        {
            manager.Load<TextureAsset>("Pixie_texture", System.IO.Path.Combine("PARTICLES", "particle"));
        }



        internal override void AfterLoad(LoadingController manager)
        {
            var texture = manager.GetTexture("Pixie_texture");

            //If is texture null, there is no reason to continue
            if (texture.Texture == null)
                return;

            particleTexture1 = texture.Texture;


            particleContainer = new Container();
            string json = File.ReadAllText(@"C:\Users\David.DESKTOP-PCSA74K\Desktop\Game development\ProjectISO\emitter.json");
            var config = new EmitterConfig(JToken.Parse(json));

            particleEmitter = new Emitter(particleContainer, new TextureRegion2D[] { new TextureRegion2D(particleTexture1) }, config);

            var x = 400;
            var y = 300;

            particleEmitter.UpdateOwnerPos(x, y);
            CorountineManager.StartCoroutine(Emit());
        }
        #endregion
    }
}
