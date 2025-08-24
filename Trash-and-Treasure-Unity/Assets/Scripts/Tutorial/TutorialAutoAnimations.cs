using Managers;
using UnityEngine;

namespace Tutorial
{
    [RequireComponent(typeof(Animator))]
    public class TutorialAutoAnimations : MonoBehaviour
    {
        // Reference to the animator      
        private Animator _tutorialAnimator;
        // The trigger to end the autoplaying tutorial animation
        [SerializeField] private string tutorialAnimTrigger = "EndTutorial";

        private void Start()
        {
            // Get the animator (required by RequireComponent)      
            _tutorialAnimator = GetComponent<Animator>();
            // Subscribe to the GameEvents OnGameStart event
            GameEvents.OnGameStart += HandleGameStart;
        }

        private void OnDestroy()
        {
            // Unsubscribe from the GameEvents OnGameStart event
            GameEvents.OnGameStart -= HandleGameStart;
        }

        private void HandleGameStart()
        {
            // Trigger the tutorial animation using the string
            _tutorialAnimator.SetTrigger(tutorialAnimTrigger);
        }
    }
}