using CodeBase.Logic.CameraLogic;
using CodeBase.Logic.Character.Shoot;
using CodeBase.Logic.Character.Spawn;
using CodeBase.Logic.Common.Death;
using CodeBase.Logic.Common.Destroy;
using CodeBase.Logic.Common.Health;
using CodeBase.Logic.Common.Move;
using CodeBase.Logic.Common.Projectile;
using CodeBase.Logic.Enemy.Attack;
using CodeBase.Logic.Enemy.EnemyState;
using CodeBase.Logic.Enemy.Follow;
using CodeBase.Logic.Enemy.Spawn;
using CodeBase.Logic.Loot;
using CodeBase.Logic.UI;
using Unity.Entities;
using VContainer;
using CharacterMoveSystem = CodeBase.Logic.Character.Move.CharacterMoveSystem;
using CooldownSystem = CodeBase.Logic.Common.Cooldown.CooldownSystem;

namespace CodeBase.Infrastructure.Boot
{
    public sealed class SystemBootstrapper
    {
        private World _world;
        private readonly IObjectResolver _resolver;

        private SimulationSystemGroup _simulationGroup;
        private PresentationSystemGroup _presentationGroup;
        private InitializationSystemGroup _initializationGroup;

        public SystemBootstrapper(World world, IObjectResolver resolver)
        {
            _world = world;
            _resolver = resolver;
        }

        public void Initialize()
        {
            InitializeWorld(_world);
            World.DefaultGameObjectInjectionWorld = _world;
            ScriptBehaviourUpdateOrder.AppendWorldToCurrentPlayerLoop(_world);
        }

        private void InitializeWorld(World world)
        {
            if (world == null || !world.IsCreated) return;

            _simulationGroup = world.GetOrCreateSystemManaged<SimulationSystemGroup>();
            _presentationGroup = world.GetOrCreateSystemManaged<PresentationSystemGroup>();
            _initializationGroup = world.GetOrCreateSystemManaged<InitializationSystemGroup>();

            AddSystem<CharacterInitializeSystem>(world, _initializationGroup);
            AddSystem<CameraInitializeSystem>(world, _initializationGroup);
            AddSystem<WindowInitializeSystem>(world, _initializationGroup);

            AddSystem<SyncTransformSystem>(world, _simulationGroup);
            AddSystem<EnemySpawnSystem>(world, _simulationGroup);
            AddSystem<CooldownSystem>(world, _simulationGroup);
            AddSystem<CharacterMoveSystem>(world, _simulationGroup);
            AddSystem<EnemyStateSystem>(world, _simulationGroup);
            AddSystem<EnemyFollowSystem>(world, _simulationGroup);
            AddSystem<EnemyAttackSystem>(world, _simulationGroup);
            AddSystem<ShootSystem>(world, _simulationGroup);
            AddSystem<ProjectileSystem>(world, _simulationGroup);
            AddSystem<ProjectileHitSystem>(world, _simulationGroup);
            AddSystem<LootPickupSystem>(world, _simulationGroup);
            AddSystem<ApplyDamageSystem>(world, _simulationGroup);

            AddSystem<SpawnLootOnEnemyDeathSystem>(world, _presentationGroup);
            AddSystem<EndGameSystem>(world, _presentationGroup);
            AddSystem<DeathSystem>(world, _presentationGroup);
            AddSystem<DestroySystem>(world, _presentationGroup);
            AddSystem<CameraFollowSystem>(world, _presentationGroup);
#if UNITY_EDITOR
            AddSystem<HitboxDebugSystem>(world, _presentationGroup);
#endif

            _initializationGroup.SortSystems();
            _simulationGroup.SortSystems();
            _presentationGroup.SortSystems();
        }

        private void AddSystem<TSystem>(World world, ComponentSystemGroup group)
            where TSystem : ComponentSystemBase
        {
            var existing = world.GetExistingSystemManaged<TSystem>();

            if (existing != null)
            {
                group.RemoveSystemFromUpdateList(existing);
                world.DestroySystemManaged(existing);
            }

            IScopedObjectResolver scope = _resolver.CreateScope(b =>
                b.Register<TSystem>(Lifetime.Singleton));
            TSystem system = scope.Resolve<TSystem>();

            world.AddSystemManaged(system);
            group.AddSystemToUpdateList(system);
        }

        public void StopGame()
        {
            _simulationGroup.Enabled = false;
            _presentationGroup.Enabled = false;
            _initializationGroup.Enabled = false;
        }

        public void RestartGame()
        {
            var entityManager = _world.EntityManager;

            entityManager.CompleteAllTrackedJobs();

            InitializeWorld(_world);

            _initializationGroup.Enabled = true;
            _simulationGroup.Enabled = true;
            _presentationGroup.Enabled = true;

            _initializationGroup.Update();
        }
    }
}