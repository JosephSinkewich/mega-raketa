using UnityEngine;

namespace MegaRaketa.Gameplay.Asteroids
{
    [CreateAssetMenu(fileName = "AsteroidConfig", menuName = "MegaRaketa/Asteroid Config")]
    public class AsteroidConfig : ScriptableObject
    {
        [SerializeField] private GameObject _explosionEffect;

        public GameObject ExplosionEffect => _explosionEffect;
    }
}
