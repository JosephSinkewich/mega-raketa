using System.Collections.Generic;
using MegaRaketa.Gameplay.Camera;
using MegaRaketa.Gameplay.GameEndScenario;
using MegaRaketa.Gameplay.StartScenario;
using MegaRaketa.Gameplay.VisualObjects.Rocket;
using MegaRaketa.Tweens;
using TMPro;
using UnityEngine;

namespace MegaRaketa.Marketing
{
    public class MarketingCreativeRig : MonoBehaviour
    {
        private const float DisabledGameEndWindowDelay = 99999f;

        [SerializeField] private MarketingCreativeProfile _profile;
        [SerializeField] private GameObject _launchText;
        [SerializeField] private GameObject _scoreText;
        [SerializeField] private GameObject _selfDestructButton;
        [SerializeField] private SpriteRenderer _groundRenderer;
        [SerializeField] private UnityEngine.Camera _camera;
        [SerializeField] private RocketView _rocketView;
        [SerializeField] private Transform _asteroidsContainer;
        [SerializeField] private CameraShakeConfig _cameraShakeConfig;
        [SerializeField] private StartScenarioConfig _startScenarioConfig;
        [SerializeField] private GameEndScenarioConfig _gameEndScenarioConfig;

        private Pulser _launchTextPulser;
        private TMP_Text _launchTextLabel;
        private TMP_Text _scoreTextLabel;
        private ParticleSystem _engineFire;
        private SpriteRenderer _rocketSpriteRenderer;
        private Sprite _baselineRocketSprite;
        private bool _hasRocketSpriteBaseline;
        private readonly Dictionary<SpriteRenderer, Sprite> _asteroidSpriteBaselines = new Dictionary<SpriteRenderer, Sprite>();
        private float _baselineEngineFireRate;
        private ParticleSystem.MinMaxGradient _baselineEngineFireStartColor;
        private bool _hasEngineFireBaseline;
        private bool _baselineEngineFireGameObjectActive = true;
        private Vector3 _baselineShakeStrength;
        private Vector3 _baselineDestructionShakeStrength;
        private float _baselineGameEndDelay;
        private float _baselineTapObjectDestroyPeriod;
        private float _baselineRocketControlUnlockDelay;
        private float _baselineCameraOperatorUnlockDelay;
        private float _baselineAsteroidsSpawnerUnlockDelay;
        private float _baselineSelfDestructionButtonUnlockDelay;
        private bool _playModeSnapshotsCaptured;

        private void Awake()
        {
            CacheSceneReferences();
        }

        private void OnEnable()
        {
            CacheSceneReferences();

            if (!Application.isPlaying)
            {
                return;
            }

            CapturePlayModeSnapshots();
            ApplyProfile();
        }

        private void OnDisable()
        {
            RestorePlayModeSnapshots();
            RestoreSpriteBaselines();
        }

        private void Update()
        {
            if (!Application.isPlaying || _profile == null)
            {
                return;
            }

            ApplyProfile();
        }

        private void OnValidate()
        {
            CacheSceneReferences();

            if (!Application.isPlaying || _profile == null)
            {
                return;
            }

            ApplyProfile();
        }

        public void RestorePlayModeSnapshotsFromEditor()
        {
            RestorePlayModeSnapshots();
        }

        private void CacheSceneReferences()
        {
            if (_launchText != null)
            {
                _launchTextPulser = _launchText.GetComponent<Pulser>();
                _launchTextLabel = _launchText.GetComponent<TMP_Text>();
            }

            if (_scoreText != null)
            {
                _scoreTextLabel = _scoreText.GetComponent<TMP_Text>();
            }

            if (_rocketView != null)
            {
                _engineFire = _rocketView.EngineFire;
                _rocketSpriteRenderer = _rocketView.GetComponentInChildren<SpriteRenderer>(true);
            }

            if (_camera == null)
            {
                _camera = UnityEngine.Camera.main;
            }
        }

