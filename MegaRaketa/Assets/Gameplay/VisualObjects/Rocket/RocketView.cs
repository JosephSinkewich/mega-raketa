using System;
using UnityEngine;

namespace MegaRaketa.Gameplay.VisualObjects.Rocket
{
    [RequireComponent(typeof(Collider2D))]
    public class RocketView : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _engineFire;

        public ParticleSystem EngineFire => _engineFire;
        public Collider2D Collider { get; private set; }

        public event Action<Collider2D> TriggerEntered;

        private void Awake()
        {
            Collider = GetComponent<Collider2D>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            TriggerEntered?.Invoke(other);
        }

        public void DestroyObject()
        {
            Destroy(gameObject);
        }
    }
}
