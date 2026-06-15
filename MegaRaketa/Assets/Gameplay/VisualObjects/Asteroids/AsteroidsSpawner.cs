using MegaRaketa.Gameplay.Rocket;
using MegaRaketa.Gameplay.VisualObjects;
using UnityEngine;
using Zenject;

namespace MegaRaketa.Gameplay.Asteroids
{
    public class AsteroidsSpawner : MonoBehaviour, IAsteroidsSpawner
    {
        [SerializeField] private Asteroid _asteroidPrefab;

        [Inject] private AsteroidsSpawnerConfig _config;
        [Inject] private IRocket _rocket;
        [Inject] private ISceneVisualObjects _sceneVisualObjects;
        [Inject] private IInstantiator _instantiator;

        private float _sectionStartY;
        private float _sectionEndY;
        private float _asteroidsFrequency;
        private float _nextSpawnTime;
        private int _startScenarioSectionIndex;
        private int _loopedScenarioSectionIndex;
        private bool _isStartScenarioCompleted;
        private AsteroidsSpawnSection _activeSection;
        private bool _isLocked = true;

        private void Start()
        {
            _rocket.OnExplode += HandleRocketExplode;
        }

        private void OnDestroy()
        {
            if (_rocket != null)
            {
                _rocket.OnExplode -= HandleRocketExplode;
            }
        }

        private void Update()
        {
            if (_isLocked || _asteroidPrefab == null || _activeSection == null)
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
            _startScenarioSectionIndex = 0;
            _loopedScenarioSectionIndex = 0;
            _isStartScenarioCompleted = false;
            InitializeSection();
            ScheduleNextSpawn();
        }

        private void HandleRocketExplode()
        {
            _isLocked = true;
        }

        private void InitializeSection()
        {
            AsteroidsSpawnSection section = GetNextSection();

            if (section == null)
            {
                _activeSection = null;
                return;
            }

            ApplySection(section);
        }

        private AsteroidsSpawnSection GetNextSection()
        {
            if (!_isStartScenarioCompleted)
            {
                AsteroidsSpawnScenario startScenario = _config.StartScenario;

                if (startScenario != null && _startScenarioSectionIndex < startScenario.SectionCount)
                {
                    return startScenario.GetSection(_startScenarioSectionIndex++);
                }

                _isStartScenarioCompleted = true;
                _loopedScenarioSectionIndex = 0;
            }

            AsteroidsSpawnScenario loopedScenario = _config.LoopedScenario;

            if (loopedScenario == null || loopedScenario.SectionCount == 0)
            {
                return null;
            }

            AsteroidsSpawnSection section = loopedScenario.GetSection(_loopedScenarioSectionIndex);
            _loopedScenarioSectionIndex = (_loopedScenarioSectionIndex + 1) % loopedScenario.SectionCount;

            return section;
        }

        private void ApplySection(AsteroidsSpawnSection section)
        {
            _activeSection = section;
            _sectionStartY = _rocket.Position.y;
            _sectionEndY = _sectionStartY + Random.Range(section.SectionLengthRange.x, section.SectionLengthRange.y);
            _asteroidsFrequency = Random.Range(section.AsteroidsFrequencyRange.x, section.AsteroidsFrequencyRange.y);
        }

        private void SpawnAsteroid()
        {
            Vector2 spawnDirection = Random.insideUnitCircle.normalized;

            if (spawnDirection == Vector2.zero)
            {
                spawnDirection = Vector2.up;
            }

            Vector3 position = _rocket.Position + new Vector3(
                spawnDirection.x,
                spawnDirection.y,
                0f) * _config.SpawnRadius;

            float speed = Random.Range(_activeSection.AsteroidSpeedRange.x, _activeSection.AsteroidSpeedRange.y);
            float rotationSpeed = Random.Range(_activeSection.AsteroidRotationSpeedRange.x, _activeSection.AsteroidRotationSpeedRange.y);
            float size = Random.Range(_activeSection.AsteroidSizeRange.x, _activeSection.AsteroidSizeRange.y);

            Asteroid asteroid = _instantiator.InstantiatePrefabForComponent<Asteroid>(
                _asteroidPrefab,
                position,
                Quaternion.identity,
                _sceneVisualObjects.AsteroidsContainer);
            asteroid.Initialize(speed, rotationSpeed, size);
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
