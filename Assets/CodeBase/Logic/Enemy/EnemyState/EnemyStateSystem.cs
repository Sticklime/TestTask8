using CodeBase.Config;
using CodeBase.Infrastructure.Services.Config;
using CodeBase.Logic.Player;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace CodeBase.Logic.Enemy.EnemyState
{
    [DisableAutoCreation]
    public partial class EnemyStateSystem : SystemBase
    {
        private readonly IConfigProvider _configProvider;
        private EnemyConfig _enemyConfig;

        public EnemyStateSystem(IConfigProvider configProvider)
        {
            _configProvider = configProvider;
        }

        protected override void OnCreate()
        {
            _enemyConfig = _configProvider.GetEnemyConfig();
        }

        protected override void OnUpdate()
        {
            if (!TryCollectPlayers(out var players))
                return;

            float attackRangeSq = _enemyConfig.AttackRange * _enemyConfig.AttackRange;

            foreach (var (enemyTransform, enemyState)
                     in SystemAPI.Query<
                         RefRO<LocalTransform>,
                         RefRW<EnemyStateComponent>>())
            {
                float minDistSq = FindClosestPlayerDistanceSq(enemyTransform.ValueRO.Position, players);

                enemyState.ValueRW.CurrentState =
                    minDistSq <= attackRangeSq
                        ? global::EnemyState.Attack
                        : global::EnemyState.Follow;
            }

            players.Dispose();
        }

        private bool TryCollectPlayers(out NativeList<float3> players)
        {
            players = new NativeList<float3>(Allocator.Temp);

            foreach (var playerTransform
                     in SystemAPI.Query<RefRO<LocalTransform>>().WithAll<PlayerTag>())
            {
                players.Add(playerTransform.ValueRO.Position);
            }

            if (players.Length == 0)
            {
                players.Dispose();
                return false;
            }

            return true;
        }

        private float FindClosestPlayerDistanceSq(float3 enemyPos, NativeList<float3> players)
        {
            float minDistSq = float.MaxValue;

            foreach (var playerPos in players)
            {
                float distSq = math.distancesq(enemyPos, playerPos);
                if (distSq < minDistSq)
                    minDistSq = distSq;
            }

            return minDistSq;
        }
    }
}
