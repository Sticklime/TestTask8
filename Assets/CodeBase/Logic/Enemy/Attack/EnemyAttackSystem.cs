using CodeBase.Logic.Common.Cooldown;
using CodeBase.Logic.Common.Health;
using CodeBase.Logic.Player;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

namespace CodeBase.Logic.Enemy.Attack
{
    [DisableAutoCreation]
    public partial class EnemyAttackSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            if (!TryCollectPlayers(out var players))
                return;

            var ecb = new EntityCommandBuffer(Allocator.Temp);

            foreach (var (enemyState, attack, enemyEntity)
                     in SystemAPI.Query<RefRO<EnemyStateComponent>, RefRO<AttackComponent>>()
                         .WithAll<CooldownComponent, CooldownReadyOneFrame>()
                         .WithEntityAccess())
            {
                if (enemyState.ValueRO.CurrentState != global::EnemyState.Attack)
                    continue;

                ApplyDamage(players, attack.ValueRO.DamageValue, ecb);
                ecb.RemoveComponent<CooldownReadyOneFrame>(enemyEntity);
            }

            ecb.Playback(EntityManager);
            ecb.Dispose();
            players.Dispose();
        }

        private bool TryCollectPlayers(out NativeList<Entity> players)
        {
            players = new NativeList<Entity>(Allocator.Temp);

            foreach (var (_, entity) in SystemAPI.Query<RefRO<HealthComponent>>()
                         .WithAll<PlayerTag, LocalTransform>()
                         .WithEntityAccess())
            {
                players.Add(entity);
            }

            if (players.Length == 0)
            {
                players.Dispose();
                return false;
            }

            return true;
        }

        private void ApplyDamage(NativeList<Entity> players, int damage, EntityCommandBuffer ecb)
        {
            foreach (var playerEntity in players)
            {
                if (!EntityManager.HasComponent<ApplyDamageOneFrame>(playerEntity))
                {
                    ecb.AddComponent(playerEntity, new ApplyDamageOneFrame { DamageAmount = damage });
                }
                else
                {
                    var existing = EntityManager.GetComponentData<ApplyDamageOneFrame>(playerEntity);
                    existing.DamageAmount += damage;
                    ecb.SetComponent(playerEntity, existing);
                }
            }
        }
    }
}
