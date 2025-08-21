using UnityEngine;

namespace Gameplay
{
    public static class GameEvents
    {
        private static bool _isPaused = false;
        private static bool _isGameInProgress;
        private static bool _hasCompletedTutorial;
        private static int _lives = MaxLives;
        private static int _currentScore;
        private const int MinLives = 0;
        private const int MaxLives = 3;

        public static System.Action OnGameStart;
        public static System.Action OnGameOver;
        public static System.Action<bool> OnPauseToggled;
        public static System.Action OnTutorialComplete;
        public static System.Action OnLevelComplete;
        public static System.Action OnLiveLost;
        public static System.Action OnLiveGained;
        public static System.Action<int> OnScoreChanged;

        public static void SetIsPaused(bool isPaused)
        {
            _isPaused = isPaused;
        }

        public static bool GetIsPaused()
        {
            return _isPaused;
        }

        public static void SetHasCompletedTutorial(bool hasCompletedTutorial)
        {
            _hasCompletedTutorial = hasCompletedTutorial;
        }

        public static bool GetHasCompletedTutorial()
        {
            return _hasCompletedTutorial;
        }

        public static void DecrementLives()
        {
            if (_lives >= MinLives)
            {
                _lives--;
            }
        }

        public static void IncrementLives()
        {
            if (_lives < MaxLives)
            {
                _lives++;
            }
        }

        public static int GetLives()
        {
            return _lives;
        }

        public static int GetMinLives()
        {
            return MinLives;
        }

        public static int GetScore()
        {
            return _currentScore;
        }
        
        public static void SetGameInProgress(bool isGameInProgress)
        {
            _isGameInProgress = isGameInProgress;
        }

        public static bool GetGameInProgress()
        {
            return _isGameInProgress;
        }

        public static void ResetScore()
        {
            _currentScore = 0;
        }

        public static void AddScore(int score)
        {
            _currentScore += score;
        }
    }
}