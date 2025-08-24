using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    [RequireComponent(typeof(Animator))]
    public class UIFadeScene : MonoBehaviour
    { 
        // Reference to the animator
        private Animator _animator;
        // Reference to the animation clip for fading the scene in and out
        [SerializeField] private AnimationClip fadeInAndOutClip;
    
        private void Start()
        {
            // Get the animator (required by RequireComponent)
            _animator = GetComponent<Animator>();
            // Subscribe to the SceneManager sceneLoaded event
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDestroy()
        {
            // Unsubscribe from the SceneManager sceneLoaded event
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene _, LoadSceneMode __)
        {
            // Early exit when we don't have the animator or the clip
            if (!_animator || !fadeInAndOutClip) return;
            // Tell the animator to play the clip, fading in the scene
            _animator.Play(fadeInAndOutClip.name, 0, 0f);
        }
    }
}