using CodeBase.Infrastructure.Factory;
using CodeBase.Logic.Common.Death;
using CodeBase.Logic.Enemy;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace CodeBase.Logic.Loot
{
    [DisableAutoCreation]
    [UpdateBefore(typeof(DeathSystem))]
    public partial class SpawnLootOnEnemyDeathSystem : SystemBase
    {
        private readonly IGameFactory _gameFactory;

        public SpawnLootOnEnemyDeathSystem(IGameFactory gameFactory)
        {
            _gameFactory = gameFactory;
        }

        protected override void OnUpdate()
        {
            var commandBuffer = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);

            foreach (var (transform, loot, death) in
                     SystemAPI.Query<RefRO<LocalTransform>, RefRO<DropLootComponent>, RefRO<DeathOneFrameComponent>>()
                         .WithAll<EnemyTag>())
            {
                float roll = Random.Range(0f, 100f);
                if (roll <= loot.ValueRO.ChanceFallingOut)
                    _gameFactory.CreateLoot(transform.ValueRO.Position, commandBuffer);
            }

            commandBuffer.Playback(EntityManager);
            commandBuffer.Dispose();
        }
    }
}