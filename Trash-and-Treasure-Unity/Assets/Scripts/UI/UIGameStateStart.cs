using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(Animator))]
    public class UIGameStateStart : MonoBehaviour
    {
        // Reference to the animator
        private Animator _animator;
        
        // Animator trigger name for game state transitions
        private const string AnimationTrigger = "GameStart";
        private readonly int _gameStart = Animator.StringToHash(AnimationTrigger);

        private void Start()
        {
            // Get the animator (required by RequireComponent)
            _animator = GetComponent<Animator>();
            // Set the animation to play in unscaled time (i.e. time is frozen when paused)
            _animator.updateMode = AnimatorUpdateMode.UnscaledTime;
            // Trigger the animation on start (autoplaying animation)
            _animator.SetTrigger(_gameStart);
        }
    }
}
