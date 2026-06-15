using UnityEngine;

namespace MegaRaketa.Gameplay.Rocket
{
    [CreateAssetMenu(fileName = "RocketConfig", menuName = "MegaRaketa/Rocket Config")]
    public class RocketConfig : ScriptableObject
    {
        [SerializeField, Min(0f)] private float _acceleration;
        [SerializeField, Min(0f)] private float _maxSpeed;
        [SerializeField, Min(0f)] private float _rotationSpeed;
        [SerializeField, Min(0f)] private float _maxDeviationAngle;
        [SerializeField] private GameObject _explosionEffect;

        public float Acceleration => _acceleration;
        public float MaxSpeed => _maxSpeed;
        public float RotationSpeed => _rotationSpeed;
        public float MaxDeviationAngle => _maxDeviationAngle;
        public GameObject ExplosionEffect => _explosionEffect;
    }
}
