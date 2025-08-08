using UnityEngine;

namespace UI
{
    public class TutorialConfirm : MonoBehaviour
    {
        [SerializeField] 
        private UIController uiController;
        [SerializeField] 
        private UIPanel tutorialPanel;
        
        public void OnClickTutorialCheck()
        {
            if (tutorialPanel != null && uiController != null)
            {
                uiController.ShowPanel(tutorialPanel);
                return;
            }
            uiController.NextScene();
        }
    }
}
