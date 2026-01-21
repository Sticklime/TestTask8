using Unity.Entities;

namespace CodeBase.Logic.Loot
{
    public struct DropLootComponent : IComponentData
    {
        public float ChanceFallingOut;
    }
}