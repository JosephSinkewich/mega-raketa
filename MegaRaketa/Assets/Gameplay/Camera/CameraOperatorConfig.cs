using UnityEngine;

namespace MegaRaketa.Gameplay.Camera
{
    [CreateAssetMenu(fileName = "CameraOperatorConfig", menuName = "MegaRaketa/Camera Operator Config")]
    public class CameraOperatorConfig : ScriptableObject
    {
        [SerializeField, Range(0f, 1f)] private float _targetViewportY;
        [SerializeField] private Vector2 _viewportXRange = new Vector2(0.1f, 0.9f);
        [SerializeField] private Vector2 _angleRange = new Vector2(45f, -45f);
        [SerializeField, Min(0f)] private float _smoothTime = 0.2f;
        [SerializeField, Min(0f)] private float _minOrthographicSize = 5f;
        [SerializeField, Min(0f)] private float _maxOrthographicSize = 10f;
        [SerializeField, Min(0)] private int _targetAsteroidCount = 5;
        [SerializeField, Min(0f)] private float _zoomSmoothTime = 0.5f;
        [SerializeField, Min(0f)] private float _explosionOrthographicSize = 3f;

        public float TargetViewportY => _targetViewportY;
        public Vector2 ViewportXRange => _viewportXRange;
        public Vector2 AngleRange => _angleRange;
        public float SmoothTime => _smoothTime;
        public float MinOrthographicSize => _minOrthographicSize;
        public float MaxOrthographicSize => _maxOrthographicSize;
        public int TargetAsteroidCount => _targetAsteroidCount;
        public float ZoomSmoothTime => _zoomSmoothTime;
        public float ExplosionOrthographicSize => _explosionOrthographicSize;
    }
}
