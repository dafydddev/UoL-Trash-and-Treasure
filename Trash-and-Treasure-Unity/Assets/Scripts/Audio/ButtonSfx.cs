using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using FMODUnity;

namespace Audio
{
    [RequireComponent(typeof(Button))]
    public class ButtonSfx : MonoBehaviour, IPointerEnterHandler, IPointerUpHandler
    {
        [SerializeField] private EventReference hoverSoundEvent;
        [SerializeField] private EventReference upSoundEvent;

        private Button _button;

        private void Start()
        {
            _button = GetComponent<Button>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_button != null && _button.interactable && AudioManager.Instance != null)
            {
                AudioManager.PlayOneShot(hoverSoundEvent);
            }
        }
        
        public void OnPointerUp(PointerEventData eventData)
        {
            if (_button != null && _button.interactable && AudioManager.Instance != null)
            {
                AudioManager.PlayOneShot(upSoundEvent);
            }
        }
    }
}