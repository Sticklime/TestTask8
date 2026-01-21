using UnityEngine;

namespace CodeBase.Logic.UI.Health
{
    public class LookAtCamera : MonoBehaviour
    {
        private Camera _mainCamera;

        private void Awake()
        {
            _mainCamera = Camera.main;
        }

        private void LateUpdate()
        {
            if (_mainCamera == null) return;
            
            Vector3 direction = _mainCamera.transform.position - transform.position;
            direction.y = 0f;

            if (direction.sqrMagnitude > 0f)
            {
                transform.rotation = Quaternion.LookRotation(-direction);
            }

            transform.localScale = Vector3.one;
        }
    }
}