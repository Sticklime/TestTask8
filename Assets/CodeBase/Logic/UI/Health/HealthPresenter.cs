using MessagePipe;
using System;
using Unity.Entities;
using UnityEngine;

namespace CodeBase.Logic.UI.Health
{
    public class HealthPresenter 
    {
        [SerializeField] private HealthView _healthView;
        
        private ISubscriber<UpdateHealthView> _subscriber;
        private IDisposable _subscription;
        private Entity _trackEntity;

        public HealthPresenter(HealthView healthView, Entity trackEntity, ISubscriber<UpdateHealthView> subscriber)
        {
            _healthView = healthView;
            _trackEntity = trackEntity;
            _subscriber = subscriber;
        }

        public void Enable()
        {
            _subscription = _subscriber.Subscribe(value =>
            {
                if (value.Entity == _trackEntity)
                    _healthView.SetHealth(value.CurrentHealth, value.MaxHealth);
            });
        }

        public void Disable()
        {
            _subscription?.Dispose();
        }
    }
}