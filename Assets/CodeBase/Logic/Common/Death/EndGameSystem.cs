using CodeBase.Logic.Common.Destroy;
using CodeBase.Logic.Player;
using CodeBase.Logic.UI.Restart;
using MessagePipe;
using Unity.Collections;
using Unity.Entities;

namespace CodeBase.Logic.Common.Death
{
    [UpdateBefore(typeof(DeathSystem))]
    [DisableAutoCreation]
    public partial class EndGameSystem : SystemBase
    {
        private readonly IPublisher<EndGame> _endGamePublisher;
        private EndSimulationEntityCommandBufferSystem _ecbSystem;

        public EndGameSystem(IPublisher<EndGame> endGamePublisher)
        {
            _endGamePublisher = endGamePublisher;
        }

        protected override void OnCreate()
        {
            _ecbSystem = World.GetOrCreateSystemManaged<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            var commandBuffer = _ecbSystem.CreateCommandBuffer();

            bool endGame = false;
            foreach (var _ in SystemAPI.Query<DeathOneFrameComponent>().WithAll<PlayerTag>())
            {
                endGame = true;
                break;
            }

            if (endGame)
            {
                _endGamePublisher.Publish(new EndGame());

                using var entities = EntityManager.GetAllEntities(Allocator.Temp);
                foreach (var entity in entities)
                {
                    if (!EntityManager.HasComponent<DestroyOneFrame>(entity))
                    {
                        commandBuffer.AddComponent<DestroyOneFrame>(entity);
                    }
                }
            }

            _ecbSystem.Update();
        }
    }
}