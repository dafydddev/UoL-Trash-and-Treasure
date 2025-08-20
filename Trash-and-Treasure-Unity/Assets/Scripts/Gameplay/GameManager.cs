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

        private void OnSceneLoaded(Scene _, LoadSceneMode __)
        {
            GameEvents.SetIsPaused(false);
            GameEvents.SetGameInProgress(false);
            AudioManager.Instance.ResetPause();
            Time.timeScale = 1f;
        }

        private void HandleGameStart()
        {
            GameEvents.SetGameInProgress(true);
        }

        private void HandleGameOver()
        {
            return;
        }
    }
}