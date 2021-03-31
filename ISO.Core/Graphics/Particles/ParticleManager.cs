using ISO.Core.Corountines;
using ISO.Core.Data.DataLoader.SqliteClient;
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
        private Dictionary<string, EmitterHolder> emitterContainer { get; set; }
        private string DbPath { get; }

        // Actually we do not need coroutine manager here... Emitting should be hanled outside manager
        //public CorountineManager CorountineManager { get; }

        public ParticleManager(string dbPath)
        {
            emitterContainer = new Dictionary<string, EmitterHolder>();
            DbPath = dbPath;
        }

        public void PreloadParticles(params string[] names)
        {
            foreach (var particleName in names)
            {
                emitterContainer.Add(particleName.ToUpper(), new EmitterHolder());
            }
        }

        public EmitterHolder GetParticleHolder(string name)
        {
            return emitterContainer[name.ToUpper()];
        }

        #region Overrides
        internal override void Update(GameTime gameTime)
        {
            foreach (var particleKey in emitterContainer)
            {
                particleKey.Value.Update(gameTime);
            }
        }

        internal override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (var particleKey in emitterContainer)
            {
                particleKey.Value.Draw(gameTime, spriteBatch);
            }
        }

        internal override void LoadContent(LoadingController manager)
        {
            manager.LoadCallback("PARTICLES", LoadParticles);
        }

        private void LoadParticles(LoadingController manager)
        {
            using (var context = new ISODbContext(DbPath))
            {
                foreach (var particleKey in emitterContainer)
                {
                    var particle = context.LoadParticle(particleKey.Key);

                    if (particle != null)
                    {
                        var upperName = particleKey.Key.ToUpper();

                        emitterContainer[upperName].Config = new EmitterConfig(JToken.Parse(particle.DATA));
                        manager.Load<TextureAsset>(upperName + "_PARTICLE", System.IO.Path.Combine("PARTICLES", upperName));
                    }
                }
            }
        }

        internal override void AfterLoad(LoadingController manager)
        {
            foreach (var particleKey in emitterContainer)
            {
                var particleHolder = particleKey.Value;
                particleHolder.ParticleContainer = new Container();
                var texture = manager.GetTexture(particleKey.Key.ToUpper() + "_PARTICLE");
                particleHolder.Emitter = new Emitter(particleHolder.ParticleContainer, new TextureRegion2D[] { new TextureRegion2D(texture.Texture) }, particleHolder.Config);
            }
        }
        #endregion
    }
}
