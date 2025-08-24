using UnityEngine;

namespace Gameplay.Scene
{
    public class DangerZone : MonoBehaviour
    {
        // Counter for items currently in the danger zone
        private int _itemsInDanger;
        
        // Flag to track if warning animation is active
        private bool _isActivated;
        
        // Animator component for warning animations
        private Animator _animator;
        
        // Animation trigger name for warning state
        [SerializeField] private string warningTrigger = "WarningActive";
        // Animation trigger name for disabling warning state
        [SerializeField] private string warningDeactivateTrigger = "WarningDeactivate";

        private void Start()
        {
            // Get the animator component for warning animations
            _animator = GetComponent<Animator>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            // Increment counter when an item enters the danger zone
            _itemsInDanger++;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            // Decrement counter when an item exits the danger zone (with safety check)
            if (_itemsInDanger <= 0) return;
            _itemsInDanger--;
        }

        private void Update()
        {
            // Activate warning when items are present and not already activated
            if (_itemsInDanger > 0 && !_isActivated)
            {
                _isActivated = true;
                _animator.SetTrigger(warningTrigger);
            }
            // Deactivate warning when no items are present
            else if (_itemsInDanger == 0 && _isActivated)
            {
                _isActivated = false;
                _animator.SetTrigger(warningDeactivateTrigger);
            }
        }
    }
}