using Gameplay;
using UnityEngine;

namespace Tutorial
{
    public class TutorialTracker : MonoBehaviour
    {
        private void Start()
        {
            if (!GameEvents.GetHasCompletedTutorial())
            {
                GameEvents.OnTutorialComplete?.Invoke();
            }
        }
    }
}
