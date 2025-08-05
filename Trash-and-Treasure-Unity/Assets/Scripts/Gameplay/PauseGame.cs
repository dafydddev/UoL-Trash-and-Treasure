using UnityEngine;

namespace Gameplay
{
    public class PauseGame : MonoBehaviour
    {
        public void Pause(bool pause)
        {
            Time.timeScale = pause ? 0f : 1f;
        }
    }
}