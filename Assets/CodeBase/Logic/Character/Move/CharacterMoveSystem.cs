using CodeBase.Infrastructure.Services.Input;
using CodeBase.Logic.Common.Move;
using CodeBase.Logic.Player;
using CodeBase.Logic.Player.Move;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace CodeBase.Logic.Character.Move
{
    [DisableAutoCreation]
    public partial class CharacterMoveSystem : SystemBase
    {
        private readonly IInputService _inputService;

        public CharacterMoveSystem(IInputService inputService)
        {
            _inputService = inputService;
        }

        protected override void OnUpdate()
        {
            float deltaTime = UnityEngine.Time.deltaTime;
            float2 moveInput = _inputService.Move;

            foreach (var (_, moveComponent, entity) in
                     SystemAPI.Query<RefRW<LocalTransform>, RefRO<MoveComponent>>().WithAll<PlayerTag>()
                         .WithEntityAccess())
            {
                CharacterControllerComponent controller =
                    EntityManager.GetComponentObject<CharacterControllerComponent>(entity);

                ApplyInput(moveInput, moveComponent, deltaTime, controller);
            }
        }

        private static void ApplyInput(float2 moveInput, RefRO<MoveComponent> moveComponent, float deltaTime,
            CharacterControllerComponent controller)
        {
            float3 direction = new float3(moveInput.x, 0f, moveInput.y);
            float3 move = direction * moveComponent.ValueRO.MoveSpeed * deltaTime;

            controller.CharacterController.Move(move);
        }
    }
}