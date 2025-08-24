using Managers;
using TMPro;
using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(Animator))]
    public class UIGameStateEnd : MonoBehaviour
    {
        // UI buttons for different actions
        [SerializeField] private GameObject retryButton;
        [SerializeField] private GameObject nextButton;
        [SerializeField] private GameObject mainMenuButton;

        // Text components for displaying game state and quotes
        [SerializeField] private TMP_Text gameStateText;
        [SerializeField] private TMP_Text quoteText;

        // Arrays of quotes to display for different game states
        [SerializeField] private string[] levelWinQuotes;

        [SerializeField] private string[] levelLoseQuotes;

        // Strings to display at the end of the level
        [SerializeField] private string levelWinString = "Level Complete";
        [SerializeField] private string levelLoseString = "Game Over";

        // Reference to the animator
        private Animator _animator;

        // Animator trigger name for game state transitions
        private const string AnimationTrigger = "GameOver";
        private readonly int _gameStart = Animator.StringToHash(AnimationTrigger);

        private void Start()
        {
            // Get the animator (required by RequireComponent)
            _animator = GetComponent<Animator>();
            // Set the animation to play in unscaled time (i.e. time is frozen when the level ends)
            _animator.updateMode = AnimatorUpdateMode.UnscaledTime;
            // Subscribe to the OnLevelComplete and OnGameOver events
            GameEvents.OnLevelComplete += HandleLevelComplete;
            GameEvents.OnGameOver += HandleGameOver;
        }

        private void OnDestroy()
        {
            // Unsubscribe from the OnLevelComplete and OnGameOver events
            GameEvents.OnLevelComplete -= HandleLevelComplete;
            GameEvents.OnGameOver -= HandleGameOver;
        }

        private void HandleLevelComplete()
        {
            // Set the game as not in progress and update UI for level complete state
            if (gameStateText)
            {
                gameStateText.text = levelWinString;
            }
            // Get a quote to show on the screen
            quoteText.text = GetRandomQuote(levelWinQuotes);
            // Trigger the animation 
            _animator.SetTrigger(_gameStart);
            // Configure buttons for level complete state
            nextButton.SetActive(true);
            mainMenuButton.SetActive(true);
            retryButton.SetActive(false);
        }


        private void HandleGameOver()
        {
            if (gameStateText)
            {
                gameStateText.text = levelLoseString;
            }
            // Get a quote to show on the screen
            quoteText.text = GetRandomQuote(levelLoseQuotes);
            // Trigger the animation 
            _animator.SetTrigger(_gameStart);
            // Configure buttons for game over state
            nextButton.SetActive(false);
            mainMenuButton.SetActive(true);
            retryButton.SetActive(true);
        }

        private static string GetRandomQuote(string[] quotes)
        {
            // Return an empty string if no quotes, otherwise pick a random one
            return quotes.Length == 0 ? "" : quotes[Random.Range(0, quotes.Length)];
        }
    }
}