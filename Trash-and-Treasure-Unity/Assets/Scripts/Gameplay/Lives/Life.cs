using UnityEngine;

namespace Gameplay.Lives
{
    public class Life : MonoBehaviour
    {
        // Animator component for life UI animations
        [SerializeField] private Animator animator;

        // Animation clip to play when a life is lost
        [SerializeField] private AnimationClip lifeLostAnimationClip;

        // Play the life lost animation from the beginning
        public void OnLifeLostAnimation()
        {
            animator.Play(lifeLostAnimationClip.name, 0, 0f);
        }

        // Deactivate this life UI element (called by animation event)
        public void SwitchOff()
        {
            gameObject.SetActive(false);
        }
    }
}