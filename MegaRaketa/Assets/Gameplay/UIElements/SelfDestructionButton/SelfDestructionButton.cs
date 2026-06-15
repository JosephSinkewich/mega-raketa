using System;
using MegaRaketa.Gameplay.Rocket;
using Zenject;

namespace MegaRaketa.Gameplay.SelfDestructionButton
{
    public class SelfDestructionButton : ISelfDestructionButton, IInitializable, IDisposable
    {
        [Inject] private SelfDestructionButtonView _view;
        [Inject] private IRocket _rocket;

        private bool _isLocked = true;

        public void Initialize()
        {
            _rocket.OnExplode += HandleRocketExplode;
            _view.Button.onClick.AddListener(HandleClick);
        }

        public void Dispose()
        {
            _rocket.OnExplode -= HandleRocketExplode;
            _view.Button.onClick.RemoveListener(HandleClick);
        }

        public void Unlock()
        {
            _isLocked = false;
            _view.Button.interactable = true;
        }

        private void HandleClick()
        {
            if (_isLocked)
            {
                return;
            }

            _rocket.Explode();
        }

        private void HandleRocketExplode()
        {
            _isLocked = true;
            _view.Button.interactable = false;
        }
    }
}
