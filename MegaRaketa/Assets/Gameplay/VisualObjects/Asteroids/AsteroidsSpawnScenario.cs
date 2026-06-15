using UnityEngine;

namespace MegaRaketa.Gameplay.Asteroids
{
    [CreateAssetMenu(fileName = "AsteroidsSpawnScenario", menuName = "MegaRaketa/Asteroids Spawn Scenario")]
    public class AsteroidsSpawnScenario : ScriptableObject
    {
        [SerializeField] private AsteroidsSpawnSection[] _sections;

        public int SectionCount => _sections != null ? _sections.Length : 0;

        public AsteroidsSpawnSection GetSection(int index)
        {
            return _sections[index];
        }
    }
}
