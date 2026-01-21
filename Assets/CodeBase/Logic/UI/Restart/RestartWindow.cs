using System;
using CodeBase.Infrastructure.Services.UI;
using UnityEngine;

namespace CodeBase.Logic.UI.Restart
{
    public class RestartWindow : WindowBase
    {
        [SerializeField] private RestartPresenter _restartPresenter;

        public override void Enable()
        {
            _restartPresenter.Enable();
        }

        public override void Disable()
        {
            _restartPresenter.Disable();
        }
    }
}