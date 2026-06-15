using System.Collections.Generic;
using DG.Tweening;
using MegaRaketa.Gameplay.Rocket;
using UnityEngine;
using Zenject;

namespace MegaRaketa.Gameplay.CameraOperator
{
    public class CameraShake : MonoBehaviour
    {
        [SerializeField, Min(0f)] private float _duration = 0.2f;
        [SerializeField] private Vector3 _strengthPerAsteroidSize = new Vector3(0.15f, 0.15f, 0f);
        [SerializeField, Min(1)] private int _vibrato = 10;
        [SerializeField, Range(0f, 180f)] private float _randomness = 90f;

        [Inject] private IRocket _rocket;
        [Inject] private ICameraOperator _cameraOperator;

        private readonly List<Tween> _activeTweens = new List<Tween>();

        private void Start()
        {
            _rocket.OnAsteroidCollide += Shake;
        }

        private void OnDestroy()
        {
            for (int i = _activeTweens.Count - 1; i >= 0; i--)
            {
                Tween tween = _activeTweens[i];
                _cameraOperator?.RemoveOffset(tween);
                tween.Kill();
            }

            _activeTweens.Clear();

            if (_rocket != null)
            {
                _rocket.OnAsteroidCollide -= Shake;
            }
        }

        private void Shake(RocketAsteroidCollisionEventData eventData)
        {
            Vector3 strength = _strengthPerAsteroidSize * eventData.AsteroidSize;

            Tween tween = CreateShakeTween(strength)
                .SetUpdate(UpdateType.Late);
            _activeTweens.Add(tween);
        }

        private Tween CreateShakeTween(Vector3 strength)
        {
            Sequence sequence = DOTween.Sequence();
            int steps = _vibrato;
            float stepDuration = _duration / steps;
            Vector3 offset = Vector3.zero;
            bool isDisposed = false;

            for (int i = 0; i < steps; i++)
            {
                sequence.Append(DOTween.To(
                    () => offset,
                    SetOffset,
                    GetRandomOffset(strength),
                    stepDuration));
            }

            sequence.Append(DOTween.To(
                () => offset,
                SetOffset,
                Vector3.zero,
                stepDuration));
            sequence.OnComplete(Dispose);
            sequence.OnKill(Dispose);

            return sequence;

            void SetOffset(Vector3 value)
            {
                if (isDisposed)
                {
                    return;
                }

                offset = value;
                _cameraOperator?.SetOffset(sequence, offset);
            }

            void Dispose()
            {
                if (isDisposed)
                {
                    return;
                }

                isDisposed = true;
                _cameraOperator?.RemoveOffset(sequence);
                _activeTweens.Remove(sequence);
            }
        }

        private Vector3 GetRandomOffset(Vector3 strength)
        {
            float angle = Random.Range(-_randomness, _randomness) * Mathf.Deg2Rad;
            float radius = Random.Range(0.4f, 1f);
            Vector2 randomOffset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;

            return new Vector3(
                randomOffset.x * strength.x,
                randomOffset.y * strength.y,
                Random.Range(-strength.z, strength.z));
        }

    }
}
