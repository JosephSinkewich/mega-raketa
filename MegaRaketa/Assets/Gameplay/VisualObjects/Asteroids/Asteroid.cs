using MegaRaketa.Gameplay.VisualObjects;
using UnityEngine;
using Zenject;

namespace MegaRaketa.Gameplay.Asteroids
{
    public class Asteroid : MonoBehaviour
    {
        [SerializeField] private GameObject _explosionEffect;

        [Inject] private ISceneVisualObjects _sceneVisualObjects;
        [Inject] private IInstantiator _instantiator;

        private float _speed;
        private float _rotationSpeed;
        private float _size;
        private Vector2 _direction;

        public float Size => _size;

        public void Initialize(float speed, float rotationSpeed, float size)
        {
            _speed = speed;
            _rotationSpeed = rotationSpeed;
            _size = size;
            _direction = Random.insideUnitCircle.normalized;

            if (_direction == Vector2.zero)
            {
                _direction = Vector2.down;
            }

            transform.localScale = Vector3.one * size;
        }

        public void Explode()
        {
            GameObject explosionEffect = _instantiator.InstantiatePrefab(
                _explosionEffect,
                transform.position,
                Quaternion.identity,
                _sceneVisualObjects.AsteroidsContainer);
            explosionEffect.transform.localScale = transform.localScale;
            Destroy(gameObject);
        }

        private void Update()
        {
            Vector3 direction = new Vector3(_direction.x, _direction.y, 0f);
            transform.position += direction * (_speed * Time.deltaTime);
            transform.Rotate(Vector3.forward, _rotationSpeed * Time.deltaTime);
        }
    }
}
