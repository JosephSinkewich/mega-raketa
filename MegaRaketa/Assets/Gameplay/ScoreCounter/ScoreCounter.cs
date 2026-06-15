using MegaRaketa.Gameplay.Rocket;
using TMPro;
using UnityEngine;
using Zenject;

namespace MegaRaketa.Gameplay.ScoreCounter
{
    [RequireComponent(typeof(TMP_Text))]
    public class ScoreCounter : MonoBehaviour
    {
        [SerializeField] private string _textFormat = "Score: {0}";

        [Inject] private IRocket _rocket;

        private TMP_Text _text;
        private int _score;

        private void Awake()
        {
            _text = GetComponent<TMP_Text>();
        }

        private void Start()
        {
            _rocket.OnAsteroidCollide += IncreaseScore;
            UpdateText();
        }

        private void OnDestroy()
        {
            if (_rocket != null)
            {
                _rocket.OnAsteroidCollide -= IncreaseScore;
            }
        }

        private void IncreaseScore(RocketAsteroidCollisionEventData eventData)
        {
            _score++;
            UpdateText();
        }

        private void UpdateText()
        {
            _text.text = string.Format(_textFormat, _score);
        }
    }
}
