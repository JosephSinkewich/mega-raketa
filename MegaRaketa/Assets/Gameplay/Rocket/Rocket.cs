using UnityEngine;
using Zenject;

namespace MegaRaketa.Gameplay.Rocket
{
    public class Rocket : MonoBehaviour
    {
        [Inject] private RocketState _state;
    }
}
