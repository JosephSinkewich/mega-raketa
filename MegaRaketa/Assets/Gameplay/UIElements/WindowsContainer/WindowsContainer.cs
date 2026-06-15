using CongratulationsWindowView = MegaRaketa.Gameplay.UIElements.CongratulationsWindow.CongratulationsWindow;
using MegaRaketa.SceneContext;
using UnityEngine;
using Zenject;

namespace MegaRaketa.Gameplay.UIElements.WindowsContainer
{
    public class WindowsContainer : IWindowsContainer
    {
        [Inject] private WindowsContainerConfig _config;
        [Inject(Id = BindingIds.WindowsParent)] private Transform _windowsParent;

        public void ShowCongratulationsWindow(int score)
        {
            CongratulationsWindowView window = UnityEngine.Object.Instantiate(
                _config.CongratulationsWindowPrefab,
                _windowsParent);
            window.Initialize(score);
        }
    }
}
