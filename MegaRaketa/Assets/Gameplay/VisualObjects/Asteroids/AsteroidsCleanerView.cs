using System;
using UnityEngine;

namespace MegaRaketa.Gameplay.VisualObjects.Asteroids
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class AsteroidsCleanerView : MonoBehaviour
    {
        public event Action<Collider2D> TriggerExited;

        private void Awake()
        {
            CircleCollider2D collider = GetComponent<CircleCollider2D>();
            collider.isTrigger = true;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            TriggerExited?.Invoke(other);
        }
    }
}
