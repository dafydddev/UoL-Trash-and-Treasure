using System;
using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Audio
{
    [RequireComponent(typeof(Slider))]
    public class AudioSlider : MonoBehaviour
    {
        // The buses defined in FMOD
        private enum AudioBusType
        {
            Master,
            BackgroundMusic,
            Sfx
        }
        // The slider that this script will control
        [SerializeField] private Slider slider;
        // The bus that the slider will control 
        [SerializeField] private AudioBusType bus;
        // The default slider value
        [SerializeField] private float defaultValue = 0.5f;

        private void Start()
        {
            slider = GetComponent<Slider>();
            // Early exit when we cannot access the slider
            if (!slider) return;
            // Add the onValueChanged event to the slider
            slider.onValueChanged.AddListener(OnSliderValueChanged);
            // Set the default value
            slider.value = defaultValue;
        }

        private void OnDestroy()
        {
            // Early exit when we cannot access the slider
            if (!slider) return;
            // Remove the onValueChanged event to the slider
            slider.onValueChanged.RemoveListener(OnSliderValueChanged);
        }

        private void OnSliderValueChanged(float value)
        {
            // Triggered when the slider attached to this script is adjusted
            // Delegate to the AudioManager functions, based on the bus, with the new slider value
            switch (bus)
            {
                case AudioBusType.Master:
                    AudioManager.SetMasterVolume(value);
                    break;
                case AudioBusType.BackgroundMusic:
                    AudioManager.SetBackgroundMusicVolume(value);
                    break;
                case AudioBusType.Sfx:
                    AudioManager.SetSfxVolume(value);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}