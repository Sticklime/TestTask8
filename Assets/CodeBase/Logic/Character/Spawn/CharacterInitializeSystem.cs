using CodeBase.Infrastructure.Factory;
using Unity.Entities;
using UnityEngine;

namespace CodeBase.Logic.Character.Spawn
{
    [DisableAutoCreation]
    public partial class CharacterInitializeSystem : SystemBase
    {
        private readonly IGameFactory _gameFactory;

        public CharacterInitializeSystem(IGameFactory gameFactory)
        {
            _gameFactory = gameFactory;
        }

        protected override void OnCreate()
        {
            _gameFactory.CreateCharacter(Vector3.up);
        }

        protected override void OnUpdate()
        {
        }
    }
}