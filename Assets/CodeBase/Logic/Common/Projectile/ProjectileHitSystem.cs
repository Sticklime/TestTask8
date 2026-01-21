using CodeBase.Logic.Common.Destroy;
using CodeBase.Logic.Common.Health;
using CodeBase.Logic.Common.Projectile;
using CodeBase.Logic.Enemy;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[DisableAutoCreation]
public partial class ProjectileHitSystem : SystemBase
{
    protected override void OnUpdate()
    {
        var ecb = new EntityCommandBuffer(Allocator.Temp);

        foreach (var (projTransform, projDamage, projEntity) in
                 SystemAPI.Query<RefRO<LocalTransform>, RefRO<DamageComponent>>()
                     .WithAll<ProjectileTag>()
                     .WithEntityAccess())
        {
            ProcessProjectile(projTransform.ValueRO.Position, projDamage.ValueRO.DamageAmount, projEntity, ref ecb);
        }

        ecb.Playback(EntityManager);
        ecb.Dispose();
    }

    private void ProcessProjectile(float3 projPos, int damage, Entity projEntity, ref EntityCommandBuffer ecb)
    {
        foreach (var (enemyTransform, collider, enemyEntity) in
                 SystemAPI.Query<RefRO<LocalTransform>, RefRO<ColliderComponent>>()
                     .WithAll<EnemyTag>()
                     .WithEntityAccess())
        {
            if (!IsHit(projPos, enemyTransform.ValueRO.Position, collider.ValueRO.Radius))
                continue;

            ecb.AddComponent(enemyEntity, new ApplyDamageOneFrame
            {
                DamageAmount = damage
            });

            ecb.AddComponent(projEntity, new DestroyOneFrame());
            return;
        }
    }

    private static bool IsHit(float3 projPos, float3 enemyPos, float radius)
    {
        float radiusSq = radius * radius;
        return math.distancesq(projPos, enemyPos) <= radiusSq;
    }
}