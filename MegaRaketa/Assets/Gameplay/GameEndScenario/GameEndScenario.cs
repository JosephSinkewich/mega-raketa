using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using MegaRaketa.Gameplay.Rocket;
using MegaRaketa.Gameplay.ScoreCounter;
using MegaRaketa.Gameplay.WindowsContainer;
using Zenject;

namespace MegaRaketa.Gameplay.GameEndScenario
{
    public class GameEndScenario : IInitializable, IDisposable
    {
        [Inject] private GameEndScenarioConfig _config;
        [Inject] private IRocket _rocket;
        [Inject] private IScoreCounter _scoreCounter;
        [Inject] private IWindowsContainer _windowsContainer;

        private CancellationTokenSource _cancellationTokenSource;

        public void Initialize()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _rocket.OnExplode += HandleRocketExplode;
        }

        public void Dispose()
        {
            _rocket.OnExplode -= HandleRocketExplode;
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
        }

        private void HandleRocketExplode()
        {
            ShowCongratulationsWindowWithDelayAsync().Forget();
        }

        private async UniTask ShowCongratulationsWindowWithDelayAsync()
        {
            if (_config.WindowShowDelay > 0f)
            {
                await UniTask.WaitForSeconds(_config.WindowShowDelay, cancellationToken: _cancellationTokenSource.Token);
            }

            _windowsContainer.ShowCongratulationsWindow(_scoreCounter.Score);
        }
    }
}
