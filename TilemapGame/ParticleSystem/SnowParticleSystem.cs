/**
 * Starting code taken from particle system exercise created by Nathan Bean
 */

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace TilemapGame.ParticleSystem
{
    public class SnowParticleSystem : ParticleSystem 
    {
        Rectangle _source;
        Game _game;

        public bool IsSnowing { get; set; } = true;

        public SnowParticleSystem(Game game, Rectangle source) : base(game, 8000)
        {
            _source = source;
        }

        protected override void InitializeConstants()
        {
            textureFilename = "snowball";
            minNumParticles = 10;
            maxNumParticles = 20;
        }

        protected override void InitializeParticle(ref Particle p, Vector2 where)
        {
            p.Initialize(where, Vector2.UnitY * 100, Vector2.Zero, Color.White, scale: RandomHelper.NextFloat(0.01f, 0.2f), lifetime: 10, angularAcceleration: RandomHelper.NextFloat(-0.1f, 0.1f));
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (IsSnowing) AddParticles(_source);
            else _game.Components.Remove(this);
        }
    }
}
