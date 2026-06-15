using System;
using MegaRaketa.Gameplay.Camera;
using MegaRaketa.Gameplay.GameEndScenario;
using MegaRaketa.Gameplay.StartScenario;
using MegaRaketa.Gameplay.UIElements.ScoreCounter;
using MegaRaketa.Gameplay.UIElements.WindowsContainer;
using MegaRaketa.Gameplay.VisualObjects.Asteroids;
using MegaRaketa.Gameplay.VisualObjects.Rocket;
using UnityEngine;

namespace MegaRaketa.Tools.GameplayConfigs
{
    [CreateAssetMenu(fileName = "GameplayConfigsFacade", menuName = "MegaRaketa/Gameplay Configs Facade")]
    public class GameplayConfigsFacade : ScriptableObject
    {
        [SerializeField] private RocketConfig _rocketConfig;
        [SerializeField] private CameraOperatorConfig _cameraOperatorConfig;
        [SerializeField] private CameraShakeConfig _cameraShakeConfig;
        [SerializeField] private AsteroidConfig _asteroidConfig;
        [SerializeField] private AsteroidsSpawnerConfig _asteroidsSpawnerConfig;
        [SerializeField] private StartScenarioConfig _startScenarioConfig;
        [SerializeField] private GameEndScenarioConfig _gameEndScenarioConfig;
        [SerializeField] private ScoreCounterConfig _scoreCounterConfig;
        [SerializeField] private WindowsContainerConfig _windowsContainerConfig;

        public ConfigSection[] GetSections()
        {
            return new[]
            {
                new ConfigSection("Rocket", _rocketConfig),
                new ConfigSection("Camera", _cameraOperatorConfig, _cameraShakeConfig),
                new ConfigSection("Asteroids", _asteroidConfig, _asteroidsSpawnerConfig),
                new ConfigSection("Scenarios", _startScenarioConfig, _gameEndScenarioConfig),
                new ConfigSection("UI", _scoreCounterConfig, _windowsContainerConfig),
            };
        }

        [Serializable]
        public struct ConfigSection
        {
            public string Title;
            public ScriptableObject[] Configs;

            public ConfigSection(string title, params ScriptableObject[] configs)
            {
                Title = title;
                Configs = configs;
            }
        }
    }
}
