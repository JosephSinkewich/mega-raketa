using TMPro;
using UnityEngine;

namespace MegaRaketa.Gameplay.ScoreCounter
{
    [RequireComponent(typeof(TMP_Text))]
    public class ScoreCounterView : MonoBehaviour
    {
        public TMP_Text Text { get; private set; }

        private void Awake()
        {
            Text = GetComponent<TMP_Text>();
        }
    }
}
