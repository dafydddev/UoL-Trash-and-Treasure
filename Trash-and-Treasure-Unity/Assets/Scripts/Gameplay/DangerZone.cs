using UnityEngine;

namespace Gameplay
{
    public class DangerZone : MonoBehaviour
    {
        private int _itemsInDanger = 0;
        private bool _isActivated = false;
        private Animator _animator;
        [SerializeField] private string warningTrigger = "WarningActive";

        private void Start()
        {
            _animator = GetComponent<Animator>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            _itemsInDanger++;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (_itemsInDanger <= 0) return;
            _itemsInDanger--;
        }

        private void Update()
        {
            if (_itemsInDanger > 0 && !_isActivated)
            {
                _isActivated = true;
                _animator.SetTrigger(warningTrigger);
            }
            else
            {
                _isActivated = false;
            }
        }
    }
}