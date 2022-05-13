﻿/**
 * Starting code taken from tilemap exercise created by Nathan Bean
 */

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace TilemapGame.ParticleSystem
{
    public interface IParticleEmitter
    {
        public Vector2 Position { get; }
        public Vector2 Velocity { get; }
    }
    
}
