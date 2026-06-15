using MegaRaketa.Gameplay.VisualObjects;
using MegaRaketa.Gameplay.VisualObjects.Rocket;
using UnityEngine;
using Zenject;

namespace MegaRaketa.Gameplay.VisualObjects.Asteroids
{
    public class AsteroidsCleaner : ITickable, IInitializable, System.IDisposable
    {
        [Inject] private AsteroidsCleanerConfig _config;
        [Inject] private IRocket _rocket;
        [Inject] private ISceneVisualObjects _sceneVisualObjects;

        private bool _isLocked;

        public void Initialize()
        {
            _rocket.OnExplode += HandleRocketExplode;
        }

        public void Dispose()
        {
            _rocket.OnExplode -= HandleRocketExplode;
        }

        public void Tick()
        {
            if (_isLocked)
            {
                return;
            }

            float cleanupY = _rocket.Position.y - _config.YThresholdFromRocket;
            Transform container = _sceneVisualObjects.AsteroidsContainer;

            for (int i = container.childCount - 1; i >= 0; i--)
            {
                Transform child = container.GetChild(i);
                AsteroidView asteroidView = child.GetComponent<AsteroidView>();

                if (asteroidView == null)
                {
                    continue;
                }

                if (child.position.y < cleanupY)
                {
                    asteroidView.Asteroid?.Dispose();
                    Object.Destroy(asteroidView.gameObject);
                }
            }
        }

        private void HandleRocketExplode()
        {
            _isLocked = true;
        }
    }
}
