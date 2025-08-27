using Managers;
using UI;
using UnityEngine;

namespace Tutorial
{
    public class TutorialCheckPoint : MonoBehaviour
    {
        // Reference to the UI controller in the scene
        [SerializeField] private UIController uiController;
        // Reference to the tutorial panel in the scene
        [SerializeField] private UIPanel tutorialPanel;
        // Bool for whether users are forced to access the tutorial
        [SerializeField] private bool forceTutorial = false;
        
        public void OnClickTutorialCheck()
        {
            // If the tutorial has not been completed, show the tutorial panel
            if (!GameEvents.GetHasCompletedTutorial() && forceTutorial)
            {
                if (tutorialPanel != null && uiController != null)
                {
                    // Delegate to the UI controller to show the tutorial panel
                    uiController.ShowPanel(tutorialPanel);
                    // Trigger the tutorial completion event
                    GameEvents.OnTutorialComplete?.Invoke();
                    return;
                }
            }
            // Otherwise, load the next scene
            uiController.NextScene();
        }
    }
}