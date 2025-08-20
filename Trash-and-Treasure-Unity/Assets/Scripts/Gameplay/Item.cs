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

    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class Item : MonoBehaviour
    {
        [SerializeField] private ItemType type;
        [SerializeField] private int scoreValue;
        [SerializeField] private float shrinkSpeed = 0.1f;
        [SerializeField] private float driftForce = 0.5f;
        [SerializeField] private float velocityThreshold = 0.1f;

        private SpriteRenderer _spriteRenderer;
        [SerializeField] private Sprite boxedSprite;
        [SerializeField] private Sprite unboxedSprite;

        [SerializeField] GameObject unboxParticle;
        [SerializeField] GameObject basketParticle;
        [SerializeField] GameObject tutorialPointer;

        [SerializeField] private int minClickToUnbox = 1;
        [SerializeField] private int maxClickToUnbox = 10;
        private int _clicksToUnboxRequired;
        private int _clicksToUnboxCounter;

        private Rigidbody2D _rb2d;
        private Collider2D _col2d;

        private readonly Vector3 _minScaleBeforeCleanUp = new(0.01f, 0.01f, 0.01f);

        private bool _isShrinking;
        private ItemState _itemState;
        private bool _isMouseOver = false;

        private Collider2D _groundCollider;
        private Collider2D _deathCollider;

        private ItemLauncher _itemLauncher;

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
            _rb2d = GetComponent<Rigidbody2D>();
            _itemLauncher = GetComponent<ItemLauncher>();
            _col2d = GetComponent<Collider2D>();
        }

        public void Update()
        {
            if (_isShrinking)
            {
                basketParticle.gameObject.SetActive(true);
                ShrinkGameObject();
            }
        }

        private void FixedUpdate()
        {
            if (!IsOnGround() || _isMouseOver || _itemLauncher.IsDragging() || !GameEvents.GetGameInProgress())
            {
                return;
            }

            if (_rb2d.linearVelocity.magnitude < velocityThreshold)
            {
                _rb2d.linearVelocity = new Vector2(driftForce, _rb2d.linearVelocity.y);
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
                Destroy(gameObject, 5.0f);
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

        public void OnMouseEnter()
        {
            _isMouseOver = true;
        }

        public void OnMouseExit()
        {
            _isMouseOver = false;
        }

        private bool IsOnGround()
        {
            return _groundCollider && _rb2d.IsTouching(_groundCollider);
        }

        private void UnboxItem()
        {
            _spriteRenderer.sprite = unboxedSprite;
            SetItemState(ItemState.Unboxed);
            unboxParticle.SetActive(true);
            tutorialPointer.SetActive(false);
            _rb2d.freezeRotation = false;
            OnUnboxed?.Invoke();
            if (!GameEvents.GetGameInProgress())
            {
                GameEvents.OnGameStart?.Invoke();
            }
        }

        private void SetItemState(ItemState state)
        {
            _itemState = state;
        }

        public ItemState GetItemState()
        {
            return _itemState;
        }

        public void SetColliders(Collider2D groundCollider, Collider2D deathCollider)
        {
            _groundCollider = groundCollider;
            _deathCollider = deathCollider;
        }

        public Collider2D GetGroundCollider()
        {
            return _groundCollider;
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col != _deathCollider) return;
            GameEvents.OnLiveLost?.Invoke();
            _col2d.enabled = false;
            Destroy(gameObject, 2.0f);
        }
    }
}