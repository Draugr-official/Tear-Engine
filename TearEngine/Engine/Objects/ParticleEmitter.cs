using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TearEngine.Engine.Objects
{
    /// <summary>
    /// Tear engine particle emitter
    /// </summary>
    class ParticleEmitter
    {
        /// <summary>
        /// Get / set the parent of the particle emitter
        /// </summary>
        public Sprite2D Parent { get; set; }

        /// <summary>
        /// Get / set the particle type of the particle emitter
        /// </summary>
        public ParticleEmitterType ParticleType { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="Parent"></param>
        public ParticleEmitter(Sprite2D Parent, ParticleEmitterType ParticleType)
        {
            this.Parent = Parent;
            this.ParticleType = ParticleType;
        }


    }

    /// <summary>
    /// Defines a particle type for the particle emitter
    /// </summary>
    enum ParticleEmitterType
    {
        Cube
    }
}
