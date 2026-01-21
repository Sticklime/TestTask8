using MessagePipe;
using UnityEngine;
using VContainer;

namespace CodeBase.Logic.UI.Restart
{
    public class RestartPresenter : MonoBehaviour
    {
        [SerializeField] private RestartView _view;
        
        private IPublisher<RestartGame> _restartPublisher;

        [Inject]
        public void Construct(IPublisher<RestartGame> restartPublisher)
        {
            _restartPublisher = restartPublisher;
        }

        public void Enable()
        {
            _view.RestartButton.onClick.AddListener(Restart);
            _view.gameObject.SetActive(true);
        }

        private void Restart()
        {
            _restartPublisher.Publish(new RestartGame());
            Disable();
        }

        public void Disable()
        {
            _view.RestartButton.onClick.RemoveListener(Restart);
            _view.gameObject.SetActive(false);
        }
    }
}