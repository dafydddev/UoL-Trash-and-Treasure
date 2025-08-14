using UnityEngine;
using UnityEngine.UI;

namespace Audio
{
    public class SliderAdjust : MonoBehaviour
    {
        private enum AudioBusType
        {
            Master,
            BackgroundMusic,
            SFX
        }
        
        [SerializeField] private AudioBusType busType;
        [SerializeField] private Slider slider;
        [SerializeField] private float defaultValue = 0.5f;

        private void Awake()
        {
            if (slider == null) {
                slider = GetComponent<Slider>();
            }
            slider.onValueChanged.AddListener(OnSliderValueChanged);
            slider.value = defaultValue;
        }
    
        private void OnSliderValueChanged(float value)
        {
            switch (busType)
            {
                case AudioBusType.Master:
                    AudioManager.Instance.SetMasterVolume(value);
                    break;
                case AudioBusType.BackgroundMusic:
                    AudioManager.Instance.SetBackgroundMusicVolume(value);
                    break;
                case AudioBusType.SFX: 
                    AudioManager.Instance.SetSFXVolume(value);
                    break;
            }
        }
    
        private void OnDestroy()
        {
            if (slider != null)
                slider.onValueChanged.RemoveListener(OnSliderValueChanged);
        }

    }
}
