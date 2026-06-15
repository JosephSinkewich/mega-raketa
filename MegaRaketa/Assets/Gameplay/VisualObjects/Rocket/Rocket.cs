using System;
using System.Threading;
using MegaRaketa.Gameplay.Asteroids;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace MegaRaketa.Gameplay.Rocket
{
    public class Rocket : IRocket, ITickable, IInitializable, IDisposable
    {
        [Inject] private RocketView _view;
        [Inject] private RocketConfig _config;
        [Inject] private IInstantiator _instantiator;

        private float _speed;
        private float _deviationAngle;
        private bool _isLaunched;
        private bool _isExploded;
        private Quaternion _startRotation;
        private CancellationTokenSource _cancellationTokenSource;

        public event Action<RocketAsteroidCollisionEventData> OnAsteroidCollide;
        public event Action OnExplode;

        public Vector3 Position => _view.transform.position;
        public float DeviationAngle => _deviationAngle;

        public void Initialize()
        {
            _startRotation = _view.transform.rotation;
            _view.TriggerEntered += HandleTriggerEnter;
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public void Dispose()
        {
            _view.TriggerEntered -= HandleTriggerEnter;
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
        }

        public void Tick()
        {
            if (_isExploded)
            {
                return;
            }

            Vector3 direction = _view.transform.rotation * Vector3.up;
            _view.transform.position += direction * (_speed * Time.deltaTime);
        }

        public void Launch()
        {
            if (_isLaunched)
            {
                return;
            }

            _isLaunched = true;
            _view.EngineFire.Play();
            AccelerateAsync().Forget();
        }

        public void Explode()
        {
            if (_isExploded)
            {
                return;
            }

            _isExploded = true;
            OnExplode?.Invoke();
            SpawnExplosionEffect();
            _view.DestroyObject();
        }

        public void RotateTo(Vector3 targetPoint)
        {
            Vector3 directionToTarget = targetPoint - _view.transform.position;

            if (directionToTarget == Vector3.zero)
            {
                return;
            }

            Vector3 startDirection = _startRotation * Vector3.up;
            float targetDeviationAngle = Vector3.SignedAngle(startDirection, directionToTarget, Vector3.forward);

            targetDeviationAngle = Mathf.Clamp(targetDeviationAngle, -_config.MaxDeviationAngle, _config.MaxDeviationAngle);
            _deviationAngle = Mathf.MoveTowards(_deviationAngle, targetDeviationAngle, _config.RotationSpeed * Time.deltaTime);

            _view.transform.rotation = Quaternion.AngleAxis(_deviationAngle, Vector3.forward) * _startRotation;
        }

        private async UniTask AccelerateAsync()
        {
            if (_config.Acceleration <= 0f)
            {
                return;
            }

            while (_speed < _config.MaxSpeed)
            {
                _speed = Mathf.MoveTowards(_speed, _config.MaxSpeed, _config.Acceleration * Time.deltaTime);
                await UniTask.Yield(cancellationToken: _cancellationTokenSource.Token);
            }
        }

        private void HandleTriggerEnter(Collider2D other)
        {
            DestroyAsteroid(other);
        }

        private void SpawnExplosionEffect()
        {
            if (_config.ExplosionEffect == null)
            {
                return;
            }

            _instantiator.InstantiatePrefab(
                _config.ExplosionEffect,
                _view.transform.position,
                _view.transform.rotation,
                _view.transform.parent);
        }

        private void DestroyAsteroid(Collider2D other)
        {
            if (_view.Collider == null || !_view.Collider.IsTouching(other))
            {
                return;
            }

            AsteroidView asteroidView = other.GetComponentInParent<AsteroidView>();

            if (asteroidView == null || asteroidView.Asteroid == null)
            {
                return;
            }

            RocketAsteroidCollisionEventData eventData = new RocketAsteroidCollisionEventData(asteroidView.Asteroid.Size);
            asteroidView.Asteroid.Explode();
            OnAsteroidCollide?.Invoke(eventData);
        }
    }
}
