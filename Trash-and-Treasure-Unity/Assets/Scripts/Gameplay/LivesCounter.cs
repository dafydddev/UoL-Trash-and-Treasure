using UnityEngine;

namespace Gameplay
{
    public class LivesCounter : MonoBehaviour
    {
        [SerializeField] private Life firstLife;
        [SerializeField] private Life secondLife;
        [SerializeField] private Life thirdLife;
        private Life[] _lives;

        private void Start()
        {
            _lives = new[] { firstLife, secondLife, thirdLife };
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
            int lifeToSwitchOffIndex = GameEvents.GetLives() - 1;
            _lives[lifeToSwitchOffIndex].OnLifeLostAnimation();
        }
    }
}