using FMODUnity;
using Managers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Audio.SFXs
{
    [RequireComponent(typeof(Button))]
    public class ButtonSfx : MonoBehaviour, IPointerEnterHandler, IPointerUpHandler
    {
        // FMOD Event References
        [SerializeField] private EventReference hoverSoundEvent;
        [SerializeField] private EventReference upSoundEvent;
        // Reference to the button attached to this script
        private Button _button;

        private void Start()
        {
            // Get the button component
            _button = GetComponent<Button>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            // If we have a button and an audio instance, play the one shot
            if (_button && AudioManager.Instance)
            {
                AudioManager.PlayOneShot(hoverSoundEvent);
            }
        }
        
        public void OnPointerUp(PointerEventData eventData)
        {
            // If we have a button and an audio instance, play the one shot
            if (_button && AudioManager.Instance)
            {
                AudioManager.PlayOneShot(upSoundEvent);
            }
        }
    }
}