using MegaRaketa.Gameplay.Rocket;
using UnityEngine;
using Zenject;

namespace MegaRaketa.Gameplay.Asteroids
{
    public class AsteroidsSpawner : MonoBehaviour, IAsteroidsSpawner
    {
        [SerializeField] private Asteroid _asteroidPrefab;
        [SerializeField] private Transform _asteroidsParent;

        [Inject] private AsteroidsSpawnerConfig _config;
        [Inject] private IRocket _rocket;

        private float _sectionStartY;
        private float _sectionEndY;
        private float _asteroidsFrequency;
        private float _nextSpawnTime;
        private bool _isLocked = true;

        private void Update()
        {
            if (_isLocked || _asteroidPrefab == null)
            {
                return;
            }

            if (_rocket.Position.y >= _sectionEndY)
            {
                InitializeSection();
            }

            if (Time.time >= _nextSpawnTime)
            {
                SpawnAsteroid();
                ScheduleNextSpawn();
            }
        }

        public void Unlock()
        {
            if (!_isLocked)
            {
                return;
            }

            _isLocked = false;
            InitializeSection();
            ScheduleNextSpawn();
        }

        private void InitializeSection()
        {
            float sectionLength = Random.Range(_config.SectionLengthRange.x, _config.SectionLengthRange.y);

            _sectionStartY = _rocket.Position.y;
            _sectionEndY = _sectionStartY + sectionLength;
            _asteroidsFrequency = Random.Range(_config.AsteroidsFrequencyRange.x, _config.AsteroidsFrequencyRange.y);
        }

        private void SpawnAsteroid()
        {
            Vector3 position = new Vector3(
                _rocket.Position.x + Random.Range(-_config.SpawnWidth * 0.5f, _config.SpawnWidth * 0.5f),
                _rocket.Position.y + _config.SpawnDistanceFromRocket,
                _rocket.Position.z);

            Asteroid asteroid = Instantiate(_asteroidPrefab, position, Quaternion.identity, _asteroidsParent);
            asteroid.Initialize(
                Random.Range(_config.AsteroidsSpeedRange.x, _config.AsteroidsSpeedRange.y),
                Random.Range(_config.AsteroidsRotationSpeedRange.x, _config.AsteroidsRotationSpeedRange.y),
                Random.Range(_config.AsteroidsSizeRange.x, _config.AsteroidsSizeRange.y));
        }

        private void ScheduleNextSpawn()
        {
            if (_asteroidsFrequency <= 0f)
            {
                _nextSpawnTime = float.PositiveInfinity;
                return;
            }

            _nextSpawnTime = Time.time + 1f / _asteroidsFrequency;
        }
    }
}