        private void CapturePlayModeSnapshots()
        {
            if (_playModeSnapshotsCaptured)
            {
                return;
            }

            if (_cameraShakeConfig != null)
            {
                _baselineShakeStrength = MarketingSerializedConfigUtility.GetVector3(
                    _cameraShakeConfig,
                    "_strengthPerAsteroidSize");
                _baselineDestructionShakeStrength = MarketingSerializedConfigUtility.GetVector3(
                    _cameraShakeConfig,
                    "_destructionStrength");
            }

            if (_gameEndScenarioConfig != null)
            {
                _baselineGameEndDelay = MarketingSerializedConfigUtility.GetFloat(
                    _gameEndScenarioConfig,
                    "_windowShowDelay");
            }

            if (_startScenarioConfig != null)
            {
                _baselineTapObjectDestroyPeriod = MarketingSerializedConfigUtility.GetFloat(
                    _startScenarioConfig,
                    "_tapObjectDestroyPeriod");
                _baselineRocketControlUnlockDelay = MarketingSerializedConfigUtility.GetFloat(
                    _startScenarioConfig,
                    "_rocketControlUnlockDelay");
                _baselineCameraOperatorUnlockDelay = MarketingSerializedConfigUtility.GetFloat(
                    _startScenarioConfig,
                    "_cameraOperatorUnlockDelay");
                _baselineAsteroidsSpawnerUnlockDelay = MarketingSerializedConfigUtility.GetFloat(
                    _startScenarioConfig,
                    "_asteroidsSpawnerUnlockDelay");
                _baselineSelfDestructionButtonUnlockDelay = MarketingSerializedConfigUtility.GetFloat(
                    _startScenarioConfig,
                    "_selfDestructionButtonUnlockDelay");
            }

            CaptureEngineFireBaseline();
            _playModeSnapshotsCaptured = true;
        }

        private void CaptureEngineFireBaseline()
        {
            if (_engineFire == null)
            {
                _hasEngineFireBaseline = false;
                return;
            }

            var main = _engineFire.main;
            _baselineEngineFireRate = _engineFire.emission.rateOverTime.constant;
            _baselineEngineFireStartColor = main.startColor;
            _baselineEngineFireGameObjectActive = _engineFire.gameObject.activeSelf;
            _hasEngineFireBaseline = true;
        }

        private void RestorePlayModeSnapshots()
        {
            if (!_playModeSnapshotsCaptured)
            {
                return;
            }

            if (_cameraShakeConfig != null)
            {
                MarketingSerializedConfigUtility.SetVector3(
                    _cameraShakeConfig,
                    "_strengthPerAsteroidSize",
                    _baselineShakeStrength);
                MarketingSerializedConfigUtility.SetVector3(
                    _cameraShakeConfig,
                    "_destructionStrength",
                    _baselineDestructionShakeStrength);
            }

            if (_gameEndScenarioConfig != null)
            {
                MarketingSerializedConfigUtility.SetFloat(
                    _gameEndScenarioConfig,
                    "_windowShowDelay",
                    _baselineGameEndDelay);
            }

            if (_startScenarioConfig != null)
            {
                MarketingSerializedConfigUtility.SetFloat(
                    _startScenarioConfig,
                    "_tapObjectDestroyPeriod",
                    _baselineTapObjectDestroyPeriod);
                MarketingSerializedConfigUtility.SetFloat(
                    _startScenarioConfig,
                    "_rocketControlUnlockDelay",
                    _baselineRocketControlUnlockDelay);
                MarketingSerializedConfigUtility.SetFloat(
                    _startScenarioConfig,
                    "_cameraOperatorUnlockDelay",
                    _baselineCameraOperatorUnlockDelay);
                MarketingSerializedConfigUtility.SetFloat(
                    _startScenarioConfig,
                    "_asteroidsSpawnerUnlockDelay",
                    _baselineAsteroidsSpawnerUnlockDelay);
                MarketingSerializedConfigUtility.SetFloat(
                    _startScenarioConfig,
                    "_selfDestructionButtonUnlockDelay",
                    _baselineSelfDestructionButtonUnlockDelay);
            }

            RestoreEngineFireBaseline();
            _playModeSnapshotsCaptured = false;
        }

        private void RestoreEngineFireBaseline()
        {
            if (!_hasEngineFireBaseline || _engineFire == null)
            {
                return;
            }

            _engineFire.gameObject.SetActive(_baselineEngineFireGameObjectActive);

            var emission = _engineFire.emission;
            var rate = emission.rateOverTime;
            rate.constant = _baselineEngineFireRate;
            emission.rateOverTime = rate;

            var main = _engineFire.main;
            main.startColor = _baselineEngineFireStartColor;
        }

