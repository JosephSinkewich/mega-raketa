using MegaRaketa.Gameplay.Rocket;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace MegaRaketa.Gameplay.SelfDestructionButton
{
    [RequireComponent(typeof(Button))]
    public class SelfDestructionButton : MonoBehaviour, ISelfDestructionButton
    {
        [Inject] private IRocket _rocket;

        private Button _button;
        private bool _isLocked = true;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.interactable = false;
        }

        private void Start()
        {
            _rocket.OnExplode += HandleRocketExplode;
            _button.onClick.AddListener(HandleClick);
        }

        private void OnDestroy()
        {
            if (_rocket != null)
            {
                _rocket.OnExplode -= HandleRocketExplode;
            }

            if (_button != null)
            {
                _button.onClick.RemoveListener(HandleClick);
            }
        }

        public void Unlock()
        {
            _isLocked = false;
            _button.interactable = true;
        }

        private void HandleClick()
        {
            if (_isLocked)
            {
                return;
            }

            _rocket.Explode();
        }

        private void HandleRocketExplode()
        {
            _isLocked = true;
            _button.interactable = false;
        }
    }
}
