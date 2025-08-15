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

        [SerializeField] GameObject unboxParticle;

        [SerializeField] private int minClickToUnbox = 1;
        [SerializeField] private int maxClickToUnbox = 10;
        private int _clicksToUnboxRequired;
        private int _clicksToUnboxCounter;

        private readonly Vector3 _minScaleBeforeCleanUp = new(0.1f, 0.1f, 0.1f);

        private bool _isShrinking;
        private ItemState _itemState;

        public ItemType GetItemType() => type;
        public int GetValue() => scoreValue;
        
        public void Start()
        {
            _isShrinking = false;
            SetItemState(ItemState.Boxed);
            _clicksToUnboxRequired = Random.Range(minClickToUnbox, maxClickToUnbox);
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
            if (gameObject.transform.localScale.x > _minScaleBeforeCleanUp.x)
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
            _spriteRenderer.sprite = unboxedSprite;
            SetItemState(ItemState.Unboxed);
            unboxParticle.SetActive(true);
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