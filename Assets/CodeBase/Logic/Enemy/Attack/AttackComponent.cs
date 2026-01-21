using Unity.Entities;

namespace CodeBase.Logic.Enemy.Attack
{
    public struct AttackComponent : IComponentData
    {
        public int DamageValue;    
    }
}