using UnityEngine;
using Zenject;

namespace MegaRaketa.Gameplay.Rocket.RocketControl
{
    public class RocketControl : MonoBehaviour, IRocketControl
    {
        [Inject] private IRocket _rocket;

        private bool _isLocked = true;

        private void Update()
        {
            if (_isLocked || !TryGetPointerWorldPoint(out Vector3 targetPoint))
            {
                return;
            }

            _rocket.RotateTo(targetPoint);
        }

        public void Unlock()
        {
            _isLocked = false;
        }

        private bool TryGetPointerWorldPoint(out Vector3 targetPoint)
        {
            targetPoint = Vector3.zero;

            if (!TryGetPointerScreenPoint(out Vector3 screenPoint))
            {
                return false;
            }

            Camera mainCamera = Camera.main;

            if (mainCamera == null)
            {
                return false;
            }

            screenPoint.z = -mainCamera.transform.position.z;
            targetPoint = mainCamera.ScreenToWorldPoint(screenPoint);
            targetPoint.z = 0f;
            return true;
        }

        private bool TryGetPointerScreenPoint(out Vector3 screenPoint)
        {
            if (Input.GetMouseButton(0))
            {
                screenPoint = Input.mousePosition;
                return true;
            }

            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled)
                {
                    screenPoint = touch.position;
                    return true;
                }
            }

            screenPoint = Vector3.zero;
            return false;
        }
    }
}
