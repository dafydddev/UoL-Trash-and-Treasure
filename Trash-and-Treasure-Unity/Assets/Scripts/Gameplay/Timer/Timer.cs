using Managers;
using UnityEngine;

namespace Gameplay.Timer
{
    [RequireComponent(typeof(TMPro.TMP_Text))]
    public class Timer : MonoBehaviour
    {
        [SerializeField] private float endTime;
        // Start time for the timer
        [SerializeField] private float startTime = 100f;
        
        // Colour to display when the time is in the normal state
        [SerializeField] private Color normalColor = Color.white;
        // Colour to display when timer enters warning state
        [SerializeField] private Color warningColor = Color.yellow;
        // Colour to display when timer enters critical state
        [SerializeField] private Color criticalColor = Color.red;
        // Time threshold for warning state
        [SerializeField] private float warningThreshold = 30f;
        // Time threshold for critical state
        [SerializeField] private float criticalThreshold = 10f;

        // Timer states for colour changes
        private enum TimerState
        {
            Normal,
            Warning,
            Critical
        }

        // Current timer state
        private TimerState _timerState;

        // Label prefix for the timer display
        private const string LabelPrefix = "Time: ";

        // Current time remaining
        private float _time;

        // Text component for displaying the timer
        private TMPro.TMP_Text _timerText;

        // Flag to prevent multiple game over triggers
        private bool _gameOverTriggered;

        private void Awake()
        {
            // Initialise the timer with start time
            _time = startTime;
            // Get the text component for displaying the timer
            _timerText = GetComponent<TMPro.TMP_Text>();
            // Set the initial timer text display
            _timerText.text = LabelPrefix + _time.ToString("0");
            // Set the initial colour to normal
            _timerText.color = normalColor;
            // Set the initial timer state
            _timerState = TimerState.Normal;
        }

        private void Update()
        {
            // Early exit when the time is over, the game is paused, or the game is not in progress
            if (!(_time > endTime) || GameEvents.IsPaused() || !GameEvents.IsGameInProgress()) return;
            // Decrement the timer
            _time -= Time.deltaTime;
            // Set the timer text
            _timerText.text = LabelPrefix + _time.ToString("0");
            // Update the timer colour
            UpdateTimerColor();
            // Trigger the game over event if the timer reaches 0
            if (_time <= 0 && !_gameOverTriggered)
            {
                GameEvents.OnGameOver?.Invoke();
                _gameOverTriggered = true;
            }
        }

        private void UpdateTimerColor()
        {
            // Change to critical colour when time is critically low
            if (_time <= criticalThreshold && _timerState != TimerState.Critical)
            {
                _timerText.color = criticalColor;
                _timerState = TimerState.Critical;
            }
            // Change to warning colour when time is getting low
            else if (_time <= warningThreshold && _time > criticalThreshold && _timerState != TimerState.Warning)
            {
                _timerText.color = warningColor;
                _timerState = TimerState.Warning;
            }
        }
    }
}