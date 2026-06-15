using MegaRaketa.Gameplay.VisualObjects;
using UnityEngine;
using Zenject;

namespace MegaRaketa.Gameplay.VisualObjects.Asteroids
{
    public class Asteroid : ITickable, System.IDisposable
    {
        [Inject] private AsteroidConfig _config;
        [Inject] private ISceneVisualObjects _sceneVisualObjects;
        [Inject] private IInstantiator _instantiator;
        [Inject] private TickableManager _tickableManager;

        private AsteroidView _view;
        private float _speed;
        private float _rotationSpeed;
        private float _size;
        private Vector2 _direction;
        private bool _isDisposed;

        public float Size => _size;

        public void Initialize(AsteroidView view, float speed, float rotationSpeed, float size)
        {
            _view = view;
            _view.Asteroid = this;
            _speed = speed;
            _rotationSpeed = rotationSpeed;
            _size = size;
            _direction = Random.insideUnitCircle.normalized;

            if (_direction == Vector2.zero)
            {
                _direction = Vector2.down;
            }

            _view.transform.localScale = Vector3.one * size;
        }

        public void Tick()
        {
            if (_isDisposed || _view == null)
            {
                return;
            }

            Vector3 direction = new Vector3(_direction.x, _direction.y, 0f);
            _view.transform.position += direction * (_speed * Time.deltaTime);
            _view.transform.Rotate(Vector3.forward, _rotationSpeed * Time.deltaTime);
        }

        public void Explode()
        {
            if (_isDisposed)
            {
                return;
            }

            GameObject explosionEffect = _instantiator.InstantiatePrefab(
                _config.ExplosionEffect,
                _view.transform.position,
                Quaternion.identity,
                _sceneVisualObjects.AsteroidsContainer);
            explosionEffect.transform.localScale = _view.transform.localScale;
            Object.Destroy(_view.gameObject);
            Dispose();
        }

        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }

            _isDisposed = true;
            _tickableManager.Remove(this);

            if (_view != null)
            {
                _view.Asteroid = null;
            }
        }
    }
}
