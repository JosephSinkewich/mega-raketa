using UnityEngine;
using Zenject;
using MegaRaketa.Gameplay.Rocket;

namespace MegaRaketa.Gameplay.StartScenario
{
    public class StartScenario : MonoBehaviour
    {
        [Inject] private IRocket _rocket;

        private void Start()
        {
            _rocket.Launch();
        }
    }
}
