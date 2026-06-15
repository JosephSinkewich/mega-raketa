using Cysharp.Threading.Tasks;
using MegaRaketa.Gameplay.Rocket;
using MegaRaketa.Gameplay.ScoreCounter;
using MegaRaketa.Gameplay.WindowsContainer;
using UnityEngine;
using Zenject;

namespace MegaRaketa.Gameplay.GameEndScenario
{
    public class GameEndScenario : MonoBehaviour
    {
        [SerializeField, Min(0f)] private float _windowShowDelay = 2f;

        [Inject] private IRocket _rocket;
        [Inject] private IScoreCounter _scoreCounter;
        [Inject] private IWindowsContainer _windowsContainer;

        private void Start()
        {
            _rocket.OnExplode += HandleRocketExplode;
        }

        private void OnDestroy()
        {
            if (_rocket != null)
            {
                _rocket.OnExplode -= HandleRocketExplode;
            }
        }

        private void HandleRocketExplode()
        {
            ShowCongratulationsWindowWithDelayAsync().Forget();
        }

        private async UniTask ShowCongratulationsWindowWithDelayAsync()
        {
            if (_windowShowDelay > 0f)
            {
                await UniTask.WaitForSeconds(_windowShowDelay, cancellationToken: destroyCancellationToken);
            }

            _windowsContainer.ShowCongratulationsWindow(_scoreCounter.Score);
        }
    }
}
