using Managers;
using UnityEngine;

namespace Gameplay.Lives
{
    public class LifeCounter : MonoBehaviour
    {
        // Individual life UI components
        [SerializeField] private Life firstLife;
        [SerializeField] private Life secondLife;
        [SerializeField] private Life thirdLife;
        
        // Array of all life components for easy access
        private Life[] _lives;
        
        // The current number of lives remaining
        private int _livesCount;

        private void Start()
        {
            // Initialise the lives array with the life components
            _lives = new[] { firstLife, secondLife, thirdLife };
            // Get the initial lives count from game events
            _livesCount = GameEvents.GetLives();
        }

        private void Awake()
        {
            // Subscribe to life lost events
            GameEvents.OnLiveLost += HandleLives;
        }

        private void OnDestroy()
        {
            // Unsubscribe from life lost events
            GameEvents.OnLiveLost -= HandleLives;
        }

        private void HandleLives()
        {
            // Calculate which life UI element to disable (rightmost first)
            var lifeToSwitchOffIndex = _livesCount - 1;
            // Decrement the lives count
            _livesCount--;
            
            // Trigger life lost animation if valid index
            if (lifeToSwitchOffIndex >= 0)
            {
                _lives[lifeToSwitchOffIndex].OnLifeLostAnimation();
            }
            
            // Check if game over condition is met
            if (_livesCount < GameEvents.GetMinLives())
            {
                GameEvents.OnGameOver?.Invoke();
            }
        }
    }
}