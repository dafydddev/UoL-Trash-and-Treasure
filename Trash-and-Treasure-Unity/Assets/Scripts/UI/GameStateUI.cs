using Gameplay;
using TMPro;
using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(Animator))]
    public class GameStateUI : MonoBehaviour
    {
        [SerializeField] private string gameOverTrigger = "GameOver";
        [SerializeField] private TMP_Text gameStateText;
        private Animator _animator;
        private void Start()
        {
            _animator = GetComponent<Animator>();
            _animator.updateMode = AnimatorUpdateMode.UnscaledTime;
            GameEvents.OnGameOver += HandleGameOver;
        }

        private void OnDestroy()
        {
            GameEvents.OnGameOver -= HandleGameOver;       
        }

        private void HandleGameOver()
        {
            gameStateText.text = "Game Over";
            _animator.SetTrigger(gameOverTrigger);
        }
    }
}
