using System;
using UnityEngine;

namespace MegaRaketa.Gameplay.Rocket
{
    public interface IRocket
    {
        event Action<RocketAsteroidCollisionEventData> OnAsteroidCollide;

        Vector3 Position { get; }
        float DeviationAngle { get; }

        void Launch();
        void RotateTo(Vector3 targetPoint);
    }
}
