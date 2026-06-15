using UnityEngine;

namespace MegaRaketa.Gameplay.VisualObjects.Asteroids
{
    [CreateAssetMenu(fileName = "AsteroidsSpawnerConfig", menuName = "MegaRaketa/Asteroids Spawner Config")]
    public class AsteroidsSpawnerConfig : ScriptableObject
    {
        [SerializeField] private AsteroidView _asteroidPrefab;
        [SerializeField, Min(0f)] private float _spawnRadius;
        [SerializeField] private AsteroidsSpawnScenario _startScenario;
        [SerializeField] private AsteroidsSpawnScenario _loopedScenario;

        public AsteroidView AsteroidPrefab => _asteroidPrefab;
        public float SpawnRadius => _spawnRadius;
        public AsteroidsSpawnScenario StartScenario => _startScenario;
        public AsteroidsSpawnScenario LoopedScenario => _loopedScenario;
    }
}
