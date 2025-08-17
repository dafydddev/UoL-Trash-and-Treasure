using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    [RequireComponent(typeof(Animator))]
    public class FadeInImage : MonoBehaviour
    { 
        private Animator _animator;
        [SerializeField] private AnimationClip fadeInAndOutClip;
    
        private void Start()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            _animator = GetComponent<Animator>();
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene _, LoadSceneMode __)
        {
            _animator.Play(fadeInAndOutClip.name, 0, 0f);
        }
    }
}