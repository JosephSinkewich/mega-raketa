using System;
using System.Collections.Generic;
using MegaRaketa.Gameplay.VisualObjects.Asteroids;
using MegaRaketa.Gameplay.VisualObjects.Rocket;
using MegaRaketa.Gameplay.VisualObjects;
using UnityEngine;
using Zenject;

namespace MegaRaketa.Gameplay.Camera
{
    public class CameraOperator : ICameraOperator, ILateTickable, IInitializable, IDisposable
    {
        [Inject] private CameraOperatorView _view;
        [Inject] private CameraOperatorConfig _config;
        [Inject] private IRocket _rocket;
        [Inject] private ISceneVisualObjects _sceneVisualObjects;

        private readonly Dictionary<object, Vector3> _offsets = new Dictionary<object, Vector3>();
        private Vector3 _followPosition;
        private Vector3 _velocity;
        private float _orthographicSize;
        private float _zoomVelocity;
        private bool _isLocked = true;
        private bool _isExplosionFocus;
        private Vector3 _explosionFocusPosition;
        private float _explosionOrthographicSize;

        public void Initialize()
        {
            _followPosition = _view.transform.position;
            _orthographicSize = _config.MinOrthographicSize;
            _view.Camera.orthographicSize = _orthographicSize;
            _rocket.OnExplode += HandleRocketExplode;
        }

        public void Dispose()
        {
            _rocket.OnExplode -= HandleRocketExplode;
        }

        public void LateTick()
        {
            if (_isLocked)
            {
                return;
            }

            Vector3 desiredPosition = GetDesiredPosition();
            Vector3 offset = GetTotalOffset();

            if (_config.SmoothTime <= 0f)
            {
                _followPosition = desiredPosition;
                _view.transform.position = _followPosition + offset;
            }
            else
            {
                _followPosition = Vector3.SmoothDamp(_followPosition, desiredPosition, ref _velocity, _config.SmoothTime);
                _view.transform.position = _followPosition + offset;
            }

            UpdateZoom();
        }

        public void Unlock()
        {
            _isLocked = false;
            _followPosition = _view.transform.position - GetTotalOffset();
        }

        public void FocusOnExplosion(Vector3 position, float orthographicSize)
        {
            _isExplosionFocus = true;
            _explosionFocusPosition = position;
            _explosionOrthographicSize = orthographicSize;
            _isLocked = false;
            _followPosition = _view.transform.position - GetTotalOffset();
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
            Vector3 rocketPosition = _isExplosionFocus ? _explosionFocusPosition : _rocket.Position;
            float rocketDepth = Vector3.Dot(rocketPosition - _followPosition, _view.transform.forward);
            float targetViewportX = _isExplosionFocus ? 0.5f : GetTargetViewportX();
            Vector3 viewportPoint = new Vector3(targetViewportX, _config.TargetViewportY, rocketDepth);
            Vector3 currentWorldPoint = _view.Camera.ViewportToWorldPoint(viewportPoint);
            currentWorldPoint += _followPosition - _view.transform.position;

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
            float rangeProgress = Mathf.InverseLerp(_config.AngleRange.x, _config.AngleRange.y, angle);

            return Mathf.Lerp(_config.ViewportXRange.x, _config.ViewportXRange.y, rangeProgress);
        }

        private void UpdateZoom()
        {
            float desiredOrthographicSize = _isExplosionFocus
                ? _explosionOrthographicSize
                : GetDesiredOrthographicSize(CountVisibleAsteroids());

            if (_config.ZoomSmoothTime <= 0f)
            {
                _orthographicSize = desiredOrthographicSize;
            }
            else
            {
                _orthographicSize = Mathf.SmoothDamp(
                    _orthographicSize,
                    desiredOrthographicSize,
                    ref _zoomVelocity,
                    _config.ZoomSmoothTime);
            }

            _view.Camera.orthographicSize = _orthographicSize;
        }

        private void HandleRocketExplode()
        {
            Vector3 explosionPosition = _rocket.Position;
            FocusOnExplosion(explosionPosition, _config.ExplosionOrthographicSize);
        }

        private float GetDesiredOrthographicSize(int visibleAsteroidCount)
        {
            if (visibleAsteroidCount >= _config.TargetAsteroidCount)
            {
                return _config.MinOrthographicSize;
            }

            float maxOrthographicSize = Mathf.Max(_config.MinOrthographicSize, _config.MaxOrthographicSize);
            float zoomProgress = 1f - (float)visibleAsteroidCount / _config.TargetAsteroidCount;

            return Mathf.Lerp(_config.MinOrthographicSize, maxOrthographicSize, zoomProgress);
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
                if (asteroidTransform.GetComponent<AsteroidView>() == null)
                {
                    continue;
                }

                Vector3 viewportPoint = _view.Camera.WorldToViewportPoint(asteroidTransform.position);
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
