using UnityEngine;

namespace MegaRaketa.Gameplay.Asteroids
{
    [CreateAssetMenu(fileName = "AsteroidsSpawnerConfig", menuName = "MegaRaketa/Asteroids Spawner Config")]
    public class AsteroidsSpawnerConfig : ScriptableObject
    {
        [SerializeField] private Vector2 _sectionLengthRange;
        [SerializeField] private Vector2 _asteroidsFrequencyRange;
        [SerializeField] private Vector2 _asteroidsSpeedRange;
        [SerializeField] private Vector2 _asteroidsRotationSpeedRange;
        [SerializeField] private Vector2 _asteroidsSizeRange;
        [SerializeField, Min(0f)] private float _spawnRadius;

        public Vector2 SectionLengthRange => _sectionLengthRange;
        public Vector2 AsteroidsFrequencyRange => _asteroidsFrequencyRange;
        public Vector2 AsteroidsSpeedRange => _asteroidsSpeedRange;
        public Vector2 AsteroidsRotationSpeedRange => _asteroidsRotationSpeedRange;
        public Vector2 AsteroidsSizeRange => _asteroidsSizeRange;
        public float SpawnRadius => _spawnRadius;
    }
}
