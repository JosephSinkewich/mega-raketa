using MegaRaketa.Gameplay.VisualObjects.Rocket;
using UnityEngine;
using Zenject;

namespace MegaRaketa.Gameplay.VisualObjects.Asteroids
{
    public class AsteroidsCleaner : IInitializable, System.IDisposable    {
        [Inject] private AsteroidsCleanerView _view;
        [Inject] private IRocket _rocket;

        private bool _isLocked;

        public void Initialize()
        {
            _rocket.OnExplode += HandleRocketExplode;
            _view.TriggerExited += HandleTriggerExit;
        }

        public void Dispose()
        {
            _rocket.OnExplode -= HandleRocketExplode;
            _view.TriggerExited -= HandleTriggerExit;
        }

        private void HandleRocketExplode()
        {
            _isLocked = true;
        }

        private void HandleTriggerExit(Collider2D other)
        {
            if (_isLocked)
            {
                return;
            }

            AsteroidView asteroidView = other.GetComponent<AsteroidView>();

            if (asteroidView == null)
            {
                return;
            }

            asteroidView.Asteroid?.Dispose();
            Object.Destroy(asteroidView.gameObject);
        }
    }
}
