using UnityEngine;

namespace MegaRaketa.Gameplay.Asteroids
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class AsteroidsCleaner : MonoBehaviour
    {
        private CircleCollider2D _collider;

        private void Awake()
        {
            _collider = GetComponent<CircleCollider2D>();
            _collider.isTrigger = true;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            Asteroid asteroid = other.GetComponent<Asteroid>();

            if (asteroid == null)
            {
                return;
            }

            Destroy(asteroid.gameObject);
        }
    }
}
