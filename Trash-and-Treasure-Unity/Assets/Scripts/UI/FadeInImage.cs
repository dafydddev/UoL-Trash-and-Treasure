using UnityEngine;
using UnityEngine.SceneManagement;


[RequireComponent(typeof(Animator))]
public class FadeInImage : MonoBehaviour
{ 
    private Animator _animator;
    [SerializeField] private AnimationClip fadeInAndOutClip;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
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
        Debug.Log("Scene loaded");
        _animator.Play(fadeInAndOutClip.name, 0, 0f);
    }
}