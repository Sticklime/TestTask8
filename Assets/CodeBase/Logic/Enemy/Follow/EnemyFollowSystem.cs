using CodeBase.Logic.Player;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace CodeBase.Logic.Enemy.Follow
{
    [DisableAutoCreation]
    public partial class EnemyFollowSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            if (!HasPlayers())
                return;

            foreach (var (enemyState, enemyTransform, entity)
                     in SystemAPI.Query<
                             RefRO<EnemyStateComponent>,
                             RefRO<LocalTransform>>()
                         .WithEntityAccess())
            {
                if (enemyState.ValueRO.CurrentState != global::EnemyState.Follow)
                    continue;

                if (!EntityManager.HasComponent<NavMeshComponent>(entity))
                    continue;

                var navMesh = EntityManager.GetComponentData<NavMeshComponent>(entity);
                if (!IsAgentValid(navMesh))
                    continue;

                float3 target = FindClosestPlayer(enemyTransform.ValueRO.Position);

                navMesh.Agent.isStopped = false;
                navMesh.Agent.SetDestination(target);
            }
        }

        private bool HasPlayers()
        {
            return !SystemAPI.QueryBuilder()
                .WithAll<PlayerTag, LocalTransform>()
                .Build()
                .IsEmpty;
        }

        private float3 FindClosestPlayer(float3 enemyPos)
        {
            float3 closest = float3.zero;
            float minDistSq = float.MaxValue;

            foreach (var playerTransform
                     in SystemAPI.Query<RefRO<LocalTransform>>().WithAll<PlayerTag>())
            {
                float distSq = math.distancesq(enemyPos, playerTransform.ValueRO.Position);

                if (distSq < minDistSq)
                {
                    minDistSq = distSq;
                    closest = playerTransform.ValueRO.Position;
                }
            }

            return closest;
        }

        private bool IsAgentValid(NavMeshComponent navMesh)
        {
            return navMesh.Agent != null && navMesh.Agent.enabled;
        }
    }
}
