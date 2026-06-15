using UnityEngine;

namespace MegaRaketa.Gameplay.CameraOperator
{
    [RequireComponent(typeof(Camera))]
    public class CameraOperatorView : MonoBehaviour
    {
        public Camera Camera { get; private set; }

        private void Awake()
        {
            Camera = GetComponent<Camera>();
        }
    }
}
