using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    [RequireComponent(typeof(Animator))]
    public class UIClickToTriggerAnimation : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private string animationTrigger = "UIClicked";
        private int _uiClicked;
        private Animator _animator;

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _uiClicked = Animator.StringToHash(animationTrigger);
        }

        public void OnPointerClick(PointerEventData _)
        {
            _animator.SetTrigger(_uiClicked);
        }
    }
}