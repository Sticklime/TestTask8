using Unity.Collections;
using Unity.Entities;

namespace CodeBase.Logic.Common.Cooldown
{
    [DisableAutoCreation]
    public partial class CooldownSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            float deltaTime = UnityEngine.Time.deltaTime;
            EntityCommandBuffer commandBuffer = new EntityCommandBuffer(Allocator.Temp);

            foreach (var (cooldown, entity) in
                     SystemAPI.Query<RefRW<CooldownComponent>>().WithEntityAccess())
            {
                cooldown.ValueRW.TimeLeft -= deltaTime;

                if (cooldown.ValueRW.TimeLeft > 0f)
                    continue;

                cooldown.ValueRW.TimeLeft = cooldown.ValueRW.Interval;

                if (!EntityManager.HasComponent<CooldownReadyOneFrame>(entity))
                    commandBuffer.AddComponent<CooldownReadyOneFrame>(entity);
            }

            commandBuffer.Playback(EntityManager);
            commandBuffer.Dispose();
        }
    }
}