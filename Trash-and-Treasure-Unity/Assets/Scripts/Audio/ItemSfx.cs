using Gameplay;
using UnityEngine;
using FMODUnity;

namespace Audio
{
    [RequireComponent(typeof(Item))]
    public class ItemSfx : MonoBehaviour
    {
        [SerializeField] private EventReference boxedClick;
        [SerializeField] private EventReference unBoxedClick;
        [SerializeField] private EventReference boxCollision;
        [SerializeField] private string collisionIntensityParameter = "collisionIntensity";
        [SerializeField] private float collisionForceThreshold = 0.1f;
        [SerializeField] private float maxForce = 10.0f;

        private Item _item;

        private void Start()
        {
            _item = GetComponent<Item>();
            if (_item != null)
            {
                _item.OnUnboxed += PlayUnboxedSound;
                _item.OnClickedBoxed += PlayBoxedSound;
            }
        }

        private void OnDestroy()
        {
            if (_item != null)
            {
                _item.OnUnboxed -= PlayUnboxedSound;
                _item.OnClickedBoxed -= PlayBoxedSound;
            }
        }

        private void PlayBoxedSound()
        {
            if (AudioManager.Instance != null)
            {
                AudioManager.PlayOneShot(boxedClick);
            }
        }

        private void PlayUnboxedSound()
        {
            if (AudioManager.Instance != null)
            {
                AudioManager.PlayOneShot(unBoxedClick);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (AudioManager.Instance != null)
            {
                var collisionForce = collision.relativeVelocity.magnitude;
                if (collisionForce > collisionForceThreshold)
                {
                    float normalizedForce = collisionForce / maxForce;
                    AudioManager.PlayOneShot(boxCollision, collisionIntensityParameter, normalizedForce);
                }
            }
        }

    }
}