using CodeBase.Logic.Player;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace CodeBase.Logic.CameraLogic
{
    [DisableAutoCreation]
    public partial class CameraFollowSystem : SystemBase
    {
        private Camera _mainCamera;

        protected override void OnCreate()
        {
            _mainCamera = Camera.main;
        }

        protected override void OnUpdate()
        {
            foreach (var (camera, follow) in
                     SystemAPI.Query<RefRW<LocalTransform>, RefRO<FollowCameraComponent>>()
                         .WithAll<CameraTag>())
            {
                foreach (var player in SystemAPI.Query<RefRO<LocalTransform>>().WithAll<PlayerTag>())
                {
                    FollowToPlayer(follow, player);
                }
            }
        }

        private void FollowToPlayer(RefRO<FollowCameraComponent> follow, RefRO<LocalTransform> player)
        {
            Quaternion rotation = Quaternion.Euler(follow.ValueRO.RotationAngleX, 0f, 0f);
            Vector3 offset = new Vector3(0f, 0f, -follow.ValueRO.Distance);
            Vector3 targetPos = rotation * offset + new Vector3(
                player.ValueRO.Position.x,
                player.ValueRO.Position.y + follow.ValueRO.OffsetY,
                player.ValueRO.Position.z
            );

            _mainCamera.transform.position = targetPos;
            _mainCamera.transform.rotation = rotation;
        }
    }
}