using UnityEngine;

namespace CodeBase.Infrastructure.Services.UI
{
    public class WindowBase : MonoBehaviour
    {
        [SerializeField] private WindowType _windowType;
        
        public WindowType WindowType => _windowType;

        public virtual void Enable()
        {
            gameObject.SetActive(true);
        }

        public virtual void Disable()
        {
            gameObject.SetActive(false);
        }
    }
}