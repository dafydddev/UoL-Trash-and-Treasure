using UnityEngine;

namespace Gameplay
{
    public class ItemCounter : MonoBehaviour
    {
        [SerializeField] private GameObject[] items;
        private int _itemCounter = 0;

        private void Start()
        {
            GameEvents.OnScoreChanged += HandleScoreChanged;
        }
        
        private void OnDestroy()
        {
            GameEvents.OnScoreChanged -= HandleScoreChanged;
        }
        
        private void HandleScoreChanged(int _)
        {
            if (items.Length > 0 && _itemCounter < items.Length)
            {
                items[_itemCounter].SetActive(true);
                _itemCounter++;
            }
        }
    }
}
