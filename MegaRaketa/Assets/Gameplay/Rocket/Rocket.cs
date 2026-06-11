using UnityEngine;
using Zenject;

namespace MegaRaketa.Gameplay.Rocket
{
    public class Rocket : MonoBehaviour
    {
        [Inject] private IRocketState _state;
    }
}
