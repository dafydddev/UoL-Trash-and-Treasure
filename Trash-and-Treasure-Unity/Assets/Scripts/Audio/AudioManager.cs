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
        [SerializeField] private bool pauseAudioOnFocusLoss = true;

        [SerializeField] private EventReference mainMenuBackground;
        [SerializeField] private EventReference gameplayBackground;
        [SerializeField] private EventReference gameStateJingle;

        private EventInstance _currentSceneAudio;
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                _studioSystem = RuntimeManager.StudioSystem;
                SetBackgroundMusicVolume(defaultLevel);
                SetMasterVolume(defaultLevel);
                SetSfxVolume(defaultLevel);
                GameEvents.OnPauseToggled += HandlePause;
                SceneManager.sceneLoaded += OnSceneLoaded;
            }
            else
            {
                Destroy(gameObject);
            }
<<<<<<< Updated upstream:Trash-and-Treasure-Unity/Assets/Scripts/Audio/AudioManager.cs
=======

            // Initialize FMOD Studio
            _studioSystem = RuntimeManager.StudioSystem;
            SetBackgroundMusicVolume(defaultLevel);
            SetMasterVolume(defaultLevel);
            SetSfxVolume(defaultLevel);
            // Subscribe to the OnPauseToggled GameEvent
            GameEvents.OnPauseToggled += HandlePause;
>>>>>>> Stashed changes:Trash-and-Treasure-Unity/Assets/Scripts/Managers/AudioManager.cs
        }

        private void OnDestroy()
        {
<<<<<<< Updated upstream:Trash-and-Treasure-Unity/Assets/Scripts/Audio/AudioManager.cs
            if (Instance == this)
            {
                Instance = null;
                GameEvents.OnPauseToggled -= HandlePause;
                SceneManager.sceneLoaded -= OnSceneLoaded;
            }
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode __)
        {
            ResetPause();
            if (scene.name == "MainMenu")
            {
                PlayMainMenuBackground();
            }
            else
            {
                // Add a small delay to prevent audio stuttering during scene transitions
                StartCoroutine(StopSceneAudioDelayed(2.0f));
            }
        }

        private System.Collections.IEnumerator StopSceneAudioDelayed(float delay)
        {
            yield return new WaitForSeconds(delay);
            StopSceneAudio();
=======
            // Unsubscribe from the OnPauseToggled GameEvent
            GameEvents.OnPauseToggled -= HandlePause;
>>>>>>> Stashed changes:Trash-and-Treasure-Unity/Assets/Scripts/Managers/AudioManager.cs
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
<<<<<<< Updated upstream:Trash-and-Treasure-Unity/Assets/Scripts/Audio/AudioManager.cs
            StopSceneAudio();
            if (!eventReference.IsNull)
            {
                _currentSceneAudio = RuntimeManager.CreateInstance(eventReference);
                _currentSceneAudio.start();
            }
=======
            // Early exit if the eventReference is null
            if (eventReference.IsNull) return;
            // Stop and existing scene audio
            StopSceneAudio();
            // Create an instance using the valid eventReference
            _currentSceneAudio = RuntimeManager.CreateInstance(eventReference);
            // Start the new instance
            _currentSceneAudio.start();
>>>>>>> Stashed changes:Trash-and-Treasure-Unity/Assets/Scripts/Managers/AudioManager.cs
        }

        public void StopSceneAudio()
        {
<<<<<<< Updated upstream:Trash-and-Treasure-Unity/Assets/Scripts/Audio/AudioManager.cs
            if (_currentSceneAudio.isValid())
            {
                _currentSceneAudio.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                _currentSceneAudio.release();
            }
=======
            // Early exit if the _currentSceneAudio is invalid
            if (!_currentSceneAudio.isValid()) return;
            // Stop the current scene audio
            _currentSceneAudio.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            // Release the instance
            _currentSceneAudio.release();
>>>>>>> Stashed changes:Trash-and-Treasure-Unity/Assets/Scripts/Managers/AudioManager.cs
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
<<<<<<< Updated upstream:Trash-and-Treasure-Unity/Assets/Scripts/Audio/AudioManager.cs
            if (!eventReference.IsNull)
            {
                EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
                eventInstance.setParameterByName(parameterName, parameterValue);
                eventInstance.start();
                eventInstance.release();
            }
        }

        // Play a one-shot sound effect using a string path (legacy)
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

        // Play sound at a specific position using a string path (legacy support)
        public void PlayOneShotAttached(string eventPath, GameObject attachedGameObject)
        {
            if (!string.IsNullOrEmpty(eventPath) && attachedGameObject != null)
            {
                RuntimeManager.PlayOneShotAttached(eventPath, attachedGameObject);
            }
        }

=======
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

>>>>>>> Stashed changes:Trash-and-Treasure-Unity/Assets/Scripts/Managers/AudioManager.cs
        // Set the volume for a specific bus
        private static void SetBusVolume(string busPath, float volume)
        {
            if (string.IsNullOrEmpty(busPath))
            {
                return;
            }

            Bus bus = RuntimeManager.GetBus(busPath);
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

        public void ResetPause()
        {
            SetGlobalParameter(PauseParameter, 0.0f);
        }
    }
}