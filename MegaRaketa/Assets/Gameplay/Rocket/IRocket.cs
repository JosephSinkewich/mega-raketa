using UnityEngine;

namespace MegaRaketa.Gameplay.Rocket
{
    public interface IRocket
    {
        void Launch();
        void RotateTo(Vector3 targetPoint);
    }
}
