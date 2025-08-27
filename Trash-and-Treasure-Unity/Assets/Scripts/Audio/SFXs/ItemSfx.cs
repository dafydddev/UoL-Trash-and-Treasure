using FMODUnity;
using Gameplay.Items;
using Managers;
using UnityEngine;

namespace Audio.SFXs
{
    [RequireComponent(typeof(Item))]
    public class ItemSfx : MonoBehaviour
    {
        // FMOD Event References
        [SerializeField] private EventReference boxedClick;
        [SerializeField] private EventReference unBoxedClick;
        [SerializeField] private EventReference boxCollision;
        [SerializeField] private EventReference positiveBeep;

        [SerializeField] private EventReference negativeBeep;

        // FMOD Params
        [SerializeField] private string collisionIntensityParameter = "collisionIntensity";

        // Physics thresholds for playing sound effects
        [SerializeField] private float collisionForceThreshold = 0.1f;
        [SerializeField] private float maxForce = 10.0f;
        private Item _item;

        private void Start()
        {
            _item = GetComponent<Item>();
            // Early exit when we cannot access the item
            if (!_item) return;
            // Subscribe to the play events on the item 
            _item.OnUnboxed += PlayUnboxedSound;
            _item.OnClickedBoxed += PlayBoxedSound;
            _item.OnItemPositive += PlayPositiveSfx;
            _item.OnItemNegative += PlayNegativeSfx;
        }

        private void OnDestroy()
        {
            // Subscribe to the play events on the item 
            if (_item == null) return;
            // Unsubscribe from the play events on the item 
            _item.OnUnboxed -= PlayUnboxedSound;
            _item.OnClickedBoxed -= PlayBoxedSound;
            _item.OnItemPositive -= PlayPositiveSfx;
            _item.OnItemNegative -= PlayNegativeSfx;
        }

        private void PlayBoxedSound()
        {
            // If we have an audio instance, play the one shot
            if (AudioManager.Instance)
            {
                AudioManager.PlayOneShot(boxedClick);
            }
        }

        private void PlayUnboxedSound()
        {
            // If we have an audio instance, play the one shot
            if (AudioManager.Instance)
            {
                AudioManager.PlayOneShot(unBoxedClick);
            }
        }

        private void PlayPositiveSfx()
        {
            // If we have an audio instance, play the one shot
            if (AudioManager.Instance)
            {
                AudioManager.PlayOneShot(positiveBeep);
            }
        }

        private void PlayNegativeSfx()
        {
            // If we have an audio instance, play the one shot
            if (AudioManager.Instance)
            {
                AudioManager.PlayOneShot(negativeBeep);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            // Early exit when we cannot access the audio instance
            if (!AudioManager.Instance) return;
            var collisionForce = collision.relativeVelocity.magnitude;
            // Exit when the collisionForce is less than the collisionForceThreshold
            if (!(collisionForce > collisionForceThreshold)) return;
            var normalizedForce = collisionForce / maxForce;
            AudioManager.PlayOneShot(boxCollision, collisionIntensityParameter, normalizedForce);
        }
    }
}