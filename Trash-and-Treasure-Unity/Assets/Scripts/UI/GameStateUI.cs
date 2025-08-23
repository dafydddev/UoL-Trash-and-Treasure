using Gameplay;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI
{
    [RequireComponent(typeof(Animator))]
    public class GameStateUI : MonoBehaviour
    {
        [SerializeField] private GameObject retryButton; 
        [SerializeField] private GameObject nextButton; 
        [SerializeField] private GameObject mainMenuButton;
        
        [SerializeField] private string gameStateTrigger = "GameOver";
        [SerializeField] private TMP_Text gameStateText;
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
            gameStateText.text = "Game Over";
            _animator.SetTrigger(gameStateTrigger);
            nextButton.SetActive(false);
            mainMenuButton.SetActive(true);
            retryButton.SetActive(true);       
        }
        
        private void HandleLevelComplete()
        {
            GameEvents.SetGameInProgress(false);
            gameStateText.text = "Level Complete";
            _animator.SetTrigger(gameStateTrigger);
            nextButton.SetActive(true);
            mainMenuButton.SetActive(true);
            retryButton.SetActive(false);       
        }
    }
}
