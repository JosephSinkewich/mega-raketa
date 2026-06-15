using UnityEngine;

namespace MegaRaketa.Gameplay.StartScenario
{
    [CreateAssetMenu(fileName = "StartScenarioConfig", menuName = "MegaRaketa/Start Scenario Config")]
    public class StartScenarioConfig : ScriptableObject
    {
        [SerializeField, Min(0f)] private float _tapObjectDestroyPeriod = 0.25f;
        [SerializeField, Min(0f)] private float _rocketControlUnlockDelay;
        [SerializeField, Min(0f)] private float _cameraOperatorUnlockDelay;
        [SerializeField, Min(0f)] private float _asteroidsSpawnerUnlockDelay;
        [SerializeField, Min(0f)] private float _selfDestructionButtonUnlockDelay;

        public float TapObjectDestroyPeriod => _tapObjectDestroyPeriod;
        public float RocketControlUnlockDelay => _rocketControlUnlockDelay;
        public float CameraOperatorUnlockDelay => _cameraOperatorUnlockDelay;
        public float AsteroidsSpawnerUnlockDelay => _asteroidsSpawnerUnlockDelay;
        public float SelfDestructionButtonUnlockDelay => _selfDestructionButtonUnlockDelay;
    }
}
