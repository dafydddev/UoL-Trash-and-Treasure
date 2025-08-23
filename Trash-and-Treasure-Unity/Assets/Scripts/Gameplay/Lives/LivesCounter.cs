using UnityEngine;

namespace Gameplay
{
    public class LivesCounter : MonoBehaviour
    {
        [SerializeField] private Life firstLife;
        [SerializeField] private Life secondLife;
        [SerializeField] private Life thirdLife;
        private Life[] _lives;
        private int _livesCount;

        private void Start()
        {
            _lives = new[] { firstLife, secondLife, thirdLife };
            _livesCount = GameEvents.GetLives();
        }

        private void Awake()
        {
            GameEvents.OnLiveLost += HandleLives;
        }

        private void OnDestroy()
        {
            GameEvents.OnLiveLost -= HandleLives;
        }

        private void HandleLives()
        {
            int lifeToSwitchOffIndex = _livesCount - 1;
            _livesCount--;
            if (lifeToSwitchOffIndex >= 0)
            {
                _lives[lifeToSwitchOffIndex].OnLifeLostAnimation();
            }
            if (_livesCount < GameEvents.GetMinLives())
            {
                GameEvents.OnGameOver?.Invoke();
            }
        }
    }
}