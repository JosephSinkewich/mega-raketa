using CongratulationsWindowView = MegaRaketa.Gameplay.UIElements.CongratulationsWindow.CongratulationsWindow;
using UnityEngine;

namespace MegaRaketa.Gameplay.UIElements.WindowsContainer
{
    [CreateAssetMenu(fileName = "WindowsContainerConfig", menuName = "MegaRaketa/Windows Container Config")]
    public class WindowsContainerConfig : ScriptableObject
    {
        [SerializeField] private CongratulationsWindowView _congratulationsWindowPrefab;

        public CongratulationsWindowView CongratulationsWindowPrefab => _congratulationsWindowPrefab;
    }
}
