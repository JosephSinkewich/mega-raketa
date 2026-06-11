using UnityEngine;

namespace MegaRaketa.Gameplay.Asteroids
{
    public class Asteroid : MonoBehaviour
    {
        private float _speed;
        private float _rotationSpeed;
        private Vector2 _direction;

        private void Update()
        {
            Vector3 direction = new Vector3(_direction.x, _direction.y, 0f);
            transform.position += direction * (_speed * Time.deltaTime);
            transform.Rotate(Vector3.forward, _rotationSpeed * Time.deltaTime);
        }

        public void Initialize(float speed, float rotationSpeed, float size)
        {
            _speed = speed;
            _rotationSpeed = rotationSpeed;
            _direction = Random.insideUnitCircle.normalized;

            if (_direction == Vector2.zero)
            {
                _direction = Vector2.down;
            }

            transform.localScale = Vector3.one * size;
        }
    }
}
