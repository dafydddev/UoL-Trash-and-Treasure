using FMODUnity;
using Gameplay;
using Managers;
using UnityEngine;

namespace Audio.SFXs
{
    public class GameStateSfx : MonoBehaviour
    {
        // FMOD Event References
        [SerializeField] private EventReference readySoundEvent;
        [SerializeField] private EventReference goSoundEvent;

        private void Start()
        {
            // Subscribe to the game start event
            GameEvents.OnGameStart += PlayGoSfx;
        }
        
        private void OnDestroy()
        {
            // Unsubscribe from the game start event
            GameEvents.OnGameStart -= PlayGoSfx;
        }

        private void PlayReadySfx()
        {
            // If we have an audio instance, play the one shot
            if (AudioManager.Instance)
            {
                AudioManager.PlayOneShot(readySoundEvent);
            }
        }

        private void PlayGoSfx()
        {
            // If we have an audio instance, play the one shot
            if (AudioManager.Instance)
            {
                AudioManager.PlayOneShot(goSoundEvent);
            }
        }
    }
}
