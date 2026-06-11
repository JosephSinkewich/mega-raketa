using MegaRaketa.Gameplay.Rocket;
using MegaRaketa.Tweens;
using PrimeTween;
using UnityEngine;
using Zenject;

namespace MegaRaketa.Gameplay.StartScenario
{
    public class StartScenario : MonoBehaviour
    {
        [SerializeField] private GameObject _tapObject;
        [SerializeField, Min(0f)] private float _tapObjectDestroyPeriod = 0.25f;

        [Inject] private IRocket _rocket;

        private bool _isLaunched;

        private void Update()
        {
            if (_isLaunched || !IsTapStarted())
            {
                return;
            }

            _isLaunched = true;
            _rocket.Launch();
            DestroyTapObject();
        }

        private bool IsTapStarted()
        {
            if (Input.GetMouseButtonDown(0))
            {
                return true;
            }

            return Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began;
        }

        private void DestroyTapObject()
        {
            if (_tapObject == null)
            {
                return;
            }

            Transform tapObjectTransform = _tapObject.transform;
            Pulser pulser = _tapObject.GetComponent<Pulser>();

            if (pulser != null)
            {
                pulser.StopPulse();
                pulser.enabled = false;
            }

            Tween.Scale(tapObjectTransform, Vector3.zero, _tapObjectDestroyPeriod)
                .OnComplete(_tapObject, Destroy);
        }
    }
}
