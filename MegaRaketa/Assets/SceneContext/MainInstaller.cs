using MegaRaketa.Gameplay.Asteroids;
using UnityEngine;
using Zenject;

namespace MegaRaketa.SceneContext
{
    public class MainInstaller : MonoInstaller
    {
        [SerializeField] private AsteroidsSpawnerConfig _asteroidsSpawnerConfig;

        public override void InstallBindings()
        {
            Container.BindInstance(_asteroidsSpawnerConfig).AsSingle();
        }
    }
}