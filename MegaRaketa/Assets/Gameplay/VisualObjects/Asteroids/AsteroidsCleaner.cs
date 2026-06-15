using MegaRaketa.Gameplay.Rocket;
using UnityEngine;
using Zenject;

namespace MegaRaketa.Gameplay.Asteroids
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class AsteroidsCleaner : MonoBehaviour
    {
        [Inject] private IRocket _rocket;

        private CircleCollider2D _collider;
        private bool _isLocked;

        private void Awake()
        {
            _collider = GetComponent<CircleCollider2D>();
            _collider.isTrigger = true;
        }

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

        private void HandleRocketExplode()
        {
            _isLocked = true;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (_isLocked)
            {
                return;
            }

            Asteroid asteroid = other.GetComponent<Asteroid>();

            if (asteroid == null)
            {
                return;
            }

            Destroy(asteroid.gameObject);
        }
    }
}
