using PrimeTween;
using UnityEngine;

namespace MegaRaketa.Tweens
{
    public class Pulser : MonoBehaviour
    {
        [SerializeField, Min(0f)] private float _period = 1f;
        [SerializeField, Min(0f)] private float _scale = 1.2f;

        private Tween _pulseTween;
        private Vector3 _startScale;

        private void Awake()
        {
            _startScale = transform.localScale;
        }

        private void OnEnable()
        {
            StartPulse();
        }

        private void OnDisable()
        {
            StopPulse();
        }

        private void OnDestroy()
        {
            StopPulse();
        }

        private void StartPulse()
        {
            StopPulse();

            if (_period <= 0f)
            {
                return;
            }

            transform.localScale = _startScale;

            _pulseTween = Tween.Scale(
                transform,
                _startScale,
                _startScale * _scale,
                _period,
                cycles: -1,
                cycleMode: CycleMode.Yoyo);
        }

        public void StopPulse()
        {
            if (_pulseTween.isAlive)
            {
                _pulseTween.Stop();
            }
        }
    }
}
