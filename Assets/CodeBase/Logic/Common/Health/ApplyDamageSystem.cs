using CodeBase.Logic.Common.Death;
using CodeBase.Logic.UI.Health;
using MessagePipe;
using Unity.Collections;
using Unity.Entities;

namespace CodeBase.Logic.Common.Health
{
    [DisableAutoCreation]
    public partial class ApplyDamageSystem : SystemBase
    {
        private readonly IPublisher<UpdateHealthView> _healthPublisher;

        public ApplyDamageSystem(IPublisher<UpdateHealthView> publisher)
        {
            _healthPublisher = publisher;
        }

        protected override void OnUpdate()
        {
            var ecb = new EntityCommandBuffer(Allocator.Temp);

            foreach (var (health, damage, entity) in
                     SystemAPI.Query<RefRW<HealthComponent>, RefRO<ApplyDamageOneFrame>>()
                         .WithEntityAccess())
            {
                health.ValueRW.CurrentHealth -= damage.ValueRO.DamageAmount;

                if (health.ValueRW.CurrentHealth <= 0)
                {
                    health.ValueRW.CurrentHealth = 0;
                    ecb.AddComponent<DeathOneFrameComponent>(entity);
                }

                _healthPublisher.Publish(
                    new UpdateHealthView(
                        health.ValueRW.CurrentHealth,
                        health.ValueRW.MaxHealth,
                        entity
                    )
                );

                ecb.RemoveComponent<ApplyDamageOneFrame>(entity);
            }

            ecb.Playback(EntityManager);
            ecb.Dispose();
        }
    }
}