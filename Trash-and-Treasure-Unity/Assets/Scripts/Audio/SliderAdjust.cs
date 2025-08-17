using System;
using UnityEngine;
using UnityEngine.UI;

namespace Audio
{
    [RequireComponent(typeof(Slider))]
    public class SliderAdjust : MonoBehaviour
    {
        private enum AudioBusType
        {
            Master,
            BackgroundMusic,
            Sfx
        }

        [SerializeField] private AudioBusType busType;
        [SerializeField] private Slider slider;
        [SerializeField] private float defaultValue = 0.5f;

        private void Start()
        {
            slider = GetComponent<Slider>();
            if (slider != null)
            {
                slider.onValueChanged.AddListener(OnSliderValueChanged);
                slider.value = defaultValue;
            }
        }

        private void OnDestroy()
        {
            if (slider != null)
                slider.onValueChanged.RemoveListener(OnSliderValueChanged);
        }

        private void OnSliderValueChanged(float value)
        {
            switch (busType)
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