using Unity.Entities;

namespace CodeBase.Logic.Common.Health
{
    public struct HealthComponent : IComponentData
    {
        public int CurrentHealth;
        public int MaxHealth;
    }
}