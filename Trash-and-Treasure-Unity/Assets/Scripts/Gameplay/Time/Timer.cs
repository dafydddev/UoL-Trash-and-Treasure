using UnityEngine;

namespace Gameplay
{
    [RequireComponent(typeof(TMPro.TMP_Text))]
    public class Timer : MonoBehaviour
    {
        [Header("Times")] [SerializeField] private float endTime;
        [SerializeField] private float startTime = 100f;

        [Header("Appearance")] [SerializeField]
        private Color normalColor = Color.white;

        [SerializeField] private Color warningColor = Color.yellow;
        [SerializeField] private Color criticalColor = Color.red;
        [SerializeField] private float warningThreshold = 30f;
        [SerializeField] private float criticalThreshold = 10f;

        private enum TimerState
        {
            Normal,
            Warning,
            Critical
        }

        private TimerState _timerState;

        private const string LabelPrefix = "Time: ";

        private float _time;

        private TMPro.TMP_Text _timerText;

        private bool GameOverTriggered = false;

        // Cache the last displayed time to avoid unnecessary text updates
        private int _lastDisplayedTime = -1;

        private void Awake()
        {
            _time = startTime;
            _timerText = GetComponent<TMPro.TMP_Text>();
<<<<<<< Updated upstream:Trash-and-Treasure-Unity/Assets/Scripts/Gameplay/Time/Timer.cs
            _timerText.text = LabelPrefix + _time.ToString("0");
=======
            // Set the initial timer text display
            UpdateTimerText();
            // Set the initial colour to normal
>>>>>>> Stashed changes:Trash-and-Treasure-Unity/Assets/Scripts/Gameplay/Timer/Timer.cs
            _timerText.color = normalColor;
            _timerState = TimerState.Normal;
        }

        private void Update()
        {
<<<<<<< Updated upstream:Trash-and-Treasure-Unity/Assets/Scripts/Gameplay/Time/Timer.cs
            if (!(_time > endTime) || GameEvents.GetIsPaused() || !GameEvents.GetGameInProgress())
            {
                return;
            }

            _time -= Time.deltaTime;
            _timerText.text = LabelPrefix + _time.ToString("0");
            UpdateTimerColor();
            if (_time <= 0 && !GameOverTriggered)
            {
                GameEvents.OnGameOver?.Invoke();
                GameOverTriggered = true;
=======
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
>>>>>>> Stashed changes:Trash-and-Treasure-Unity/Assets/Scripts/Gameplay/Timer/Timer.cs
            }
            
            // Only update UI when the displayed time actually changes
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
            // Set the bool to true, to allow script to early exit
            _gameOverTriggered = true;
        }

        private void UpdateTimerText()
        {
            // Update the label text
            _timerText.text = LabelPrefix + _time.ToString("0");
        }

        private void UpdateTimerColor()
        {
            if (_time <= criticalThreshold && _timerState != TimerState.Critical)
            {
                _timerText.color = criticalColor;
                _timerState = TimerState.Critical;
            }
            else if (_time <= warningThreshold && _time > criticalThreshold && _timerState != TimerState.Warning)
            {
                _timerText.color = warningColor;
                _timerState = TimerState.Warning;
            }
        }
    }
}