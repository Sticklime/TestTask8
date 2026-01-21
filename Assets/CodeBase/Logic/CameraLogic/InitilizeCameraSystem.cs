using CodeBase.Config;
using CodeBase.Infrastructure.Services.Config;
using Unity.Entities;
using Unity.Transforms;

namespace CodeBase.Logic.CameraLogic
{
    [DisableAutoCreation]
    public partial class CameraInitializeSystem : SystemBase
    {
        private readonly World _world;
        private readonly IConfigProvider _configProvider;
        private UnityEngine.Camera _mainCamera;

        public CameraInitializeSystem(World world, IConfigProvider configProvider)
        {
            _world = world;
            _configProvider = configProvider;
        }

        protected override void OnCreate()
        {
            _mainCamera = UnityEngine.Camera.main;

            EntityManager entityManager = _world.EntityManager;
            Entity cameraEntity = entityManager.CreateEntity();

            var cameraConfig = _configProvider.GetCameraConfig();

            InitCamera(entityManager, cameraEntity, cameraConfig);
        }

        private void InitCamera(EntityManager entityManager, Entity cameraEntity, CameraConfig cameraConfig)
        {
            entityManager.AddComponentData(cameraEntity, new FollowCameraComponent
            {
                RotationAngleX = cameraConfig.RotationAngleX,
                Distance = cameraConfig.Distance,
                OffsetY = cameraConfig.OffsetY
            });

            entityManager.AddComponent<CameraTag>(cameraEntity);

            entityManager.AddComponentData(cameraEntity,
                LocalTransform.FromPositionRotationScale(
                    new Unity.Mathematics.float3(
                        _mainCamera.transform.position.x,
                        _mainCamera.transform.position.y,
                        _mainCamera.transform.position.z),
                    _mainCamera.transform.rotation,
                    1f
                )
            );
        }

        protected override void OnUpdate()
        {
        }
    }
}