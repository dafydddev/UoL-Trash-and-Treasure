using UnityEngine;

namespace UI
{
    public class UIAnimationEvent : MonoBehaviour
    {
        [SerializeField] private UIController uiController;

        // Called by Animation Event
        public void OnTextAnimationComplete()
        {
            uiController.NextScene();
        }
    }
}
