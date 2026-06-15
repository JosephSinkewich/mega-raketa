using System;
using UnityEngine;

namespace MegaRaketa.Gameplay.Asteroids
{
    [Serializable]
    public class AsteroidsSpawnSection
    {
        [SerializeField] private Vector2 _sectionLengthRange;
        [SerializeField] private Vector2 _asteroidsFrequencyRange;
        [SerializeField] private Vector2 _asteroidSpeedRange;
        [SerializeField] private Vector2 _asteroidRotationSpeedRange;
        [SerializeField] private Vector2 _asteroidSizeRange;

        public Vector2 SectionLengthRange => _sectionLengthRange;
        public Vector2 AsteroidsFrequencyRange => _asteroidsFrequencyRange;
        public Vector2 AsteroidSpeedRange => _asteroidSpeedRange;
        public Vector2 AsteroidRotationSpeedRange => _asteroidRotationSpeedRange;
        public Vector2 AsteroidSizeRange => _asteroidSizeRange;
    }
}
