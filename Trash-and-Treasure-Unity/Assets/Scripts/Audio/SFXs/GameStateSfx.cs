using System;
using FMODUnity;
using Gameplay;
using UnityEngine;

namespace Audio
{
    public class GameStateSfx : MonoBehaviour
    {
        [SerializeField] private EventReference readySoundEvent;
        [SerializeField] private EventReference goSoundEvent;

        private void Start()
        {
            GameEvents.OnGameStart += PlayGoSfx;
        }
        
        private void OnDestroy()
        {
            GameEvents.OnGameStart -= PlayGoSfx;
        }

        private void PlayReadySfx()
        {
            if (AudioManager.Instance != null)
            {
                AudioManager.PlayOneShot(readySoundEvent);
            }
        }

        private void PlayGoSfx()
        {
            if (AudioManager.Instance != null)
            {
                AudioManager.PlayOneShot(goSoundEvent);
            }
        }
    }
}
