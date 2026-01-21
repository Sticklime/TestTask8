using Unity.Entities;

namespace CodeBase.Logic.UI.Health
{
    public struct UpdateHealthView
    {
        public int CurrentHealth;
        public int MaxHealth;
        public Entity Entity;

        public UpdateHealthView(int currentHealth, int maxHealth, Entity entity)
        {
            CurrentHealth = currentHealth;
            MaxHealth = maxHealth;
            Entity = entity;
        }
    }
}