using UnityEngine;

namespace MegaRaketa.Marketing
{
    [CreateAssetMenu(fileName = "MarketingCreativeProfile", menuName = "MegaRaketa/Marketing Creative Profile")]
    public class MarketingCreativeProfile : ScriptableObject
    {
        [Header("UI Visibility")]
        [SerializeField] private bool _showLaunchText = true;
        [SerializeField] private bool _showScoreCounter = true;
        [SerializeField] private bool _showSelfDestructButton = true;

        [Header("UI Style")]
        [SerializeField] private bool _overrideLaunchTextStyle;
        [SerializeField] private Color _launchTextColor = Color.white;
        [SerializeField, Min(1f)] private float _launchFontSize = 72f;
        [SerializeField] private bool _overrideScoreTextStyle;
        [SerializeField] private Color _scoreTextColor = Color.white;
        [SerializeField, Min(1f)] private float _scoreFontSize = 36f;

        [Header("Scene Look")]
        [SerializeField] private bool _overrideGroundColor;
        [SerializeField] private Color _groundColor = new Color(0.415f, 0.415f, 0.415f, 1f);
        [SerializeField] private bool _overrideCameraBackground;
        [SerializeField] private Color _cameraBackgroundColor = Color.black;

        [Header("Particles")]
        [SerializeField] private bool _engineFireEnabled = true;
        [SerializeField, Min(0f)] private float _engineFireIntensity = 1f;

        [Header("Trail")]
        [SerializeField] private bool _asteroidTrailEnabled = true;

        [Header("Motion (Scene)")]
        [SerializeField] private bool _tapPulseEnabled = true;

        [Header("Config Overrides (Play Mode only, restored on exit)")]
        [SerializeField] private bool _asteroidShakeEnabled = true;
        [SerializeField] private bool _destructionShakeEnabled = true;
        [SerializeField] private bool _gameEndWindowEnabled = true;

        [Header("Timing (StartScenarioConfig, before tap)")]
        [SerializeField] private bool _applyStartScenarioOverrides;
        [SerializeField, Min(0f)] private float _tapObjectDestroyPeriod = 0.25f;
        [SerializeField, Min(0f)] private float _rocketControlUnlockDelay = 3f;
        [SerializeField, Min(0f)] private float _cameraOperatorUnlockDelay = 3f;
        [SerializeField, Min(0f)] private float _asteroidsSpawnerUnlockDelay;
        [SerializeField, Min(0f)] private float _selfDestructionButtonUnlockDelay = 3f;

        [Header("Game End (GameEndScenarioConfig)")]
        [SerializeField, Min(0f)] private float _gameEndWindowDelay = 2f;

        public bool ShowLaunchText => _showLaunchText;
        public bool ShowScoreCounter => _showScoreCounter;
        public bool ShowSelfDestructButton => _showSelfDestructButton;
        public bool OverrideLaunchTextStyle => _overrideLaunchTextStyle;
        public Color LaunchTextColor => _launchTextColor;
        public float LaunchFontSize => _launchFontSize;
        public bool OverrideScoreTextStyle => _overrideScoreTextStyle;
        public Color ScoreTextColor => _scoreTextColor;
        public float ScoreFontSize => _scoreFontSize;
        public bool OverrideGroundColor => _overrideGroundColor;
        public Color GroundColor => _groundColor;
        public bool OverrideCameraBackground => _overrideCameraBackground;
        public Color CameraBackgroundColor => _cameraBackgroundColor;
        public bool EngineFireEnabled => _engineFireEnabled;
        public float EngineFireIntensity => _engineFireIntensity;
        public bool AsteroidTrailEnabled => _asteroidTrailEnabled;
        public bool TapPulseEnabled => _tapPulseEnabled;
        public bool AsteroidShakeEnabled => _asteroidShakeEnabled;
        public bool DestructionShakeEnabled => _destructionShakeEnabled;
        public bool GameEndWindowEnabled => _gameEndWindowEnabled;
        public bool ApplyStartScenarioOverrides => _applyStartScenarioOverrides;
        public float TapObjectDestroyPeriod => _tapObjectDestroyPeriod;
        public float RocketControlUnlockDelay => _rocketControlUnlockDelay;
        public float CameraOperatorUnlockDelay => _cameraOperatorUnlockDelay;
        public float AsteroidsSpawnerUnlockDelay => _asteroidsSpawnerUnlockDelay;
        public float SelfDestructionButtonUnlockDelay => _selfDestructionButtonUnlockDelay;
        public float GameEndWindowDelay => _gameEndWindowDelay;
    }
}
