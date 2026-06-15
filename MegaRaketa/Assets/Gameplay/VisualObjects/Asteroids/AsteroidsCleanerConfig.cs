using UnityEngine;

namespace MegaRaketa.Gameplay.VisualObjects.Asteroids
{
    [CreateAssetMenu(fileName = "AsteroidsCleanerConfig", menuName = "MegaRaketa/Asteroids Cleaner Config")]
    public class AsteroidsCleanerConfig : ScriptableObject
    {
        [SerializeField, Min(0f)] private float _yThresholdFromRocket;

        public float YThresholdFromRocket => _yThresholdFromRocket;
    }
}
