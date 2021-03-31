using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using P.Particles;
using System;
using System.Collections.Generic;
using System.Text;

namespace ISO.Core.Graphics.Particles
{
    public class EmitterHolder
    {
        /// <summary>
        /// Container is collections of pictures emmited from emitter
        /// </summary>
        public Container ParticleContainer { get; set; }
        
        /// <summary>
        /// Emitter manages behaviour of particles
        /// </summary>
        public Emitter Emitter { get; set; }
        /// <summary>
        /// Config holds JSON with particle configuration
        /// </summary>
        public EmitterConfig Config { get; set; }        


        public void Emmit() => Emitter.Emit = true;

        public void SetEmitterPosition(Point position)
        {
            Emitter.UpdateOwnerPos(position.X, position.Y);
        }

        internal void Update(GameTime gameTime)
        {
            Emitter.Update((float)gameTime.ElapsedGameTime.TotalSeconds); // 0.001f is magical number used in official docs
        }

        internal void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var particles = ParticleContainer.Children;

            for (int i = 0; i < particles.Count; i++)
            {
                var particle = (Sprite)particles[i];

                if (spriteBatch.GraphicsDevice.BlendState != particle.Material.blendState)
                    spriteBatch.GraphicsDevice.BlendState = particle.Material.blendState;

                Vector2 origin = new Vector2(particle.TextureRegion.Size.X * particle.Anchor.X - particle.TextureRegion.Trim.X,
                    particle.TextureRegion.Size.Y * particle.Anchor.Y - particle.TextureRegion.Trim.Y);

                spriteBatch.Draw(
                    particle.TextureRegion.BaseTexture,
                    particle.Position,
                    particle.TextureRegion.Frame,
                    particle.Tint * (particle.Alpha * ParticleContainer.Alpha),
                    particle.Rotation,
                    origin,
                    particle.Scale,
                    SpriteEffects.None,
                    0
                );
            }
        }
    }
}
