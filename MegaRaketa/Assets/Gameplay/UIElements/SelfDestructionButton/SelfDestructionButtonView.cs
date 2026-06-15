using UnityEngine.UI;
using UnityEngine;

namespace MegaRaketa.Gameplay.SelfDestructionButton
{
    [RequireComponent(typeof(Button))]
    public class SelfDestructionButtonView : MonoBehaviour
    {
        public Button Button { get; private set; }

        private void Awake()
        {
            Button = GetComponent<Button>();
            Button.interactable = false;
        }
    }
}
