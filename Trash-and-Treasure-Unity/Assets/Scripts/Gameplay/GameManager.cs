using Audio;
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

            GameEvents.OnScoreChanged += HandleScore;
            GameEvents.OnLiveLost += HandleLiveLost;
            GameEvents.OnLiveGained += HandleLiveGained;
            GameEvents.OnPauseToggled += HandlePause;
            GameEvents.OnGameStart += HandleGameStart;
            GameEvents.OnGameOver += HandleGameOver;
            GameEvents.OnLevelComplete += HandleLevelComplete;
            GameEvents.OnTutorialComplete += HandleTutorialComplete;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDestroy()
        {
            GameEvents.OnScoreChanged -= HandleScore;
            GameEvents.OnLiveLost -= HandleLiveLost;
            GameEvents.OnLiveGained -= HandleLiveGained;
            GameEvents.OnPauseToggled -= HandlePause;
            GameEvents.OnGameStart -= HandleGameStart;
            GameEvents.OnGameOver -= HandleGameOver;
            GameEvents.OnLevelComplete -= HandleLevelComplete;
            GameEvents.OnTutorialComplete += HandleTutorialComplete;
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

        private static void HandlePause(bool isPaused)
        {
            Time.timeScale = isPaused ? 0f : 1f;
            GameEvents.SetIsPaused(isPaused);
        }

        private static void HandleLiveGained()
        {
            GameEvents.IncrementLives();
        }

        private static void HandleLiveLost()
        {
            GameEvents.DecrementLives();
        }

        private static void HandleScore(int score)
        {
            GameEvents.AddScore(score);
        }

        private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            GameEvents.SetIsPaused(false);
            GameEvents.SetGameInProgress(false);
            GameEvents.ResetScore();
            GameEvents.ResetLives();
            Time.timeScale = 1f;
            // If we're selected to move onto the next scene from the Main Menu, we've completed the tutorial
            if (!GameEvents.GetHasCompletedTutorial() && scene.name != "MainMenu")
            {
                GameEvents.SetHasCompletedTutorial(true);
            }

        }

        private static void HandleGameStart()
        {
            GameEvents.SetGameInProgress(true);
            AudioManager.Instance.PlayGameplayBackground();
        }

        private static void HandleLevelComplete()
        {
            Time.timeScale = 0f;
            AudioManager.Instance.StopSceneAudio();
            AudioManager.Instance.PlayGameStateJingle();
        }

        private static void HandleGameOver()
        {
            Time.timeScale = 0f;
            AudioManager.Instance.StopSceneAudio();
            AudioManager.Instance.PlayGameStateJingle();
        }

        private static void HandleTutorialComplete()
        {
            GameEvents.SetHasCompletedTutorial(true);
        }
    }
}