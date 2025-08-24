using UnityEngine;

namespace UI
{
    // Allows animations to call events on the UI controller
    public class UIAnimationEvents : MonoBehaviour
    {
        // Reference to the UI controller in the scene
        [SerializeField] private UIController uiController;
        
        public void AutoLoadNextScene()
        {
            // Load the next scene by delegating to the UIController at the end of an animation
            uiController.NextScene();
        }
    }
}
