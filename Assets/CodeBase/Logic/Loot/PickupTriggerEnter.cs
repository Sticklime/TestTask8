using CodeBase.Logic.Character.Wallet;
using CodeBase.Logic.Common.Destroy;
using CodeBase.Logic.Player;
using CodeBase.Logic.UI.Coin;
using MessagePipe;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace CodeBase.Logic.Loot
{
    [DisableAutoCreation]
    public partial class LootPickupSystem : SystemBase
    {
        private readonly IPublisher<UpdateCoinView> _coinPublisher;

        public LootPickupSystem(IPublisher<UpdateCoinView> coinPublisher)
        {
            _coinPublisher = coinPublisher;
        }

        protected override void OnUpdate()
        {
            var ecb = new EntityCommandBuffer(Allocator.Temp);

            foreach (var (lootTransform, lootHitbox, loot, lootEntity) in
                     SystemAPI.Query<RefRO<LocalTransform>, RefRO<ColliderComponent>, RefRO<LootComponent>>()
                         .WithAll<LootTag>()
                         .WithEntityAccess())
            {
                TryPickupLoot(
                    lootTransform.ValueRO,
                    lootHitbox.ValueRO,
                    loot.ValueRO,
                    lootEntity,
                    ref ecb
                );
            }

            ecb.Playback(EntityManager);
            ecb.Dispose();
        }

        private void TryPickupLoot(
            LocalTransform lootTransform,
            ColliderComponent lootHitbox,
            LootComponent loot,
            Entity lootEntity,
            ref EntityCommandBuffer ecb)
        {
            float lootRadius = GetScaledRadius(lootHitbox.Radius, lootTransform.Scale);

            foreach (var (playerTransform, playerHitbox, wallet, playerEntity) in
                     SystemAPI.Query<RefRO<LocalTransform>, RefRO<ColliderComponent>, RefRW<WalletComponent>>()
                         .WithAll<PlayerTag>()
                         .WithEntityAccess())
            {
                float playerRadius = GetScaledRadius(playerHitbox.ValueRO.Radius, playerTransform.ValueRO.Scale);

                if (!IsIntersect(
                        lootTransform.Position,
                        playerTransform.ValueRO.Position,
                        lootRadius + playerRadius))
                    continue;

                wallet.ValueRW.ValueCoin += loot.Amount;
                _coinPublisher.Publish(new UpdateCoinView(wallet.ValueRW.ValueCoin));

                ecb.AddComponent<DestroyOneFrame>(lootEntity);
                return;
            }
        }

        private static float GetScaledRadius(float radius, float3 scale) =>
            radius * math.cmax(scale);

        private static bool IsIntersect(float3 a, float3 b, float radius) =>
            math.distancesq(a, b) <= radius * radius;
    }
}