        private void ApplyProfile()
        {
            if (_profile == null)
            {
                return;
            }

            ApplyUiVisibility();
            ApplyUiStyle();
            ApplySceneLook();
            ApplySprites();
            ApplyEngineFire();
            ApplyAsteroidTrails();
            ApplyTapPulse();
            ApplyConfigOverrides();
        }

        private void ApplyUiVisibility()
        {
            if (_launchText != null)
            {
                _launchText.SetActive(_profile.ShowLaunchText);
            }

            if (_scoreText != null)
            {
                _scoreText.SetActive(_profile.ShowScoreCounter);
            }

            if (_selfDestructButton != null)
            {
                _selfDestructButton.SetActive(_profile.ShowSelfDestructButton);
            }
        }

        private void ApplyUiStyle()
        {
            if (_launchTextLabel != null && _profile.OverrideLaunchTextStyle)
            {
                _launchTextLabel.enableAutoSizing = false;
                _launchTextLabel.fontSize = _profile.LaunchFontSize;
                _launchTextLabel.color = _profile.LaunchTextColor;
            }

            if (_scoreTextLabel != null && _profile.OverrideScoreTextStyle)
            {
                _scoreTextLabel.color = _profile.ScoreTextColor;
                _scoreTextLabel.fontSize = _profile.ScoreFontSize;
            }
        }

        private void ApplySceneLook()
        {
            if (_groundRenderer != null && _profile.OverrideGroundColor)
            {
                _groundRenderer.color = _profile.GroundColor;
            }

            if (_camera != null && _profile.OverrideCameraBackground)
            {
                _camera.backgroundColor = _profile.CameraBackgroundColor;
            }
        }

        private void ApplySprites()
        {
            ApplyRocketSprite();
            ApplyAsteroidSprites();
        }

        private void ApplyRocketSprite()
        {
            if (_rocketSpriteRenderer == null)
            {
                return;
            }

            CaptureRocketSpriteBaseline();

            if (_profile.OverrideRocketSprite && _profile.RocketSprite != null)
            {
                _rocketSpriteRenderer.sprite = _profile.RocketSprite;
                return;
            }

            if (_hasRocketSpriteBaseline)
            {
                _rocketSpriteRenderer.sprite = _baselineRocketSprite;
            }
        }

        private void ApplyAsteroidSprites()
        {
            if (_asteroidsContainer == null)
            {
                return;
            }

            for (int i = 0; i < _asteroidsContainer.childCount; i++)
            {
                Transform asteroidTransform = _asteroidsContainer.GetChild(i);
                SpriteRenderer spriteRenderer = asteroidTransform.GetComponentInChildren<SpriteRenderer>(true);

                if (spriteRenderer == null)
                {
                    continue;
                }

                CaptureAsteroidSpriteBaseline(spriteRenderer);

                if (_profile.OverrideAsteroidSprite && _profile.AsteroidSprite != null)
                {
                    spriteRenderer.sprite = _profile.AsteroidSprite;
                }
                else if (_asteroidSpriteBaselines.TryGetValue(spriteRenderer, out Sprite baselineSprite))
                {
                    spriteRenderer.sprite = baselineSprite;
                }
            }
        }

        private void CaptureRocketSpriteBaseline()
        {
            if (_hasRocketSpriteBaseline || _rocketSpriteRenderer == null)
            {
                return;
            }

            _baselineRocketSprite = _rocketSpriteRenderer.sprite;
            _hasRocketSpriteBaseline = true;
        }

        private void CaptureAsteroidSpriteBaseline(SpriteRenderer spriteRenderer)
        {
            if (_asteroidSpriteBaselines.ContainsKey(spriteRenderer))
            {
                return;
            }

            _asteroidSpriteBaselines[spriteRenderer] = spriteRenderer.sprite;
        }

