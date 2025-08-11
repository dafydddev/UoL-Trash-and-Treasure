using UnityEngine;

namespace Gameplay
{
    public static class GameEvents
    {
        private static bool _isPaused;
        private static bool _hasCompletedTutorial;
        private static int _lives = MaxLives;
        private const int MaxLives = 3;
        private static int _currentScore;

        public static System.Action OnGameStart;
        public static System.Action OnGameOver;
        public static System.Action<bool> OnPauseToggled;
        public static System.Action OnTutorialComplete;
        public static System.Action OnLevelComplete;
        public static System.Action OnLiveLost;
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
            if (_lives <= 0)
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

        public static int GetScore()
        {
            return _currentScore;
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