using UnityEngine;

namespace MegaRaketa.Gameplay.Camera
{
    [RequireComponent(typeof(UnityEngine.Camera))]
    public class CameraOperatorView : MonoBehaviour
    {
        public UnityEngine.Camera Camera { get; private set; }

        private void Awake()
        {
            Camera = GetComponent<UnityEngine.Camera>();
        }
    }
}
