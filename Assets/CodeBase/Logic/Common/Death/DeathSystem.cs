using CodeBase.Logic.Common.Destroy;
using Unity.Collections;
using Unity.Entities;

namespace CodeBase.Logic.Common.Death
{
    [DisableAutoCreation]
    public partial class DeathSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var commandBuffer = new EntityCommandBuffer(Allocator.Temp);
            
            foreach (var (deathOneFrame, entity) in SystemAPI.Query<DeathOneFrameComponent>().WithEntityAccess())
            {
                commandBuffer.AddComponent<DestroyOneFrame>(entity);
                
                if (HasComponent<DeathOneFrameComponent>(entity)) 
                    commandBuffer.RemoveComponent<DeathOneFrameComponent>(entity);
            }

            commandBuffer.Playback(EntityManager);
            commandBuffer.Dispose();
        }
    }
}