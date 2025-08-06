using UnityEngine;
using UnityEngine.SceneManagement;

namespace Gameplay
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager _instance;
        
        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
            GameEvents.OnPauseChanged += HandlePause;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        
        private void OnDestroy()
        {
            GameEvents.OnPauseChanged -= HandlePause;
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GameEvents.OnPauseChanged?.Invoke(!GameEvents.IsPaused);
            }
        }
        
        private void HandlePause(bool isPaused)
        {
            Time.timeScale = isPaused ? 0f : 1f;
            GameEvents.IsPaused = !GameEvents.IsPaused;
        }

        private void OnSceneLoaded(Scene _, LoadSceneMode __)
        {
            GameEvents.IsPaused = false;
            Time.timeScale = 1f;
        }
    }
}
