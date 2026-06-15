using UnityEngine;

namespace MegaRaketa.Gameplay.Camera
{
    public interface ICameraOperator
    {
        void Unlock();
        void FocusOnExplosion(Vector3 position, float orthographicSize);
        void SetOffset(object key, Vector3 offset);
        void RemoveOffset(object key);
    }
}
