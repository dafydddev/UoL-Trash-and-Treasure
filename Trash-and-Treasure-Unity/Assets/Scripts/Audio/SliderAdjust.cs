using Audio;
using UnityEngine;
using UnityEngine.UI;

public class SliderAdjust : MonoBehaviour
{
    public enum AudioBusType
    {
        Master,
        BackgroundMusic,
        SFX
    }
    
    [SerializeField] private AudioBusType busType;
    [SerializeField] private Slider slider;

    private void Start()
    {
        if (slider == null) {
            slider = GetComponent<Slider>();
        }
        slider.onValueChanged.AddListener(OnSliderValueChanged);
    }
    
    private void OnSliderValueChanged(float value)
    {
        switch (busType)
        {
            case AudioBusType.Master:
                Debug.Log($"Setting master volume to: {value}");
                AudioManager.Instance.SetMasterVolume(value);
                break;
            case AudioBusType.BackgroundMusic:
                Debug.Log($"Setting BGM volume to: {value}");
                AudioManager.Instance.SetBackgroundMusicVolume(value);
                break;
            case AudioBusType.SFX: 
                Debug.Log($"Setting SFX volume to: {value}");
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
