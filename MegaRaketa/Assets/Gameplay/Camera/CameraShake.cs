using System.Collections.Generic;
using DG.Tweening;
using MegaRaketa.Gameplay.VisualObjects.Rocket;
using UnityEngine;
using Zenject;

namespace MegaRaketa.Gameplay.Camera
{
    public class CameraShake : IInitializable, System.IDisposable
    {
        [Inject] private CameraShakeConfig _config;
        [Inject] private IRocket _rocket;
        [Inject] private ICameraOperator _cameraOperator;

        private readonly List<Tween> _activeTweens = new List<Tween>();

        public void Initialize()
        {
            _rocket.OnAsteroidCollide += ShakeOnAsteroidCollide;
            _rocket.OnExplode += ShakeOnDestruction;
        }

        public void Dispose()
        {
            for (int i = _activeTweens.Count - 1; i >= 0; i--)
            {
                Tween tween = _activeTweens[i];
                _cameraOperator?.RemoveOffset(tween);
                tween.Kill();
            }

            _activeTweens.Clear();
            _rocket.OnAsteroidCollide -= ShakeOnAsteroidCollide;
            _rocket.OnExplode -= ShakeOnDestruction;
        }

        private void ShakeOnAsteroidCollide(RocketAsteroidCollisionEventData eventData)
        {
            Vector3 strength = _config.StrengthPerAsteroidSize * eventData.AsteroidSize;

            Tween tween = CreateShakeTween(
                    strength,
                    _config.Duration,
                    _config.Vibrato,
                    _config.Randomness)
                .SetUpdate(UpdateType.Late);
            _activeTweens.Add(tween);
        }

        private void ShakeOnDestruction()
        {
            Tween tween = CreateShakeTween(
                    _config.DestructionStrength,
                    _config.DestructionDuration,
                    _config.DestructionVibrato,
                    _config.DestructionRandomness)
                .SetUpdate(UpdateType.Late);
            _activeTweens.Add(tween);
        }

        private Tween CreateShakeTween(Vector3 strength, float duration, int vibrato, float randomness)
        {
            Sequence sequence = DOTween.Sequence();
            int steps = vibrato;
            float stepDuration = duration / steps;
            Vector3 offset = Vector3.zero;
            bool isDisposed = false;

            for (int i = 0; i < steps; i++)
            {
                sequence.Append(DOTween.To(
                    () => offset,
                    SetOffset,
                    GetRandomOffset(strength, randomness),
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

        private Vector3 GetRandomOffset(Vector3 strength, float randomness)
        {
            float angle = Random.Range(-randomness, randomness) * Mathf.Deg2Rad;
            float radius = Random.Range(0.4f, 1f);
            Vector2 randomOffset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;

            return new Vector3(
                randomOffset.x * strength.x,
                randomOffset.y * strength.y,
                Random.Range(-strength.z, strength.z));
        }
    }
}
