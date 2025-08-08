using UnityEngine;
using UnityEngine.SceneManagement;

namespace Gameplay
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager _instance;
        
        [SerializeField]
        private Animator sceneTransitionAnimator;
        private readonly string _fadeInTrigger = "FadeIn";
        
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
            GameEvents.OnPauseToggled += HandlePause;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        
        private void OnDestroy()
        {
            GameEvents.OnPauseToggled -= HandlePause;
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                bool newPauseState = !GameEvents.GetIsPaused();
                GameEvents.OnPauseToggled?.Invoke(newPauseState);
            }
        }
        
        private void HandlePause(bool isPaused)
        {
            Time.timeScale = isPaused ? 0f : 1f; 
            GameEvents.SetIsPaused(isPaused);
        }

        private void OnSceneLoaded(Scene _, LoadSceneMode __)
        {
            GameEvents.SetIsPaused(false);
            Time.timeScale = 1f;
            sceneTransitionAnimator.SetTrigger(_fadeInTrigger);
        }
    }
}
