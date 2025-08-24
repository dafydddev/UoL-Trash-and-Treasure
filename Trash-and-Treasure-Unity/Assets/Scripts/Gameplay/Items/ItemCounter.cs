using Managers;
using UnityEngine;

namespace Gameplay.Items
{
    public class ItemCounter : MonoBehaviour
    {
        // Array of UI item representations to activate as the score increases
        [SerializeField] private GameObject[] items;
        
        // Tracks how many items have been activated so far
        private int _itemCounter;

        private void Start()
        {
            // Subscribe to score change events
            GameEvents.OnScoreChanged += HandleScoreChanged;
        }

        private void OnDestroy()
        {
            // Unsubscribe from score change events
            GameEvents.OnScoreChanged -= HandleScoreChanged;
        }

        // Handle score changes by activating the next item UI element
        private void HandleScoreChanged(int _)
        {
            // Check if there are items to activate and we haven't activated them all yet
            if (items.Length > 0 && _itemCounter < items.Length)
            {
                // Activate the next item in the sequence
                items[_itemCounter].SetActive(true);
                _itemCounter++;
                
                // Check if all items have been activated (level complete condition)
                if (_itemCounter == items.Length)
                {
                    GameEvents.OnLevelComplete?.Invoke();
                }
            }
        }
    }
}