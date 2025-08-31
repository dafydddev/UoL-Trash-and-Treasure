using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace Managers
{
    public class AudioManager : MonoBehaviour
    {
        // AudioInstance Singleton
        public static AudioManager Instance { get; private set; }

        // FMOD Studio Instance
        private FMOD.Studio.System _studioSystem;

        // Default Gain Level
        [SerializeField] private float defaultLevel = 0.5f;

        // FMOD Events References (set in inspector)
        [SerializeField] private EventReference mainMenuBackground;
        [SerializeField] private EventReference gameplayBackground;
        [SerializeField] private EventReference gameStateJingle;

        // FMOD Params
        private const string PauseParameter = "Paused";

        // FMOD Events References (set by scripting on level transitions)
        private EventInstance _currentSceneAudio;

        private void Awake()
        {
            // Singleton pattern implementation
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                // Important: exit early if destroying this instance
                return;
            }

            // Initialize FMOD Studio
            _studioSystem = RuntimeManager.StudioSystem;
    
            // Try to set volumes with error handling
            try
            {
                SetBackgroundMusicVolume(defaultLevel);
                SetMasterVolume(defaultLevel);
                SetSfxVolume(defaultLevel);
            }
            catch (System.Exception e)
            {
                Debug.LogWarning($"Failed to set initial audio volumes: {e.Message}");
            }
    
            // Subscribe to the OnPauseToggled GameEvent
            GameEvents.OnPauseToggled += HandlePause;
        }

        private void OnDestroy()
        {
            // Unsubscribe from the OnPauseToggled GameEvent
            GameEvents.OnPauseToggled -= HandlePause;
        }

        public void PlayMainMenuBackground()
        {
            PlaySceneAudio(mainMenuBackground);
        }

        public void PlayGameplayBackground()
        {
            PlaySceneAudio(gameplayBackground);
        }

        public void PlayGameStateJingle()
        {
            PlayOneShot(gameStateJingle);
        }

        private void PlaySceneAudio(EventReference eventReference)
        {
            // Early exit if the eventReference is null
            if (eventReference.IsNull) return;
            // Stop and existing scene audio
            StopSceneAudio();
            // Create an instance using the valid eventReference
            _currentSceneAudio = RuntimeManager.CreateInstance(eventReference);
            // Start the new instance
            _currentSceneAudio.start();
        }

        public void StopSceneAudio()
        {
            // Early exit if the _currentSceneAudio is invalid
            if (!_currentSceneAudio.isValid()) return;
            // Stop the current scene audio
            _currentSceneAudio.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            // Release the instance
            _currentSceneAudio.release();
        }

        public static void PlayOneShot(EventReference eventReference)
        {
            // Early exit if the eventReference is null
            if (eventReference.IsNull) return;
            // Play a one-shot sound effect using eventReference
            RuntimeManager.PlayOneShot(eventReference);
        }

        // Play a one-shot sound effect using eventReference and param
        public static void PlayOneShot(EventReference eventReference, string parameterName, float parameterValue)
        {
            // Early exit if the eventReference is null
            if (eventReference.IsNull) return;
            // Play a one-shot sound effect using eventReference and param
            EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
            // Set the param using the name and value
            eventInstance.setParameterByName(parameterName, parameterValue);
            // Start the event
            eventInstance.start();
            // Release the event instance
            eventInstance.release();
        }

        // Set the volume for a specific bus
        private static void SetBusVolume(string busPath, float volume)
        {
            if (string.IsNullOrEmpty(busPath))
            {
                return;
            }

            var bus = RuntimeManager.GetBus(busPath);
            bus.setVolume(volume);
        }

        // Set the volume for the master bus
        public static void SetMasterVolume(float volume)
        {
            SetBusVolume("bus:/", volume);
        }

        // Set the volume for the background music bus
        public static void SetBackgroundMusicVolume(float volume)
        {
            SetBusVolume("bus:/Music", volume);
        }

        // Set the volume for the SFX bus
        public static void SetSfxVolume(float volume)
        {
            SetBusVolume("bus:/SFX", volume);
        }

        // Set a global parameter value
        private void SetGlobalParameter(string parameterName, float value)
        {
            _studioSystem.setParameterByName(parameterName, value);
        }
        
        // Reset a global parameter value
        private void ResetGlobalParameter(string parameterName)
        {
            _studioSystem.setParameterByName(parameterName, 0.0f);
        }

        public void ResetPauseParameter()
        {
            ResetGlobalParameter(PauseParameter);
        }

        private void HandlePause(bool isPaused)
        {
            var paramValue = isPaused ? 1.0f : 0.0f;
            SetGlobalParameter(PauseParameter, paramValue);
        }
    }
}