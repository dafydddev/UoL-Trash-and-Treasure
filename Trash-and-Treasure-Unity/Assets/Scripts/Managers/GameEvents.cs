namespace Managers
{
    public static class GameEvents
    {
        // For whether the game is paused
        private static bool _isPaused;

        // Flag for whether we have completed the tutorial 
        private static bool _hasCompletedTutorial;

        // Gameplay States
        private enum GameState
        {
            WaitingToStart,
            InProgress,
            LevelOver,
        }

        // Current game state
        private static GameState _gameState = GameState.WaitingToStart;

        // Gameplay Events
        public static System.Action OnGameStart;
        public static System.Action OnGameOver;
        public static System.Action<bool> OnPauseToggled;
        public static System.Action OnTutorialComplete;
        public static System.Action OnLevelComplete;
        public static System.Action OnLiveLost;
        public static System.Action<int> OnScoreChanged;

        // Score        
        private static int _currentScore;

        // Lives
        private static int _lives = MaxLives;
        private const int MinLives = 0;
        private const int MaxLives = 3;

        private static void SetGameState(GameState gameState)
        {
            // Update the current game state
            _gameState = gameState;
        }

        public static void SetWaitingToStart()
        {
            // Set the game state to waiting for the player to start
            SetGameState(GameState.WaitingToStart);
        }

        public static bool IsWaitingToStart()
        {
            // Return true if the game is waiting to start
            return _gameState == GameState.WaitingToStart;
        }

        public static void SetGameInProgress()
        {
            // Set the game state to in progress
            SetGameState(GameState.InProgress);
        }

        public static bool IsGameInProgress()
        {
            // Return true if the game is currently in progress
            return _gameState == GameState.InProgress;
        }

        public static void SetLevelOver()
        {
            // Set the game state to level over
            _gameState = GameState.LevelOver;
        }

        public static bool IsLevelOver()
        {
            // Return true if the level is over
            return _gameState == GameState.LevelOver;
        }

        public static void SetPaused(bool isPaused)
        {
            // Set the pause state
            _isPaused = isPaused;
        }

        public static bool IsPaused()
        {
            // Return the current pause state
            return _isPaused;
        }

        public static bool GetHasCompletedTutorial()
        {
            // Return whether the tutorial has been completed
            return _hasCompletedTutorial;
        }

        public static void SetHasCompletedTutorial()
        {
            // Mark the tutorial as completed
            _hasCompletedTutorial = true;
        }

        public static void DecrementLives()
        {
            // Decrement lives if above the minimum
            if (_lives >= MinLives)
            {
                _lives--;
            }
        }

        public static void IncrementLives()
        {
            // Increment lives if below maximum
            if (_lives < MaxLives)
            {
                _lives++;
            }
        }

        public static int GetLives()
        {
            // Return the current lives count
            return _lives;
        }

        public static int GetMinLives()
        {
            // Return the minimum lives value
            return MinLives;
        }

        public static int GetScore()
        {
            // Return the current score
            return _currentScore;
        }

        public static void ResetScore()
        {
            // Reset the score to zero
            _currentScore = 0;
        }

        public static void ResetLives()
        {
            // Reset lives to the maximum
            _lives = MaxLives;
        }

        public static void AddScore(int score)
        {
            // Add the score to the current total
            _currentScore += score;
        }
    }
}