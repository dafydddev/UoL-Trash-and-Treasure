using UnityEngine;
using Random = UnityEngine.Random;
using System;

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
        [SerializeField] GameObject tutorialPointer;

        [SerializeField] private int minClickToUnbox = 1;
        [SerializeField] private int maxClickToUnbox = 10;
        private int _clicksToUnboxRequired;
        private int _clicksToUnboxCounter;

        private readonly Vector3 _minScaleBeforeCleanUp = new(0.1f, 0.1f, 0.1f);

        private bool _isShrinking;
        private ItemState _itemState;
        
        private Collider2D _groundCollider;
        
        public event Action OnClickedBoxed;
        public event Action OnUnboxed;
        
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
                    OnClickedBoxed?.Invoke();
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
            tutorialPointer.SetActive(false);
            OnUnboxed?.Invoke();
        }

        private void SetItemState(ItemState state)
        {
            _itemState = state;
        }

        public ItemState GetItemState()
        {
            return _itemState;
        }
        
        public void SetGroundCollider(Collider2D groundCollider)
        {
            _groundCollider = groundCollider;
        }

        public Collider2D GetGroundCollider()
        {
            return _groundCollider;
        }

        private void OnTriggerEnter2D(Collider2D _)
        {
            GameEvents.OnLiveLost?.Invoke();
            Destroy(gameObject);
        }
    }
}