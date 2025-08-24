using Managers;
using UnityEngine;

namespace Gameplay.Items
{
    [RequireComponent(typeof(Collider2D))]
    public class ItemBasket : MonoBehaviour
    {
        // The type of item this basket accepts for scoring
        [SerializeField] private ItemType acceptedItemType;
        
        // Multiplier applied to item value when scoring
        [SerializeField] private int scoreModifier;

        // Handle items entering the basket trigger zone
        private void OnTriggerEnter2D(Collider2D other)
        {
            var item = other.GetComponent<Item>();
            if (item == null) return;
            
            // Check if the item matches the accepted type
            if (item.GetItemType() == acceptedItemType)
            {
                // Correct item: add score and remove item
                GameEvents.OnScoreChanged?.Invoke(item.GetValue() * scoreModifier);
                item.SwitchOff();
                other.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            }
            else
            {
                // Wrong item: lose a life
                GameEvents.OnLiveLost?.Invoke();
            }
        }
    }
}