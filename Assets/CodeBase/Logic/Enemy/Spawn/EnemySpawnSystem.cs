using CodeBase.Config;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services.Config;
using CodeBase.Logic.Common.Cooldown;
using Unity.Entities;
using UnityEngine;

//По хорошему надо пул сделать, но я не успел

namespace CodeBase.Logic.Enemy.Spawn
{
    [DisableAutoCreation]
    public partial class EnemySpawnSystem : SystemBase
    {
        private readonly IConfigProvider _configProvider;
        private readonly IGameFactory _gameFactory;
        private EnemyConfig _enemyConfig;
        private Camera _mainCamera;
        private Entity _entityForTimer;

        private EnemySpawnSystem(IConfigProvider configProvider, IGameFactory gameFactory)
        {
            _configProvider = configProvider;
            _gameFactory = gameFactory;
        }

        protected override void OnCreate()
        {
            _mainCamera = Camera.main;
            _enemyConfig = _configProvider.GetEnemyConfig();

            CreateEntityCooldown();
        }

        protected override void OnUpdate()
        {
            if (EntityManager.HasComponent<CooldownReadyOneFrame>(_entityForTimer))
            {
                SpawnEnemy();
                EntityManager.RemoveComponent<CooldownReadyOneFrame>(_entityForTimer);
            }
        }

        private void CreateEntityCooldown()
        {
            _entityForTimer = EntityManager.CreateEntity(typeof(CooldownComponent));

            EntityManager.SetComponentData(_entityForTimer, new CooldownComponent
            {
                Interval = _enemyConfig.SpawnInterval
            });
        }


        private void SpawnEnemy() =>
            _gameFactory.CreateEnemy(GetSpawnPositionOutsideCamera(_enemyConfig.SpawnOffset));

        private Vector3 GetSpawnPositionOutsideCamera(float offset)
        {
            float depth = _mainCamera.nearClipPlane + 1f;

            Vector3 minWorld = _mainCamera.ViewportToWorldPoint(new Vector3(0f, 0f, depth));
            Vector3 maxWorld = _mainCamera.ViewportToWorldPoint(new Vector3(1f, 1f, depth));

            bool spawnHorizontally = Random.value > 0.5f;
            float sideSign = Random.value > 0.5f ? 1f : -1f;

            float xPosition, zPosition;

            if (spawnHorizontally)
            {
                xPosition = sideSign > 0f ? maxWorld.x + offset : minWorld.x - offset;
                zPosition = Random.Range(minWorld.z, maxWorld.z);
            }
            else
            {
                xPosition = Random.Range(minWorld.x, maxWorld.x);
                zPosition = sideSign > 0f ? maxWorld.z + offset : minWorld.z - offset;
            }

            return new Vector3(xPosition, 0f, zPosition);
        }
    }
}