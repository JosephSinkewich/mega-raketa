using System.Threading;
using Cysharp.Threading.Tasks;
using MegaRaketa.Gameplay.Asteroids;
using MegaRaketa.Gameplay.CameraOperator;
using MegaRaketa.Gameplay.Rocket;
using MegaRaketa.Gameplay.Rocket.RocketControl;
using MegaRaketa.Gameplay.SelfDestructionButton;
using MegaRaketa.SceneContext;
using MegaRaketa.Tweens;
using PrimeTween;
using UnityEngine;
using Zenject;

namespace MegaRaketa.Gameplay.StartScenario
{
    public class StartScenario : ITickable, IInitializable, System.IDisposable
    {
        [Inject] private StartScenarioConfig _config;
        [Inject(Id = BindingIds.TapToStart)] private GameObject _tapObject;
        [Inject] private IRocket _rocket;
        [Inject] private IRocketControl _rocketControl;
        [Inject] private ICameraOperator _cameraOperator;
        [Inject] private IAsteroidsSpawner _asteroidsSpawner;
        [Inject] private ISelfDestructionButton _selfDestructionButton;

        private bool _isLaunched;
        private CancellationTokenSource _cancellationTokenSource;

        public void Initialize()
        {
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public void Dispose()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
        }

        public void Tick()
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

            Tween.Scale(tapObjectTransform, Vector3.zero, _config.TapObjectDestroyPeriod)
                .OnComplete(_tapObject, UnityEngine.Object.Destroy);
        }

        private async UniTask UnlockRocketControlWithDelayAsync()
        {
            if (_rocketControl == null)
            {
                return;
            }

            if (_config.RocketControlUnlockDelay > 0f)
            {
                await UniTask.WaitForSeconds(_config.RocketControlUnlockDelay, cancellationToken: _cancellationTokenSource.Token);
            }

            _rocketControl.Unlock();
        }

        private async UniTask UnlockCameraOperatorWithDelayAsync()
        {
            if (_cameraOperator == null)
            {
                return;
            }

            if (_config.CameraOperatorUnlockDelay > 0f)
            {
                await UniTask.WaitForSeconds(_config.CameraOperatorUnlockDelay, cancellationToken: _cancellationTokenSource.Token);
            }

            _cameraOperator.Unlock();
        }

        private async UniTask UnlockAsteroidsSpawnerWithDelayAsync()
        {
            if (_asteroidsSpawner == null)
            {
                return;
            }

            if (_config.AsteroidsSpawnerUnlockDelay > 0f)
            {
                await UniTask.WaitForSeconds(_config.AsteroidsSpawnerUnlockDelay, cancellationToken: _cancellationTokenSource.Token);
            }

            _asteroidsSpawner.Unlock();
        }

        private async UniTask UnlockSelfDestructionButtonWithDelayAsync()
        {
            if (_selfDestructionButton == null)
            {
                return;
            }

            if (_config.SelfDestructionButtonUnlockDelay > 0f)
            {
                await UniTask.WaitForSeconds(_config.SelfDestructionButtonUnlockDelay, cancellationToken: _cancellationTokenSource.Token);
            }

            _selfDestructionButton.Unlock();
        }
    }
}
