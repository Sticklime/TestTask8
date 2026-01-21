using Unity.Entities;
using Unity.Transforms;

namespace CodeBase.Logic.Common.Move
{
    [DisableAutoCreation]
    public partial class SyncTransformSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            foreach (var (localTransform,entity) in SystemAPI.Query<RefRW<LocalTransform>>().WithEntityAccess())
            {
                if (!EntityManager.HasComponent<TransformReference>(entity))
                    continue;

                TransformReference transformReference = EntityManager.GetComponentObject<TransformReference>(entity);
                
                localTransform.ValueRW.Position = transformReference.Transform.position;
                localTransform.ValueRW.Rotation = transformReference.Transform.rotation;
                localTransform.ValueRW.Scale = transformReference.Transform.localScale.x;
            }
        }
    }
}