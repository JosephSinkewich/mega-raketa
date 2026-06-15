using CongratulationsWindowView = MegaRaketa.Gameplay.CongratulationsWindow.CongratulationsWindow;
using UnityEngine;

namespace MegaRaketa.Gameplay.WindowsContainer
{
    public class WindowsContainer : MonoBehaviour, IWindowsContainer
    {
        [SerializeField] private CongratulationsWindowView _congratulationsWindowPrefab;

        public void ShowCongratulationsWindow(int score)
        {
            CongratulationsWindowView window = Instantiate(_congratulationsWindowPrefab, transform);
            window.Initialize(score);
        }
    }
}
