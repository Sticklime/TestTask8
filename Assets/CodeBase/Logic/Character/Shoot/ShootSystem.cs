using CodeBase.Infrastructure.Factory;
using CodeBase.Logic.Common.Cooldown;
using CodeBase.Logic.Enemy;
using CodeBase.Logic.Player;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace CodeBase.Logic.Character.Shoot
{
    [DisableAutoCreation]
    public partial class ShootSystem : SystemBase
    {
        private readonly IGameFactory _gameFactory;

        public ShootSystem(IGameFactory gameFactory)
        {
            _gameFactory = gameFactory;
        }

        protected override void OnUpdate()
        {
            if (!TryCollectEnemies(out var enemies))
                return;

            var ecb = new EntityCommandBuffer(Allocator.Temp);

            foreach (var (playerTransform, playerEntity) in
                     SystemAPI.Query<RefRO<LocalTransform>>()
                         .WithAll<PlayerTag, CooldownReadyOneFrame>()
                         .WithEntityAccess())
            {
                if (!TryGetClosestEnemy(playerTransform.ValueRO.Position, enemies, out var direction))
                    continue;

                _gameFactory.CreateProjectile(
                    playerTransform.ValueRO.Position,
                    direction,
                    ecb);

                ecb.RemoveComponent<CooldownReadyOneFrame>(playerEntity);
            }

            ecb.Playback(EntityManager);
            ecb.Dispose();
            enemies.Dispose();
        }

        private bool TryCollectEnemies(out NativeList<float3> enemies)
        {
            enemies = new NativeList<float3>(Allocator.Temp);

            foreach (var enemyTransform in
                     SystemAPI.Query<RefRO<LocalTransform>>()
                         .WithAll<EnemyTag>())
            {
                enemies.Add(enemyTransform.ValueRO.Position);
            }

            if (enemies.Length == 0)
            {
                enemies.Dispose();
                return false;
            }

            return true;
        }

        private bool TryGetClosestEnemy(
            float3 playerPos,
            NativeList<float3> enemies,
            out float3 direction)
        {
            float minDistSq = float.MaxValue;
            float3 closest = float3.zero;

            foreach (var enemyPos in enemies)
            {
                float distSq = math.distancesq(playerPos, enemyPos);
                if (distSq < minDistSq)
                {
                    minDistSq = distSq;
                    closest = enemyPos;
                }
            }

            if (minDistSq == float.MaxValue)
            {
                direction = float3.zero;
                return false;
            }

            direction = math.normalize(closest - playerPos);
            return true;
        }
    }
}
