using System.Collections.Generic;
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
        private readonly Dictionary<object, Vector3> _offsets = new Dictionary<object, Vector3>();
        private Vector3 _followPosition;
        private Vector3 _velocity;
        private bool _isLocked = true;

        private void Awake()
        {
            _camera = GetComponent<Camera>();
            _followPosition = transform.position;
        }

        private void LateUpdate()
        {
            if (_isLocked)
            {
                return;
            }

            Vector3 desiredPosition = GetDesiredPosition();
            Vector3 offset = GetTotalOffset();

            if (_smoothTime <= 0f)
            {
                _followPosition = desiredPosition;
                transform.position = _followPosition + offset;
                return;
            }

            _followPosition = Vector3.SmoothDamp(_followPosition, desiredPosition, ref _velocity, _smoothTime);
            transform.position = _followPosition + offset;
        }

        public void Unlock()
        {
            _isLocked = false;
            _followPosition = transform.position - GetTotalOffset();
        }

        public void SetOffset(object key, Vector3 offset)
        {
            if (key == null)
            {
                return;
            }

            _offsets[key] = offset;
        }

        public void RemoveOffset(object key)
        {
            if (key == null)
            {
                return;
            }

            _offsets.Remove(key);
        }

        private Vector3 GetDesiredPosition()
        {
            Vector3 rocketPosition = _rocket.Position;
            float rocketDepth = Vector3.Dot(rocketPosition - _followPosition, transform.forward);
            Vector3 viewportPoint = new Vector3(GetTargetViewportX(), _targetViewportY, rocketDepth);
            Vector3 currentWorldPoint = _camera.ViewportToWorldPoint(viewportPoint);
            currentWorldPoint += _followPosition - transform.position;

            return _followPosition + rocketPosition - currentWorldPoint;
        }

        private Vector3 GetTotalOffset()
        {
            Vector3 offset = Vector3.zero;

            foreach (Vector3 value in _offsets.Values)
            {
                offset += value;
            }

            return offset;
        }

        private float GetTargetViewportX()
        {
            float angle = -_rocket.DeviationAngle;
            float rangeProgress = Mathf.InverseLerp(_angleRange.x, _angleRange.y, angle);

            return Mathf.Lerp(_viewportXRange.x, _viewportXRange.y, rangeProgress);
        }
    }
}
