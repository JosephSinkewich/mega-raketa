using MegaRaketa.Gameplay.Asteroids;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace MegaRaketa.Gameplay.Rocket
{
    [RequireComponent(typeof(Collider2D))]
    public class Rocket : MonoBehaviour, IRocket
    {
        [SerializeField, Min(0f)] private float _acceleration;
        [SerializeField, Min(0f)] private float _maxSpeed;
        [SerializeField, Min(0f)] private float _rotationSpeed;
        [SerializeField, Min(0f)] private float _maxDeviationAngle;

        private float _speed;
        private float _deviationAngle;
        private bool _isLaunched;
        private Collider2D _rocketCollider;
        private Quaternion _startRotation;

        public Vector3 Position => transform.position;
        public float DeviationAngle => _deviationAngle;

        private void Awake()
        {
            _rocketCollider = GetComponent<Collider2D>();
            _startRotation = transform.rotation;
        }

        private void Update()
        {
            Vector3 direction = transform.rotation * Vector3.up;
            transform.position += direction * (_speed * Time.deltaTime);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            DestroyAsteroid(other);
        }

        public void Launch()
        {
            if (_isLaunched)
            {
                return;
            }

            _isLaunched = true;
            AccelerateAsync().Forget();
        }

        public void RotateTo(Vector3 targetPoint)
        {
            Vector3 directionToTarget = targetPoint - transform.position;

            if (directionToTarget == Vector3.zero)
            {
                return;
            }

            Vector3 startDirection = _startRotation * Vector3.up;
            float targetDeviationAngle = Vector3.SignedAngle(startDirection, directionToTarget, Vector3.forward);

            targetDeviationAngle = Mathf.Clamp(targetDeviationAngle, -_maxDeviationAngle, _maxDeviationAngle);
            _deviationAngle = Mathf.MoveTowards(_deviationAngle, targetDeviationAngle, _rotationSpeed * Time.deltaTime);

            transform.rotation = Quaternion.AngleAxis(_deviationAngle, Vector3.forward) * _startRotation;
        }

        private async UniTask AccelerateAsync()
        {
            if (_acceleration <= 0f)
            {
                return;
            }

            while (_speed < _maxSpeed)
            {
                _speed = Mathf.MoveTowards(_speed, _maxSpeed, _acceleration * Time.deltaTime);
                await UniTask.Yield(cancellationToken: destroyCancellationToken);
            }
        }

        private void DestroyAsteroid(Collider2D other)
        {
            if (_rocketCollider == null || !_rocketCollider.IsTouching(other))
            {
                return;
            }

            Asteroid asteroid = other.GetComponentInParent<Asteroid>();

            if (asteroid == null)
            {
                return;
            }

            Destroy(asteroid.gameObject);
        }
    }
}
