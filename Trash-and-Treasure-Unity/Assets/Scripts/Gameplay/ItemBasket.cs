using UnityEngine;

namespace Gameplay
{
    [RequireComponent(typeof(Collider2D))]
    public class ItemBasket : MonoBehaviour
    {
        [SerializeField] private ItemType acceptedItemType;

        private void OnTriggerEnter2D(Collider2D other)
        {
            var item = other.GetComponent<Item>();
            if (item == null) return;
            if (item.GetItemType() == acceptedItemType)
            {
                GameEvents.OnScoreChanged?.Invoke(item.GetValue());
                item.SwitchOff();
            }
            else
            {
                GameEvents.OnLiveLost?.Invoke();
            }
        }
    }
}