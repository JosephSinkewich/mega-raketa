using UnityEngine;

namespace MegaRaketa.Gameplay.Asteroids
{
    [CreateAssetMenu(fileName = "AsteroidsSpawnerConfig", menuName = "MegaRaketa/Asteroids Spawner Config")]
    public class AsteroidsSpawnerConfig : ScriptableObject
    {
        [SerializeField, Min(0f)] private float _spawnRadius;
        [SerializeField] private AsteroidsSpawnScenario _startScenario;
        [SerializeField] private AsteroidsSpawnScenario _loopedScenario;

        public float SpawnRadius => _spawnRadius;
        public AsteroidsSpawnScenario StartScenario => _startScenario;
        public AsteroidsSpawnScenario LoopedScenario => _loopedScenario;
    }
}
