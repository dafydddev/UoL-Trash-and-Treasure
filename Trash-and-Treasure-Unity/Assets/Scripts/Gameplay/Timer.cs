using UnityEngine;

namespace Gameplay
{
    [RequireComponent(typeof(TMPro.TMP_Text))]
    public class Timer : MonoBehaviour
    {
        [Header("Times")] [SerializeField] private float endTime;
        [SerializeField] private float startTime = 100;

        private const string LabelPrefix = "Time: ";

        private float _time;

        private TMPro.TMP_Text _timerText;

        private void Awake()
        {
            _time = startTime;
            _timerText = GetComponent<TMPro.TMP_Text>();
            _timerText.text = LabelPrefix + _time.ToString("0");
        }

        private void Update()
        {
            if (!(_time > endTime) || GameEvents.GetIsPaused() || !GameEvents.GetGameInProgress())
            {
                return;
            }

            _time -= Time.deltaTime;
            _timerText.text = LabelPrefix + _time.ToString("0");
        }
    }
}