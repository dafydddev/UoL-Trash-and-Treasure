using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            }

            // Initialize FMOD Studio
            _studioSystem = RuntimeManager.StudioSystem;
            SetBackgroundMusicVolume(defaultLevel);
            SetMasterVolume(defaultLevel);
            SetSfxVolume(defaultLevel);

            // Subscribe to the GameEvents
            GameEvents.OnPauseToggled += HandlePause;
            // Subscribe to the SceneManager sceneLoaded event
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDestroy()
        {
            GameEvents.OnPauseToggled -= HandlePause;
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode __)
        {
            HandlePause(GameEvents.IsPaused());
            if (scene.name == "MainMenu")
            {
                PlayMainMenuBackground();
            }
            else
            {
                // Add a small delay to prevent audio stuttering during scene transitions
                StartCoroutine(StopSceneAudioDelayed(0.5f));
            }
        }

        private System.Collections.IEnumerator StopSceneAudioDelayed(float delay)
        {
            yield return new WaitForSeconds(delay);
            StopSceneAudio();
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
            StopSceneAudio();
            if (eventReference.IsNull) return;
            _currentSceneAudio = RuntimeManager.CreateInstance(eventReference);
            _currentSceneAudio.start();
        }

        public void StopSceneAudio()
        {
            if (!_currentSceneAudio.isValid()) return;
            _currentSceneAudio.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            _currentSceneAudio.release();
        }

        // Play a one-shot sound effect using eventReference
        public static void PlayOneShot(EventReference eventReference)
        {
            if (!eventReference.IsNull)
            {
                RuntimeManager.PlayOneShot(eventReference);
            }
        }

        // Play a one-shot sound effect using eventReference and param
        public static void PlayOneShot(EventReference eventReference, string parameterName, float parameterValue)
        {
            if (eventReference.IsNull) return;
            EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
            eventInstance.setParameterByName(parameterName, parameterValue);
            eventInstance.start();
            eventInstance.release();
        }

        // Play a one-shot sound effect using a string path (legacy)
        public void PlayOneShot(string eventPath)
        {
            if (!string.IsNullOrEmpty(eventPath))
            {
                RuntimeManager.PlayOneShot(eventPath);
            }
        }

        // Play sound at a specific position using a string path (legacy support)
        public void PlayOneShotAttached(string eventPath, GameObject attachedGameObject)
        {
            if (!string.IsNullOrEmpty(eventPath) && attachedGameObject != null)
            {
                RuntimeManager.PlayOneShotAttached(eventPath, attachedGameObject);
            }
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
            SetBusVolume("bus:/BackgroundMusic", volume);
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

        private void HandlePause(bool isPaused)
        {
            SetGlobalParameter(PauseParameter, isPaused ? 1.0f : 0.0f);
        }
    }
}