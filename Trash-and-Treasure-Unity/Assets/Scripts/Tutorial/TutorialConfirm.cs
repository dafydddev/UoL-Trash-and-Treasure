using Gameplay;
using UI;
using UnityEngine;

namespace Tutorial
{
    public class TutorialConfirm : MonoBehaviour
    {
        [SerializeField] private UIController uiController;
        [SerializeField] private UIPanel tutorialPanel;

        public void OnClickTutorialCheck()
        {
            if (!GameEvents.GetHasCompletedTutorial())
            {
                if (tutorialPanel != null && uiController != null)
                {
                    uiController.ShowPanel(tutorialPanel);
                    return;
                }
            }
            uiController.NextScene();
        }
    }
}