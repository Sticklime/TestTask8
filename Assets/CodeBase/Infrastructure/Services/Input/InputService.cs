using System;
using UnityEngine;

namespace CodeBase.Infrastructure.Services.Input
{
    public class InputService : IInputService, IDisposable
    {
        private InputSystem_Actions _input;

        public Vector2 Move => _input.Player.Move.ReadValue<Vector2>();
        public Vector2 Look => _input.Player.Look.ReadValue<Vector2>();

        public void Initialize()
        {
            _input = new InputSystem_Actions();
            _input.Enable();
        }

        public void Dispose()
        {
            _input?.Dispose();
        }
    }
}