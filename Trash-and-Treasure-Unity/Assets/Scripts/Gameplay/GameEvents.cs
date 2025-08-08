namespace Gameplay
{
    public static class GameEvents
    {
        public static bool IsPaused;
        public static System.Action OnGameStart;
        public static System.Action OnGameOver;
        public static System.Action<bool> OnPauseToggled;
    }
}