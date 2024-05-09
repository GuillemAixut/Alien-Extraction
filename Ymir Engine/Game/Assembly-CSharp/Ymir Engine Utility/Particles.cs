using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace YmirEngine
{
    public class Particles
    {
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        public static extern void PlayParticles(object go);

        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        public static extern void ParticleShoot(object go, object vec);

        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        public static extern void PlayParticlesTrigger(object go);

        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        public static extern void StopParticles(object go);

        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        public static extern void RestartParticles(object go);

        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        public static extern void ParticlesForward(object go, object vec, int emitter, float distance);
    }
}
