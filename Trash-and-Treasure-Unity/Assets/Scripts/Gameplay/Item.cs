using UnityEngine;

namespace Gameplay
{
    public enum ItemType
    {
        Apple,
        Banana,
        Carrot
    }

    public enum ItemState
    {
        Boxed,
        Unboxed
    }

    public class Item : MonoBehaviour
    {
        [SerializeField] private ItemType type;
        [SerializeField] private int scoreValue;
        [SerializeField] private float shrinkSpeed = 0.1f;

        private SpriteRenderer _spriteRenderer;
        [SerializeField] private Sprite boxedSprite;
        [SerializeField] private Sprite unboxedSprite;

        [SerializeField] private int minClickToUnbox = 1;
        [SerializeField] private int maxClickToUnbox = 10;
        private int _clicksToUnboxRequired;
        private int _clicksToUnboxCounter = 0;

        private bool _isShrinking;
        private ItemState _itemState;

        public ItemType GetItemType() => type;
        public int GetValue() => scoreValue;

        public void Start()
        {
            _isShrinking = false;
            SetItemState(ItemState.Boxed);
            _clicksToUnboxRequired = Random.Range(minClickToUnbox, maxClickToUnbox);

            gameObject.transform.localScale = Vector3.one;
            gameObject.transform.localPosition = Vector3.zero;
            gameObject.transform.localRotation = Quaternion.identity;

            _spriteRenderer = GetComponent<SpriteRenderer>();
            _spriteRenderer.sprite = boxedSprite;
        }

        public void Update()
        {
            if (_isShrinking)
            {
                ShrinkGameObject();
            }
        }

        public void SwitchOff()
        {
            _isShrinking = true;
        }

        private void ShrinkGameObject()
        {
            if (gameObject.transform.localScale.x > 0.1f)
            {
                gameObject.transform.localScale -=
                    new Vector3(shrinkSpeed * Time.deltaTime, shrinkSpeed * Time.deltaTime, 0);
            }
            else
            {
                _isShrinking = false;
                Destroy(gameObject);
            }
        }

        public void OnMouseOver()
        {
            if (Input.GetMouseButtonDown(0) && _itemState == ItemState.Boxed)
            {
                if (_clicksToUnboxCounter < _clicksToUnboxRequired)
                {
                    _clicksToUnboxCounter++;
                }
                else
                {
                    UnboxItem();
                }
            }
        }

        private void UnboxItem()
        {
            SetItemState(ItemState.Unboxed);
            _spriteRenderer.sprite = unboxedSprite;
        }

        private void SetItemState(ItemState state)
        {
            _itemState = state;
        }

        public ItemState GetItemState()
        {
            return _itemState;
        }
    }
}