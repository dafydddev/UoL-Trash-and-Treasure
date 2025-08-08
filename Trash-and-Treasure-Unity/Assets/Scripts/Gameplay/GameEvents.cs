namespace Gameplay
{
    public static class GameEvents
    {
        private static bool _isPaused;
        private static bool _hasCompletedTutorial;

        public static System.Action OnGameStart;
        public static System.Action OnGameOver;
        public static System.Action<bool> OnPauseToggled;
        public static System.Action OnTutorialComplete;

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
    }
}