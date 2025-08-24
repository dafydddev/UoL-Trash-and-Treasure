using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        // GameManager singleton instance
        private static GameManager _instance;

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

            // Subscribe to the GameEvents
            GameEvents.OnScoreChanged += HandleScore;
            GameEvents.OnLiveLost += HandleLiveLost;
            GameEvents.OnPauseToggled += HandlePause;
            GameEvents.OnGameStart += HandleGameStart;
            GameEvents.OnGameOver += HandleGameOver;
            GameEvents.OnLevelComplete += HandleLevelComplete;
            GameEvents.OnTutorialComplete += HandleTutorialComplete;
            // Subscribe to the SceneManager sceneLoaded event
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDestroy()
        {
            // Unsubscribe from the GameEvents
            GameEvents.OnScoreChanged -= HandleScore;
            GameEvents.OnLiveLost -= HandleLiveLost;
            GameEvents.OnPauseToggled -= HandlePause;
            GameEvents.OnGameStart -= HandleGameStart;
            GameEvents.OnGameOver -= HandleGameOver;
            GameEvents.OnLevelComplete -= HandleLevelComplete;
            GameEvents.OnTutorialComplete -= HandleTutorialComplete;
            // Unsubscribe from the SceneManager sceneLoaded event
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void Update()
        {
            // Early exit if the user is not pressing escape
            if (!Input.GetKeyDown(KeyCode.Escape)) return;
            // Set the newPauseState to inverse of the current IsPaused
            var newPauseState = !GameEvents.IsPaused();
            // Invoke the GameEvents OnPauseToggled event with the new state
            GameEvents.OnPauseToggled?.Invoke(newPauseState);
        }

        private static void HandlePause(bool isPaused)
        {
            // If we are paused, set the timescale to 0
            // If we are *NOT* paused, set the timescale to 1
            Time.timeScale = isPaused ? 0f : 1f;
            // Set the paused state of the game
            GameEvents.SetPaused(isPaused);
        }
        
        private static void HandleLiveLost()
        {
            // Decrement the lives when a life is lost
            GameEvents.DecrementLives();
        }

        private static void HandleScore(int score)
        {
            // Add the score to the current total
            GameEvents.AddScore(score);
        }

        private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            // Set the game state to waiting for the player to start
            GameEvents.SetWaitingToStart();
            // Reset the pause flag
            GameEvents.SetPaused(false);
            // Flush any cached score 
            GameEvents.ResetScore();
            // Flush any cached lives
            GameEvents.ResetLives();
            Time.timeScale = 1f;
            // If we have (somehow) got into the game without completing the tutorial, flag it is completed
            if (!GameEvents.GetHasCompletedTutorial() && scene.name != "MainMenu")
            {
                GameEvents.SetHasCompletedTutorial();
            }

        }

        private static void HandleGameStart()
        {
            // Set the game state to in progress
            GameEvents.SetGameInProgress();
            // Play the gameplay background music
            AudioManager.Instance.PlayGameplayBackground();
        }

        private static void HandleLevelComplete()
        {
            // Pause the game by setting timescale to 0
            Time.timeScale = 0f;
            // Stop any scene audio that is playing
            AudioManager.Instance.StopSceneAudio();
            // Play the game state jingle for level completion
            AudioManager.Instance.PlayGameStateJingle();
            // Set the game state to level over
            GameEvents.SetLevelOver();
        }

        private static void HandleGameOver()
        {
            // Pause the game by setting timescale to 0
            Time.timeScale = 0f;
            // Stop any scene audio that is playing
            AudioManager.Instance.StopSceneAudio();
            // Play the game state jingle for game over
            AudioManager.Instance.PlayGameStateJingle();
            // Set the game state to level over
            GameEvents.SetLevelOver();
        }

        private static void HandleTutorialComplete()
        {
            // Set the tutorial as completed in the game state
            GameEvents.SetHasCompletedTutorial();
        }
    }
}