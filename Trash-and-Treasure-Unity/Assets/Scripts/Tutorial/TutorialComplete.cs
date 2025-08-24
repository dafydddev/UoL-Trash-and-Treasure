using Managers;
using UnityEngine;

namespace Tutorial
{
    public class TutorialComplete : MonoBehaviour
    {
        private void Start()
        {
            // If the user has not already completed the tutorial, trigger the event
            if (!GameEvents.GetHasCompletedTutorial())
            {
                GameEvents.OnTutorialComplete?.Invoke();
            }
        }
    }
}