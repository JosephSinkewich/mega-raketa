using System.Collections.Generic;
using MegaRaketa.Gameplay.Asteroids;
using MegaRaketa.Gameplay.Rocket;
using MegaRaketa.Gameplay.VisualObjects;
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
        [SerializeField, Min(0f)] private float _minOrthographicSize = 5f;
        [SerializeField, Min(0f)] private float _maxOrthographicSize = 10f;
        [SerializeField, Min(0)] private int _targetAsteroidCount = 5;
        [SerializeField, Min(0f)] private float _zoomSmoothTime = 0.5f;

        [Inject] private IRocket _rocket;
        [Inject] private ISceneVisualObjects _sceneVisualObjects;

        private Camera _camera;
        private readonly Dictionary<object, Vector3> _offsets = new Dictionary<object, Vector3>();
        private Vector3 _followPosition;
        private Vector3 _velocity;
        private float _orthographicSize;
        private float _zoomVelocity;
        private bool _isLocked = true;

        private void Awake()
        {
            _camera = GetComponent<Camera>();
            _followPosition = transform.position;
            _orthographicSize = _minOrthographicSize;
            _camera.orthographicSize = _orthographicSize;
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
            }
            else
            {
                _followPosition = Vector3.SmoothDamp(_followPosition, desiredPosition, ref _velocity, _smoothTime);
                transform.position = _followPosition + offset;
            }

            UpdateZoom();
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

        private void UpdateZoom()
        {
            int visibleAsteroidCount = CountVisibleAsteroids();
            float desiredOrthographicSize = GetDesiredOrthographicSize(visibleAsteroidCount);

            if (_zoomSmoothTime <= 0f)
            {
                _orthographicSize = desiredOrthographicSize;
            }
            else
            {
                _orthographicSize = Mathf.SmoothDamp(
                    _orthographicSize,
                    desiredOrthographicSize,
                    ref _zoomVelocity,
                    _zoomSmoothTime);
            }

            _camera.orthographicSize = _orthographicSize;
        }

        private float GetDesiredOrthographicSize(int visibleAsteroidCount)
        {
            if (visibleAsteroidCount >= _targetAsteroidCount)
            {
                return _minOrthographicSize;
            }

            float maxOrthographicSize = Mathf.Max(_minOrthographicSize, _maxOrthographicSize);
            float zoomProgress = 1f - (float)visibleAsteroidCount / _targetAsteroidCount;

            return Mathf.Lerp(_minOrthographicSize, maxOrthographicSize, zoomProgress);
        }

        private int CountVisibleAsteroids()
        {
            Transform asteroidsContainer = _sceneVisualObjects.AsteroidsContainer;
            if (asteroidsContainer == null)
            {
                return 0;
            }

            int count = 0;

            for (int i = 0; i < asteroidsContainer.childCount; i++)
            {
                Transform asteroidTransform = asteroidsContainer.GetChild(i);
                if (asteroidTransform.GetComponent<Asteroid>() == null)
                {
                    continue;
                }

                Vector3 viewportPoint = _camera.WorldToViewportPoint(asteroidTransform.position);
                if (viewportPoint.z < 0f)
                {
                    continue;
                }

                if (viewportPoint.x < 0f || viewportPoint.x > 1f)
                {
                    continue;
                }

                if (viewportPoint.y < 0f || viewportPoint.y > 1f)
                {
                    continue;
                }

                count++;
            }

            return count;
        }
    }
}
