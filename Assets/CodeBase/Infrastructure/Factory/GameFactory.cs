using CodeBase.Config;
using CodeBase.Infrastructure.Services.Config;
using CodeBase.Logic.Character.Wallet;
using CodeBase.Logic.Common.Cooldown;
using CodeBase.Logic.Common.Health;
using CodeBase.Logic.Common.Move;
using CodeBase.Logic.Common.Projectile;
using CodeBase.Logic.Enemy;
using CodeBase.Logic.Enemy.Attack;
using CodeBase.Logic.Enemy.Follow;
using CodeBase.Logic.Loot;
using CodeBase.Logic.Player;
using CodeBase.Logic.Player.Move;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.Infrastructure.Factory
{
    /// <summary>
    /// Сделано из соображений расширения в абстрактную фабрику
    /// </summary>
    public sealed class GameFactory : IGameFactory
    {
        private readonly World _world;
        private readonly IUIFactory _uiFactory;
        private readonly IConfigProvider _configProvider;

        public GameFactory(World world, IUIFactory uiFactory, IConfigProvider configProvider)
        {
            _world = world;
            _uiFactory = uiFactory;
            _configProvider = configProvider;
        }

        public void CreateCharacter(Vector3 at)
        {
            CharacterConfig characterConfig = _configProvider.GetCharacterConfig();

            GameObject characterInstance = Object.Instantiate(characterConfig.PrefabReference, at, Quaternion.identity);

            EntityManager entityManager = _world.EntityManager;
            Entity characterEntity = entityManager.CreateEntity();

            Transform transform = characterInstance.transform;
            CharacterController characterController = transform.GetComponent<CharacterController>();

            entityManager.AddComponent<PlayerTag>(characterEntity);
            entityManager.AddComponentData(characterEntity, new TransformReference { Transform = transform });
            entityManager.AddComponentData(characterEntity,
                new CharacterControllerComponent { CharacterController = characterController });
            entityManager.AddComponentData(characterEntity, new MoveComponent { MoveSpeed = characterConfig.MoveSpeed });
            entityManager.AddComponentData(characterEntity,
                new ColliderComponent() { Radius = characterConfig.RadiusCollider });
            entityManager.AddComponentData(characterEntity, new WalletComponent());
            entityManager.AddComponentData(characterEntity,
                new CooldownComponent() { Interval = characterConfig.AttackCooldown });
            entityManager.AddComponentData(characterEntity, new HealthComponent
            {
                CurrentHealth = characterConfig.MaxHealth,
                MaxHealth = characterConfig.MaxHealth
            });
            entityManager.AddComponentData(characterEntity,
                LocalTransform.FromPositionRotationScale(
                    transform.position,
                    transform.rotation,
                    1f));

            _uiFactory.CreateHUD(characterEntity);
        }

        public void CreateEnemy(Vector3 at)
        {
            EnemyConfig enemyConfig = _configProvider.GetEnemyConfig();

            GameObject enemyInstance = Object.Instantiate(enemyConfig.PrefabReference, at, Quaternion.identity);

            EntityManager entityManager = _world.EntityManager;
            Entity enemyEntity = entityManager.CreateEntity();

            Transform transform = enemyInstance.transform;

            var agent = enemyInstance.GetComponent<NavMeshAgent>();

            entityManager.AddComponent<EnemyTag>(enemyEntity);
            entityManager.AddComponentData(enemyEntity, new MoveComponent { MoveSpeed = enemyConfig.MoveSpeed });
            entityManager.AddComponentData(enemyEntity, new AttackComponent { DamageValue = enemyConfig.DamageValue });
            entityManager.AddComponentData(enemyEntity, new CooldownComponent { Interval = enemyConfig.AttackCooldown });
            entityManager.AddComponentData(enemyEntity, new ColliderComponent { Radius = enemyConfig.RadiusHitBox });
            entityManager.AddComponentData(enemyEntity, new EnemyStateComponent { });
            entityManager.AddComponentData(enemyEntity, new TransformReference { Transform = transform });
            entityManager.AddComponentData(enemyEntity, new NavMeshComponent { Agent = agent });
            entityManager.AddComponentData(enemyEntity, LocalTransform.FromPositionRotationScale(
                transform.position,
                transform.rotation,
                1f));
            entityManager.AddComponentData(enemyEntity,
                new DropLootComponent() { ChanceFallingOut = enemyConfig.ChanceFallingOutLoot });
            entityManager.AddComponentData(enemyEntity,
                new HealthComponent { CurrentHealth = enemyConfig.MaxHealth, MaxHealth = enemyConfig.MaxHealth });

            _uiFactory.CreateEnemyHealthBar(enemyEntity, enemyInstance);
        
        }

        public void CreateProjectile(Vector3 at, Vector3 targetDirection, EntityCommandBuffer commandBuffer)
        {
            ProjectileConfig projectileConfig = _configProvider.GetProjectileConfig();

            GameObject projectileInstance = Object.Instantiate(projectileConfig.Prefab, at, Quaternion.identity);
            Transform transform = projectileInstance.transform;

            Entity projectileEntity = commandBuffer.CreateEntity();

            commandBuffer.AddComponent(projectileEntity, new ProjectileTag());
            commandBuffer.AddComponent(projectileEntity, new TransformReference { Transform = transform });
            commandBuffer.AddComponent(projectileEntity, new MoveComponent { MoveSpeed = projectileConfig.MoveSpeed });
            commandBuffer.AddComponent(projectileEntity, new ProjectileComponent { Direction = targetDirection });
            commandBuffer.AddComponent(projectileEntity, new DamageComponent { DamageAmount = projectileConfig.Damage });
            commandBuffer.AddComponent(projectileEntity, new CooldownComponent { Interval = projectileConfig.LifeTime, });
            commandBuffer.AddComponent(projectileEntity, LocalTransform.FromPositionRotationScale(
                transform.position,
                transform.rotation,
                1f));
        }

        public void CreateLoot(Vector3 at, EntityCommandBuffer commandBuffe)
        {
            LootConfig lootConfig = _configProvider.GetLootConfig();

            GameObject lootInstance = Object.Instantiate(lootConfig.PrefabReference, at, Quaternion.identity);

            Entity lootEntity = commandBuffe.CreateEntity();

            Transform transform = lootInstance.transform;

            commandBuffe.AddComponent(lootEntity, new LootTag());
            commandBuffe.AddComponent(lootEntity, new TransformReference { Transform = transform });
            commandBuffe.AddComponent(lootEntity, new ColliderComponent() { Radius = lootConfig.RadiusHitBox });
            commandBuffe.AddComponent(lootEntity, new LootComponent() { Amount = lootConfig.CountInOneDrop });
            commandBuffe.AddComponent(lootEntity, LocalTransform.FromPositionRotationScale(
                transform.position,
                transform.rotation,
                1f));
        }
    }
}