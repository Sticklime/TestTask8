using CodeBase.Logic.Common.Move;
using Unity.Entities;
using UnityEngine;

namespace CodeBase.Logic.Common.Destroy
{
    [DisableAutoCreation]
    public partial class DestroySystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var ecb = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);

            foreach (var (destroy, entity) in SystemAPI.Query<DestroyOneFrame>().WithEntityAccess())
            {
                if (EntityManager.HasComponent<TransformReference>(entity))
                {
                    var transformRef = EntityManager.GetComponentData<TransformReference>(entity);
                    if (transformRef.Transform != null)
                        Object.Destroy(transformRef.Transform.gameObject); 
                }

                ecb.DestroyEntity(entity);
            }

            ecb.Playback(EntityManager);
            ecb.Dispose();
        }
    }
}