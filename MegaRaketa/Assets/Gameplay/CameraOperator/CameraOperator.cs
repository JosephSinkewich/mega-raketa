using MegaRaketa.Gameplay.Rocket;
using UnityEngine;
using Zenject;

namespace MegaRaketa.Gameplay.CameraOperator
{
    [RequireComponent(typeof(Camera))]
    public class CameraOperator : MonoBehaviour, ICameraOperator
    {
        [SerializeField, Range(0f, 1f)] private float _targetViewportY = -0.0f;
        [SerializeField] private Vector2 _viewportXRange = new Vector2(0.1f, 0.9f);
        [SerializeField] private Vector2 _angleRange = new Vector2(45f, -45f);
        [SerializeField, Min(0f)] private float _smoothTime = 0.2f;

        [Inject] private IRocket _rocket;

        private Camera _camera;
        private Vector3 _velocity;
        private bool _isLocked = true;

        private void Awake()
        {
            _camera = GetComponent<Camera>();
        }

        private void LateUpdate()
        {
            if (_isLocked)
            {
                return;
            }

            Vector3 desiredPosition = GetDesiredPosition();

            if (_smoothTime <= 0f)
            {
                transform.position = desiredPosition;
                return;
            }

            transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref _velocity, _smoothTime);
        }

        public void Unlock()
        {
            _isLocked = false;
        }

        private Vector3 GetDesiredPosition()
        {
            Vector3 rocketPosition = _rocket.Position;
            float rocketDepth = Vector3.Dot(rocketPosition - transform.position, transform.forward);
            Vector3 viewportPoint = new Vector3(GetTargetViewportX(), _targetViewportY, rocketDepth);
            Vector3 currentWorldPoint = _camera.ViewportToWorldPoint(viewportPoint);

            return transform.position + rocketPosition - currentWorldPoint;
        }

        private float GetTargetViewportX()
        {
            float angle = -_rocket.DeviationAngle;
            float rangeProgress = Mathf.InverseLerp(_angleRange.x, _angleRange.y, angle);

            return Mathf.Lerp(_viewportXRange.x, _viewportXRange.y, rangeProgress);
        }
    }
}
