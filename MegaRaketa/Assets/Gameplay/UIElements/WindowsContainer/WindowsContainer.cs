using MegaRaketa.SceneContext;
using UnityEngine;
using Zenject;

namespace MegaRaketa.Gameplay.WindowsContainer
{
    public class WindowsContainer : IWindowsContainer
    {
        [Inject] private WindowsContainerConfig _config;
        [Inject(Id = BindingIds.WindowsParent)] private Transform _windowsParent;

        public void ShowCongratulationsWindow(int score)
        {
            CongratulationsWindow.CongratulationsWindow window = UnityEngine.Object.Instantiate(
                _config.CongratulationsWindowPrefab,
                _windowsParent);
            window.Initialize(score);
        }
    }
}
