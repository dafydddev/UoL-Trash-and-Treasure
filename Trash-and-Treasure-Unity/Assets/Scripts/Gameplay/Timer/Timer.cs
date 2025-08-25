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

        // Cache the last displayed time to avoid unnecessary text updates
        private int _lastDisplayedTime = -1;

        private void Awake()
        {
            // Initialise the timer with start time
            _time = startTime;
            // Get the text component for displaying the timer
            _timerText = GetComponent<TMPro.TMP_Text>();
            // Set the initial timer text display
            UpdateTimerText();
            // Set the initial colour to normal
            _timerText.color = normalColor;
            // Set the initial timer state
            _timerState = TimerState.Normal;
        }

        private void Update()
        {
            // Early exit when game over has been triggered, paused, or not in progress
            if (_gameOverTriggered || GameEvents.IsPaused() || !GameEvents.IsGameInProgress()) return;

            // Decrement the timer
            _time -= Time.deltaTime;

            // Check for game over
            if (_time <= 0)
            {
                // If the timer is less than or equal to zero, trigger the game over
                TriggerGameOver();
                return;
            }

            // Only update the UI when the displayed time actually changes
            var currentDisplayTime = Mathf.FloorToInt(_time);
            if (currentDisplayTime != _lastDisplayedTime)
            {
                UpdateTimerText();
                _lastDisplayedTime = currentDisplayTime;
            }

            // Update the timer colour (only changes when state transitions occur)
            UpdateTimerColor();
        }

        private void TriggerGameOver()
        {
            // Trigger the same over event
            GameEvents.OnGameOver?.Invoke();
            // Set the bool to true to allow scripts to early exit
            _gameOverTriggered = true;
        }

        private void UpdateTimerText()
        {
            // Update the label text
            _timerText.text = LabelPrefix + _time.ToString("0");
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