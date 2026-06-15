using System;
using MegaRaketa.Gameplay.Rocket;
using Zenject;

namespace MegaRaketa.Gameplay.ScoreCounter
{
    public class ScoreCounter : IScoreCounter, IInitializable, IDisposable
    {
        [Inject] private ScoreCounterView _view;
        [Inject] private ScoreCounterConfig _config;
        [Inject] private IRocket _rocket;

        private int _score;

        public int Score => _score;

        public void Initialize()
        {
            _rocket.OnAsteroidCollide += IncreaseScore;
            UpdateText();
        }

        public void Dispose()
        {
            _rocket.OnAsteroidCollide -= IncreaseScore;
        }

        private void IncreaseScore(RocketAsteroidCollisionEventData eventData)
        {
            _score++;
            UpdateText();
        }

        private void UpdateText()
        {
            _view.Text.text = string.Format(_config.TextFormat, _score);
        }
    }
}
