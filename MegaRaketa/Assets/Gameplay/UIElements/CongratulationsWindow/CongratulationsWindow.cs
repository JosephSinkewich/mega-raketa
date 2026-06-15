using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MegaRaketa.Gameplay.UIElements.CongratulationsWindow
{
    public class CongratulationsWindow : MonoBehaviour
    {
        [SerializeField] private TMP_Text _scoreText;
        [SerializeField] private Button _restartButton;
        [SerializeField] private string _scoreFormat = "Score: {0}";

        private void Awake()
        {
            _restartButton.onClick.AddListener(RestartScene);
        }

        private void OnDestroy()
        {
            if (_restartButton != null)
            {
                _restartButton.onClick.RemoveListener(RestartScene);
            }
        }

        public void Initialize(int score)
        {
            _scoreText.text = string.Format(_scoreFormat, score);
        }

        private void RestartScene()
        {
            Scene activeScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(activeScene.buildIndex);
        }
    }
}
