using System.Collections.Generic;
using CodeBase.Infrastructure.Factory;

namespace CodeBase.Infrastructure.Services.UI
{
    public class OpenWindow : IOpenWindow
    {
        private readonly IUIFactory _uiFactory;
        private readonly Dictionary<WindowType, WindowBase> _windows = new();

        public OpenWindow(IUIFactory uiFactory)
        {
            _uiFactory = uiFactory;
        }

        public void RegisterWindow(WindowType type)
        {
            if (!_windows.ContainsKey(type))
            {
                var window = _uiFactory.CreateWindow(type);
                _windows.Add(type, window);
            }
        }

        public void Open(WindowType type)
        {
            if (_windows.TryGetValue(type, out var window))
            {
                window.gameObject.SetActive(true);
                window.Enable();
            }
        }

        public bool HasWindow(WindowType type) =>
            _windows.ContainsKey(type);

        public void Close(WindowType type)
        {
            if (_windows.TryGetValue(type, out var window))
            {
                window.gameObject.SetActive(false);
                window.Disable();
            }
        }

        public void CloseAll()
        {
            foreach (var window in _windows.Values)
            {
                window.gameObject.SetActive(false);
                window.Disable();
            }
        }
    }
}