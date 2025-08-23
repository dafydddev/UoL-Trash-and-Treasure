using UnityEngine;

namespace Gameplay
{
    [RequireComponent(typeof(Collider2D))]
    public class ItemBasket : MonoBehaviour
    {
        [SerializeField] private ItemType acceptedItemType;
        [SerializeField] private int scoreModifier;

        private void OnTriggerEnter2D(Collider2D other)
        {
            var item = other.GetComponent<Item>();
            if (item == null) return;
            if (item.GetItemType() == acceptedItemType)
            {
                GameEvents.OnScoreChanged?.Invoke(item.GetValue() * scoreModifier);
                item.SwitchOff();
                other.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            }
            else
            {
                GameEvents.OnLiveLost?.Invoke();
            }
        }
    }
}