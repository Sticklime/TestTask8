using Unity.Entities;

namespace CodeBase.Logic.Common.Health
{
    public struct DamageComponent : IComponentData
    {
        public int DamageAmount;
    }
}