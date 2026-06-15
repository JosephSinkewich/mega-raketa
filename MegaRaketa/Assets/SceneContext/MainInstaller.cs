using MegaRaketa.Gameplay.Asteroids;
using MegaRaketa.Gameplay.CameraOperator;
using MegaRaketa.Gameplay.GameEndScenario;
using MegaRaketa.Gameplay.Rocket;
using MegaRaketa.Gameplay.Rocket.RocketControl;
using MegaRaketa.Gameplay.ScoreCounter;
using MegaRaketa.Gameplay.SelfDestructionButton;
using MegaRaketa.Gameplay.StartScenario;
using MegaRaketa.Gameplay.WindowsContainer;
using UnityEngine;
using Zenject;

namespace MegaRaketa.SceneContext
{
    public class MainInstaller : MonoInstaller
    {
        [SerializeField] private AsteroidsSpawnerConfig _asteroidsSpawnerConfig;
        [SerializeField] private AsteroidConfig _asteroidConfig;
        [SerializeField] private RocketConfig _rocketConfig;
        [SerializeField] private CameraOperatorConfig _cameraOperatorConfig;
        [SerializeField] private CameraShakeConfig _cameraShakeConfig;
        [SerializeField] private StartScenarioConfig _startScenarioConfig;
        [SerializeField] private GameObject _tapToStartObject;
        [SerializeField] private GameEndScenarioConfig _gameEndScenarioConfig;
        [SerializeField] private WindowsContainerConfig _windowsContainerConfig;
        [SerializeField] private Transform _windowsContainerParent;
        [SerializeField] private ScoreCounterConfig _scoreCounterConfig;

        public override void InstallBindings()
        {
            Container.BindInstance(_asteroidsSpawnerConfig).AsSingle();
            Container.BindInstance(_asteroidConfig).AsSingle();
            Container.BindInstance(_rocketConfig).AsSingle();
            Container.BindInstance(_cameraOperatorConfig).AsSingle();
            Container.BindInstance(_cameraShakeConfig).AsSingle();
            Container.BindInstance(_startScenarioConfig).AsSingle();
            Container.BindInstance(_gameEndScenarioConfig).AsSingle();
            Container.BindInstance(_windowsContainerConfig).AsSingle();
            Container.BindInstance(_scoreCounterConfig).AsSingle();
            Container.BindInstance(_tapToStartObject).WithId(BindingIds.TapToStart).AsSingle();
            Container.BindInstance(_windowsContainerParent).WithId(BindingIds.WindowsParent).AsSingle();

            Container.BindInterfacesAndSelfTo<Rocket>().AsSingle();
            Container.BindInterfacesAndSelfTo<RocketControl>().AsSingle();
            Container.BindInterfacesAndSelfTo<CameraOperator>().AsSingle();
            Container.BindInterfacesAndSelfTo<CameraShake>().AsSingle();
            Container.BindInterfacesAndSelfTo<AsteroidsSpawner>().AsSingle();
            Container.BindInterfacesAndSelfTo<AsteroidsCleaner>().AsSingle();
            Container.BindInterfacesAndSelfTo<ScoreCounter>().AsSingle();
            Container.BindInterfacesAndSelfTo<SelfDestructionButton>().AsSingle();
            Container.BindInterfacesAndSelfTo<WindowsContainer>().AsSingle();
            Container.BindInterfacesAndSelfTo<StartScenario>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameEndScenario>().AsSingle();
            Container.Bind<Asteroid>().AsTransient();
        }
    }
}
