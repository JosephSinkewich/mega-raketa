using UnityEngine;

namespace MegaRaketa.Gameplay.GameEndScenario
{
    [CreateAssetMenu(fileName = "GameEndScenarioConfig", menuName = "MegaRaketa/Game End Scenario Config")]
    public class GameEndScenarioConfig : ScriptableObject
    {
        [SerializeField, Min(0f)] private float _windowShowDelay = 2f;

        public float WindowShowDelay => _windowShowDelay;
    }
}
