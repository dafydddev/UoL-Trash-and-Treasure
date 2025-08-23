using Gameplay;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI
{
    [RequireComponent(typeof(Animator))]
    public class GameEndUI : MonoBehaviour
    {
        [SerializeField] private GameObject retryButton; 
        [SerializeField] private GameObject nextButton; 
        [SerializeField] private GameObject mainMenuButton;
        
        [SerializeField] private string gameStateTrigger = "GameOver";
        [SerializeField] private TMP_Text gameStateText; 
        [SerializeField] private TMP_Text quoteText;
        [SerializeField] private string[] levelWinQuotes; 
        [SerializeField] private string[] levelLoseQuotes;
        private Animator _animator;
        
        private void Start()
        {
            _animator = GetComponent<Animator>();
            _animator.updateMode = AnimatorUpdateMode.UnscaledTime;
            GameEvents.OnGameOver += HandleGameOver;
            GameEvents.OnLevelComplete += HandleLevelComplete;
        }

        private void OnDestroy()
        {
            GameEvents.OnGameOver -= HandleGameOver;   
            GameEvents.OnLevelComplete -= HandleLevelComplete;
        }

        private void HandleGameOver()
        {
            GameEvents.SetGameInProgress(false);
            if (gameStateText)
            {
                gameStateText.text = "Game Over";
            }
            quoteText.text = GetRandomQuote(levelLoseQuotes);
            _animator.SetTrigger(gameStateTrigger);
            nextButton.SetActive(false);
            mainMenuButton.SetActive(true);
            retryButton.SetActive(true);       
        }
        
        private void HandleLevelComplete()
        {
            GameEvents.SetGameInProgress(false);
            if (gameStateText)
            {
                gameStateText.text = "Level Complete";
            }
            quoteText.text = GetRandomQuote(levelWinQuotes);
            _animator.SetTrigger(gameStateTrigger);
            nextButton.SetActive(true);
            mainMenuButton.SetActive(true);
            retryButton.SetActive(false);       
        }

        private string GetRandomQuote(string[] quotes)
        {
            return quotes.Length == 0 ? "" : quotes[Random.Range(0, quotes.Length)];
        }
    }
}
