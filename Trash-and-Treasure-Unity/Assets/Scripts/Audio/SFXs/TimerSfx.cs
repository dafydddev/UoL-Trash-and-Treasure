using FMODUnity;
using Managers;
using UnityEngine;

namespace Audio.SFXs
{
    public class TimerSfx: MonoBehaviour
    {
        [SerializeField] private EventReference timerWarningSfx;

        public void PlayTimeWarningSfx()
        {
            // If we have an audio instance, play the one shot
            if (AudioManager.Instance)
            {
                AudioManager.PlayOneShot(timerWarningSfx);
            }
        }
        
    }
}