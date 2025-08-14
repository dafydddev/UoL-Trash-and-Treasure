using FMOD.Studio;
using FMODUnity;
using Gameplay;
using UnityEngine;

namespace Audio
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }
        private FMOD.Studio.System _studioSystem;
        private const string PauseParameter = "Paused";
        
        [SerializeField] private float defaultLevel = 0.5f;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                _studioSystem = RuntimeManager.StudioSystem;
                SetBackgroundMusicVolume(defaultLevel);
                SetMasterVolume(defaultLevel);
                SetSFXVolume(defaultLevel);
            }
            else
            {
                Destroy(gameObject);
            }

            GameEvents.OnPauseToggled += HandlePause;
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }

            GameEvents.OnPauseToggled -= HandlePause;
        }

        // Play a one-shot sound effect using eventReference (recommended)
        public void PlayOneShot(EventReference eventReference)
        {
            if (!eventReference.IsNull)
            {
                RuntimeManager.PlayOneShot(eventReference);
            }
        }
        
        // Play a one-shot sound effect using string path (legacy support)
        public void PlayOneShot(string eventPath)
        {
            if (!string.IsNullOrEmpty(eventPath))
            {
                RuntimeManager.PlayOneShot(eventPath);
            }
        }

        // Play sound at a specific position using EventReference
        public void PlayOneShotAttached(EventReference eventReference, GameObject attachedGameObject)
        {
            if (!eventReference.IsNull && attachedGameObject != null)
            {
                RuntimeManager.PlayOneShotAttached(eventReference, attachedGameObject);
            }
        }

        // Play sound at a specific position using string path (legacy support)
        public void PlayOneShotAttached(string eventPath, GameObject attachedGameObject)
        {
            if (!string.IsNullOrEmpty(eventPath) && attachedGameObject != null)
            {
                RuntimeManager.PlayOneShotAttached(eventPath, attachedGameObject);
            }
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

        // Set a global parameter value
        private void SetGlobalParameter(string parameterName, float value)
        {
            _studioSystem.setParameterByName(parameterName, value);
        }

        private void HandlePause(bool isPaused)
        {
            SetGlobalParameter(PauseParameter, isPaused ? 1.0f : 0.0f);
        }

        public void ResetPause()
        {
            SetGlobalParameter(PauseParameter, 0.0f);
        }
    }
}