        private void RestoreSpriteBaselines()
        {
            if (_hasRocketSpriteBaseline && _rocketSpriteRenderer != null)
            {
                _rocketSpriteRenderer.sprite = _baselineRocketSprite;
            }

            foreach (KeyValuePair<SpriteRenderer, Sprite> asteroidSpriteBaseline in _asteroidSpriteBaselines)
            {
                if (asteroidSpriteBaseline.Key != null)
                {
                    asteroidSpriteBaseline.Key.sprite = asteroidSpriteBaseline.Value;
                }
            }

            _hasRocketSpriteBaseline = false;
            _asteroidSpriteBaselines.Clear();
        }

        private void ApplyEngineFire()
        {
            if (_engineFire == null)
            {
                return;
            }

            _engineFire.gameObject.SetActive(_profile.EngineFireEnabled);

            if (!_profile.EngineFireEnabled)
            {
                return;
            }

            if (!_hasEngineFireBaseline)
            {
                CaptureEngineFireBaseline();
            }

            if (Mathf.Approximately(_profile.EngineFireIntensity, 1f))
            {
                return;
            }

            var emission = _engineFire.emission;
            var rate = emission.rateOverTime;
            rate.constant = _baselineEngineFireRate * _profile.EngineFireIntensity;
            emission.rateOverTime = rate;
        }

        private void ApplyAsteroidTrails()
        {
            if (_asteroidsContainer == null)
            {
                return;
            }

            for (int i = 0; i < _asteroidsContainer.childCount; i++)
            {
                Transform asteroidTransform = _asteroidsContainer.GetChild(i);
                TrailRenderer[] trails = asteroidTransform.GetComponentsInChildren<TrailRenderer>(true);

                for (int j = 0; j < trails.Length; j++)
                {
                    trails[j].enabled = _profile.AsteroidTrailEnabled;
                }
            }
        }

        private void ApplyTapPulse()
        {
            if (_launchTextPulser == null)
            {
                return;
            }

            _launchTextPulser.enabled = _profile.TapPulseEnabled;
        }

        private void ApplyConfigOverrides()
        {
            if (_cameraShakeConfig != null && _playModeSnapshotsCaptured)
            {
                Vector3 asteroidStrength = _profile.AsteroidShakeEnabled ? _baselineShakeStrength : Vector3.zero;
                MarketingSerializedConfigUtility.SetVector3(
                    _cameraShakeConfig,
                    "_strengthPerAsteroidSize",
                    asteroidStrength);

                Vector3 destructionStrength = _profile.DestructionShakeEnabled
                    ? _baselineDestructionShakeStrength
                    : Vector3.zero;
                MarketingSerializedConfigUtility.SetVector3(
                    _cameraShakeConfig,
                    "_destructionStrength",
                    destructionStrength);
            }

            if (_gameEndScenarioConfig != null && _playModeSnapshotsCaptured)
            {
                if (!_profile.GameEndWindowEnabled)
                {
                    MarketingSerializedConfigUtility.SetFloat(
                        _gameEndScenarioConfig,
                        "_windowShowDelay",
                        DisabledGameEndWindowDelay);
                }
                else
                {
                    MarketingSerializedConfigUtility.SetFloat(
                        _gameEndScenarioConfig,
                        "_windowShowDelay",
                        _profile.GameEndWindowDelay);
                }
            }

            if (_startScenarioConfig != null && _profile.ApplyStartScenarioOverrides)
            {
                MarketingSerializedConfigUtility.SetFloat(
                    _startScenarioConfig,
                    "_tapObjectDestroyPeriod",
                    _profile.TapObjectDestroyPeriod);
                MarketingSerializedConfigUtility.SetFloat(
                    _startScenarioConfig,
                    "_rocketControlUnlockDelay",
                    _profile.RocketControlUnlockDelay);
                MarketingSerializedConfigUtility.SetFloat(
                    _startScenarioConfig,
                    "_cameraOperatorUnlockDelay",
                    _profile.CameraOperatorUnlockDelay);
                MarketingSerializedConfigUtility.SetFloat(
                    _startScenarioConfig,
                    "_asteroidsSpawnerUnlockDelay",
                    _profile.AsteroidsSpawnerUnlockDelay);
                MarketingSerializedConfigUtility.SetFloat(
                    _startScenarioConfig,
                    "_selfDestructionButtonUnlockDelay",
                    _profile.SelfDestructionButtonUnlockDelay);
            }
        }
    }
}
