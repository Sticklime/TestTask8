using Unity.Entities;
using Unity.Mathematics;

namespace CodeBase.Logic.Common.Projectile
{
    public struct ProjectileComponent : IComponentData
    {
        public float3 Direction;
    }
}