using UnityEngine;

namespace Gameplay
{
    public class ScoreCounter : MonoBehaviour
    {
        private const string LabelPrefix = "Score: ";
        
        private TMPro.TMP_Text _scoreText;

        private void Awake()
        {
            _scoreText = GetComponent<TMPro.TMP_Text>();
            _scoreText.text = LabelPrefix + GameEvents.GetScore().ToString("0");
            GameEvents.OnScoreChanged += HandleScore;
        }

        private void OnDestroy()
        {
            GameEvents.OnScoreChanged -= HandleScore;

        }

        private void HandleScore(int score)
        {
            _scoreText.text = LabelPrefix + GameEvents.GetScore().ToString("0");
        }
    }
}