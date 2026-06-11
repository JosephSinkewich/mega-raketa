using MegaRaketa.Gameplay.Rocket;
using Zenject;

namespace MegaRaketa.SceneContext
{
    public class MainInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IRocketState>().To<RocketState>().AsSingle();
        }
    }
}