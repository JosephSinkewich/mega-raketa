using Cysharp.Threading.Tasks;
using MegaRaketa.Gameplay.Asteroids;
using MegaRaketa.Gameplay.CameraOperator;
using MegaRaketa.Gameplay.Rocket;
using MegaRaketa.Gameplay.Rocket.RocketControl;
using MegaRaketa.Gameplay.SelfDestructionButton;
using MegaRaketa.Tweens;
using PrimeTween;
using UnityEngine;
using Zenject;

namespace MegaRaketa.Gameplay.StartScenario
{
    public class StartScenario : MonoBehaviour
    {
        [SerializeField] private GameObject _tapObject;
        [SerializeField, Min(0f)] private float _tapObjectDestroyPeriod = 0.25f;
        [SerializeField, Min(0f)] private float _rocketControlUnlockDelay;
        [SerializeField, Min(0f)] private float _cameraOperatorUnlockDelay;
        [SerializeField, Min(0f)] private float _asteroidsSpawnerUnlockDelay;
        [SerializeField, Min(0f)] private float _selfDestructionButtonUnlockDelay;

        [Inject] private IRocket _rocket;
        [Inject] private IRocketControl _rocketControl;
        [Inject] private ICameraOperator _cameraOperator;
        [Inject] private IAsteroidsSpawner _asteroidsSpawner;
        [Inject] private ISelfDestructionButton _selfDestructionButton;

        private bool _isLaunched;

        private void Update()
        {
            if (_isLaunched || !IsTapStarted())
            {
                return;
            }

            _isLaunched = true;
            _rocket.Launch();
            DestroyTapObject();
            UnlockRocketControlWithDelayAsync().Forget();
            UnlockCameraOperatorWithDelayAsync().Forget();
            UnlockAsteroidsSpawnerWithDelayAsync().Forget();
            UnlockSelfDestructionButtonWithDelayAsync().Forget();
        }

        private bool IsTapStarted()
        {
            if (Input.GetMouseButtonDown(0))
            {
                return true;
            }

            return Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began;
        }

        private void DestroyTapObject()
        {
            if (_tapObject == null)
            {
                return;
            }

            Transform tapObjectTransform = _tapObject.transform;
            Pulser pulser = _tapObject.GetComponent<Pulser>();

            if (pulser != null)
            {
                pulser.StopPulse();
                pulser.enabled = false;
            }

            Tween.Scale(tapObjectTransform, Vector3.zero, _tapObjectDestroyPeriod)
                .OnComplete(_tapObject, Destroy);
        }

        private async UniTask UnlockRocketControlWithDelayAsync()
        {
            if (_rocketControl == null)
            {
                return;
            }

            if (_rocketControlUnlockDelay > 0f)
            {
                await UniTask.WaitForSeconds(_rocketControlUnlockDelay, cancellationToken: destroyCancellationToken);
            }

            _rocketControl.Unlock();
        }

        private async UniTask UnlockCameraOperatorWithDelayAsync()
        {
            if (_cameraOperator == null)
            {
                return;
            }

            if (_cameraOperatorUnlockDelay > 0f)
            {
                await UniTask.WaitForSeconds(_cameraOperatorUnlockDelay, cancellationToken: destroyCancellationToken);
            }

            _cameraOperator.Unlock();
        }

        private async UniTask UnlockAsteroidsSpawnerWithDelayAsync()
        {
            if (_asteroidsSpawner == null)
            {
                return;
            }

            if (_asteroidsSpawnerUnlockDelay > 0f)
            {
                await UniTask.WaitForSeconds(_asteroidsSpawnerUnlockDelay, cancellationToken: destroyCancellationToken);
            }

            _asteroidsSpawner.Unlock();
        }

        private async UniTask UnlockSelfDestructionButtonWithDelayAsync()
        {
            if (_selfDestructionButton == null)
            {
                return;
            }

            if (_selfDestructionButtonUnlockDelay > 0f)
            {
                await UniTask.WaitForSeconds(_selfDestructionButtonUnlockDelay, cancellationToken: destroyCancellationToken);
            }

            _selfDestructionButton.Unlock();
        }
    }
}
