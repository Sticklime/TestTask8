using Unity.Entities;

namespace CodeBase.Logic.Common.Cooldown
{
    public struct CooldownComponent : IComponentData
    {
        public float TimeLeft;
        public float Interval;
    }
}