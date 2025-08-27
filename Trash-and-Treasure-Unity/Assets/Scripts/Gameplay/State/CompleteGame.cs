using Managers;
using UnityEngine;

namespace Gameplay.State
{
    public class CompleteGame : MonoBehaviour
    {
        // Hooked up with GUI last scene's button to trigger game completion
        public void TriggerCompleteEvent()
        {
            // Trigger the OnGameComplete event when called
            GameEvents.OnGameComplete?.Invoke();
        }
    }
}
