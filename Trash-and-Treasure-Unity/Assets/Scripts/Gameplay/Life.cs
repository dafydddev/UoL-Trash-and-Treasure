using UnityEngine;

namespace Gameplay
{
    public class Life : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private AnimationClip lifeLostAnimationClip;

        public void OnLifeLostAnimation()
        {
            animator.Play(lifeLostAnimationClip.name, 0, 0f);
        }
        
        public void SwitchOff()
        {
            gameObject.SetActive(false);
        }
    }
}
