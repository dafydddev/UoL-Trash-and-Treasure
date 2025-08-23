using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    [RequireComponent(typeof(Animator))]
    public class GameStartUI : MonoBehaviour
    {
        private Animator _animator;
        private string _animationTrigger = "GameStart";

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _animator.updateMode = AnimatorUpdateMode.UnscaledTime;
            SceneManager.sceneLoaded += HandleLevelLoad;
        }
        
        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= HandleLevelLoad;
        }
        
        private void HandleLevelLoad(Scene _, LoadSceneMode __)
        {
            _animator.SetTrigger(_animationTrigger);
        }
    
    }
}
