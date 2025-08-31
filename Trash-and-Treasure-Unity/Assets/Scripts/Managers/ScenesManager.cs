using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class ScenesManager : MonoBehaviour
    {
        // ScenesManager singleton instance
        private static ScenesManager _instance;

        private void Awake()
        {
            // Singleton pattern implementation
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
            // Subscribe to the SceneManager sceneLoaded event
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        
        private void OnDestroy()
        {
            // Unsubscribe from the SceneManager sceneLoaded event
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        public static void ReloadCurrentScene()
        {
            // Reload whatever the current scene is
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public static void LoadSceneByReference(SceneReference sceneRef)
        {
            // Log an error if we have attempted to load a scene without a valid reference
            if (sceneRef == null || string.IsNullOrEmpty(sceneRef.SceneName))
            {
                Debug.LogError("SceneReference is null or missing name.");
                return;
            }

            // Delegate to the SceneManager to load the scene
            SceneManager.LoadScene(sceneRef.SceneName);
        }

        private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name != "MainMenu")
            {
                // If we are not in the main menu, stop the scene audio
                // We will later trigger it to start when the player starts the gameplay
                AudioManager.Instance.StopSceneAudio();
            }
            else
            {
                // If we are in the main menu, play the main menu background music
                AudioManager.Instance.ResetPauseParameter();
                AudioManager.Instance.PlayMainMenuBackground();
            }
        }
    }
}