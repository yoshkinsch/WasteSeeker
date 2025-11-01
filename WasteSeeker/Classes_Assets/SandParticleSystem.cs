using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WasteSeeker.Classes_Assets
{
    public class SandParticleSystem : ParticleSystem
    {
        Rectangle _source;

        public bool IsRaining { get; set; } = true;

        public SandParticleSystem(Game game, Rectangle source) : base(game, 4000)
        {
            _source = source;
        }

        protected override void InitializeConstants()
        {
            textureFilename = "sand_pixel";
            minNumParticles = 1;
            maxNumParticles = 20;
        }

        protected override void InitializeParticle(ref Particle p, Vector2 where)
        {
            p.Initialize(where, Vector2.UnitX * -500, Vector2.Zero, Color.Beige, scale: RandomHelper.NextFloat(0.1f, 0.5f), lifetime: 5, rotation: 20, angularVelocity: 180);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (IsRaining) { AddParticles(_source); }
        }
    }
}
