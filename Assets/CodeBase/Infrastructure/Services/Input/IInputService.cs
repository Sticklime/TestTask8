using UnityEngine;

namespace CodeBase.Infrastructure.Services.Input
{
    public interface IInputService
    {
        Vector2 Move { get; }
        Vector2 Look { get; }
        void Initialize();
    }
}