using UnityEngine;

namespace MegaRaketa.Gameplay.ScoreCounter
{
    [CreateAssetMenu(fileName = "ScoreCounterConfig", menuName = "MegaRaketa/Score Counter Config")]
    public class ScoreCounterConfig : ScriptableObject
    {
        [SerializeField] private string _textFormat = "Score: {0}";

        public string TextFormat => _textFormat;
    }
}
