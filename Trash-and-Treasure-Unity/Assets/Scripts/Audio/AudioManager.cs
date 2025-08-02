using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Play a one-shot sound effect, useful for UI interactions or short sound effects
    public void PlayOneShot(string eventPath)
    {
        RuntimeManager.PlayOneShot(eventPath);
    }

    // Play sound at a specific position, useful for placing sounds in the world
    public void PlayOneShotAttached(string eventPath, GameObject gameObject)
    {
        RuntimeManager.PlayOneShotAttached(eventPath, gameObject);
    }

    // Set the volume for a specific bus
    public void SetBusVolume(string busPath, float volume)
    {
        if (string.IsNullOrEmpty(busPath))
        {
            return;
        }
        Bus bus = RuntimeManager.GetBus(busPath);
        bus.setVolume(volume);
    }

    // Set the volume for the master bus
    public void SetMasterVolume(float volume)
    {
        SetBusVolume("bus:/", volume);

    }

    // Set the volume for the background music bus
    public void SetBackgroundMusicVolume(float volume)
    {
        SetBusVolume("bus:/BackgroundMusic", volume);
    }
    
    // Set the volume for the SFX bus
    public void SetSFXVolume(float volume)
    {
        SetBusVolume("bus:/SFX", volume);
    }
}
