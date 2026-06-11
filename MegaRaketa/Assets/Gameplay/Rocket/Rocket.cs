using Cysharp.Threading.Tasks;
using UnityEngine;

namespace MegaRaketa.Gameplay.Rocket
{
    public class Rocket : MonoBehaviour, IRocket
    {
        [SerializeField, Min(0f)] private float _acceleration;
        [SerializeField, Min(0f)] private float _maxSpeed;

        private float _speed;
        private bool _isLaunched;

        private void Update()
        {
            Vector3 direction = transform.rotation * Vector3.up;
            transform.position += direction * (_speed * Time.deltaTime);
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
    }
}
