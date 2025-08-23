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

        private void Awake()
        {
            _time = startTime;
            _timerText = GetComponent<TMPro.TMP_Text>();
            _timerText.text = LabelPrefix + _time.ToString("0");
            _timerText.color = normalColor;
            _timerState = TimerState.Normal;
        }

        private void Update()
        {
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
            }
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