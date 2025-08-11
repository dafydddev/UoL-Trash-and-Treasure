using System;
using UnityEngine;

namespace Gameplay
{
    public class LivesCounter : MonoBehaviour
    {
        private const string LabelPrefix = "Score: ";

        [SerializeField] private GameObject firstLife;
        [SerializeField] private GameObject secondLife;
        [SerializeField] private GameObject thirdLife;
        private GameObject[] _lives;

        private void Start()
        {
            _lives = new[] {firstLife, secondLife, thirdLife};
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
            _lives[lifeToSwitchOffIndex].SetActive(false);
        }
    }
}