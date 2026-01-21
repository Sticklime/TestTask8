using System;
using MessagePipe;

namespace CodeBase.Logic.UI.Coin
{
    public class CoinPresenter : IDisposable
    {
        private readonly CoinView _view;
        private ISubscriber<UpdateCoinView> _subscriber;
        private IDisposable _subscription;

        public CoinPresenter(CoinView view, ISubscriber<UpdateCoinView> subscriber)
        {
            _view = view;
            _subscriber = subscriber;
        }

        public void Enable()
        {
            _subscription = _subscriber.Subscribe(value => { _view.SetCoin(value.Amount); });
        }

        public void Disable()
        {
            _subscription?.Dispose();
        }

        public void Dispose()
        {
            Disable();
        }
    }
}