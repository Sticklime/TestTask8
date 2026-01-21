using CodeBase.Logic.Common.Move;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace CodeBase.Logic.Common.Projectile
{
    [DisableAutoCreation]
    public partial class ProjectileSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            float deltaTime = UnityEngine.Time.deltaTime;
            
            foreach (var (projectileRO, moveRO, entity) 
                     in SystemAPI.Query<RefRO<ProjectileComponent>, RefRO<MoveComponent>>()
                         .WithEntityAccess())
            {
                if (!EntityManager.HasComponent<TransformReference>(entity))
                    continue;

                TransformReference transformRef = EntityManager.GetComponentData<TransformReference>(entity);
                Transform transform = transformRef.Transform;

                if (transform == null)
                    continue;
                
                float3 move = math.normalize(projectileRO.ValueRO.Direction) * moveRO.ValueRO.MoveSpeed * deltaTime;
                transform.position += new Vector3(move.x, move.y, move.z);
                transform.rotation = Quaternion.LookRotation(projectileRO.ValueRO.Direction, Vector3.up);
            }
        }
    }
}