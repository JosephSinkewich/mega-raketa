using UnityEngine;

namespace MegaRaketa.Gameplay.Camera
{
    [CreateAssetMenu(fileName = "CameraShakeConfig", menuName = "MegaRaketa/Camera Shake Config")]
    public class CameraShakeConfig : ScriptableObject
    {
        [SerializeField, Min(0f)] private float _duration = 0.2f;
        [SerializeField] private Vector3 _strengthPerAsteroidSize = new Vector3(0.15f, 0.15f, 0f);
        [SerializeField, Min(1)] private int _vibrato = 10;
        [SerializeField, Range(0f, 180f)] private float _randomness = 90f;

        public float Duration => _duration;
        public Vector3 StrengthPerAsteroidSize => _strengthPerAsteroidSize;
        public int Vibrato => _vibrato;
        public float Randomness => _randomness;
    }
}
