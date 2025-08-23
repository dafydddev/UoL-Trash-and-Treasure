using Gameplay;
using UnityEngine;

namespace Tutorial
{
    public class TutorialAnimations : MonoBehaviour
    {
        [SerializeField] private Animator tutorialAnimator;
        [SerializeField] private string tutorialAnimTrigger = "EndTutorial";

        private void Start()
        {
            GameEvents.OnGameStart += HandleGameStart;
        }

        private void OnDestroy()
        {
            GameEvents.OnGameStart -= HandleGameStart;
        }

        private void HandleGameStart()
        {
            tutorialAnimator.SetTrigger(tutorialAnimTrigger);
        }
    }
}