using Managers;
using UnityEngine;

namespace Gameplay.Score
{
    public class ScoreCounter : MonoBehaviour
    {
        // Label prefix for the score display
        private const string LabelPrefix = "Score: ";
        
        // Text component for displaying the score
        private TMPro.TMP_Text _scoreText;

        private void Awake()
        {
            // Get the text component for displaying the score
            _scoreText = GetComponent<TMPro.TMP_Text>();
            // Initialise the score display with the current score
            _scoreText.text = LabelPrefix + GameEvents.GetScore().ToString("0");
            // Subscribe to score changed events
            GameEvents.OnScoreChanged += HandleScore;
        }

        private void OnDestroy()
        {
            // Unsubscribe from score changed events
            GameEvents.OnScoreChanged -= HandleScore;
        }

        private void HandleScore(int score)
        {
            // Update the score display when score changes
            _scoreText.text = LabelPrefix + GameEvents.GetScore().ToString("0");
        }
    }
}