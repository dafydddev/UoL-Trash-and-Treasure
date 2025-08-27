using System;
using Managers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Gameplay.Items
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
    [RequireComponent(typeof(TrailRenderer))]
    public class Item : MonoBehaviour
    {
        [Header("Item Properties")]
        // The type of item (Apple, Banana, Carrot)
        [SerializeField] private ItemType type;
        // Score value when collected in the correct basket
        [SerializeField] private int scoreValue;
        
        [Header("Movement Settings")]
        // Speed at which item shrinks when collected
        [SerializeField] private float shrinkSpeed = 0.1f;
        // Force applied to keep item drifting when stationary
        [SerializeField] private float driftForce = 0.5f;
        // Velocity threshold below which drift force is applied
        [SerializeField] private float velocityThreshold = 0.1f;
        
        [Header("Item Physics Settings")]
        // Easing applied to crusher logic
        // If the item position is greater than this, we don't ever destroy it
        [SerializeField] private float crusherEasing = -1.2f;

        [Header("Visual Components")]
        private SpriteRenderer _spriteRenderer;
        // Sprite shown when item is in boxed state
        [SerializeField] private Sprite boxedSprite;
        // Sprite shown when item is unboxed and ready to launch
        [SerializeField] private Sprite unboxedSprite;

        [Header("Effects")]
        // Particle effect when unboxing
        [SerializeField] private GameObject unboxParticle;
        // Particle effect when collected in the basket
        [SerializeField] private GameObject basketParticle;
        // Tutorial pointer to guide player interaction
        [SerializeField] private GameObject tutorialPointer;

        [Header("Unboxing Settings")]
        // Minimum clicks required to unbox
        [SerializeField] private int minClickToUnbox = 1;
        // Maximum clicks required to unbox
        [SerializeField] private int maxClickToUnbox = 10;
        private int _clicksToUnboxRequired;
        private int _clicksToUnboxCounter;

        // Component references
        private Rigidbody2D _rb2d;
        private Collider2D _col2d;
        private TrailRenderer _trailRenderer;

        // Shrinking threshold - item is destroyed when the scale reaches this
        private readonly Vector3 _minScaleBeforeCleanUp = new(0.01f, 0.01f, 0.01f);

        // State tracking
        private bool _isShrinking;
        private ItemState _itemState;
        private bool _isMouseOver;

        // World collider references
        private Collider2D _groundCollider;
        private Collider2D _deathCollider;

        private ItemLauncher _itemLauncher;

        // Events for item interactions
        public event Action OnClickedBoxed;
        public event Action OnUnboxed;
        public event Action OnItemPositive;
        public event Action OnItemNegative;

        // Public getters for item properties
        public ItemType GetItemType() => type;
        public int GetValue() => scoreValue;

        public void Start()
        {
            // Initialise item in boxed state
            _isShrinking = false;
            SetItemState(ItemState.Boxed);
            _clicksToUnboxRequired = Random.Range(minClickToUnbox, maxClickToUnbox);
            
            // Get component references
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _spriteRenderer.sprite = boxedSprite;
            _rb2d = GetComponent<Rigidbody2D>();
            _itemLauncher = GetComponent<ItemLauncher>();
            _col2d = GetComponent<Collider2D>();
            _trailRenderer = GetComponent<TrailRenderer>();
        }

        public void Update()
        {
            // Handle shrinking animation when the item is collected
            if (_isShrinking)
            {
                basketParticle.gameObject.SetActive(true);
                ShrinkGameObject();
            }
        }

        private void FixedUpdate()
        {
            // Skip drift logic if the item is not on ground,
            // mouse is over it, being dragged, or the game isn't in progress
            if (!IsOnGround() || _isMouseOver || _itemLauncher.IsDragging() || !GameEvents.IsGameInProgress())
            {
                return;
            }

            // Apply drift force if item velocity is too low
            if (_rb2d.linearVelocity.magnitude < velocityThreshold)
            {
                _rb2d.linearVelocity = new Vector2(driftForce, _rb2d.linearVelocity.y);
            }
        }

        // Start the shrinking process when the item is collected correctly
        public void SwitchOff()
        {
            OnItemPositive?.Invoke();
            _isShrinking = true;
        }

        // Gradually shrink the item until it's small enough to destroy
        private void ShrinkGameObject()
        {
            if (gameObject.transform.localScale.x > _minScaleBeforeCleanUp.x)
            {
                gameObject.transform.localScale -=
                    new Vector3(shrinkSpeed * Time.deltaTime, shrinkSpeed * Time.deltaTime, 0);
            }
            else
            {
                Destroy(gameObject, 1.0f);
            }
        }

        // Handle mouse clicks for unboxing when the item is clicked
        public void OnMouseOver()
        {
            // Early exit when the level is over or paused
            if (GameEvents.IsLevelOver() || GameEvents.IsPaused()) return;
            // Early exit when we are not clicking down or the item is already unboxed
            if (!Input.GetMouseButtonDown(0) || _itemState != ItemState.Boxed) return;
            
            // Count clicks until the required amount is reached
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

        // Track when mouse enters item area
        public void OnMouseEnter()
        {
            _isMouseOver = true;
        }

        // Track when mouse exits item area
        public void OnMouseExit()
        {
            _isMouseOver = false;
        }

        // Check if the item is currently touching the ground
        private bool IsOnGround()
        {
            return _groundCollider && _rb2d.IsTouching(_groundCollider);
        }

        // Transform item from boxed to unboxed state
        private void UnboxItem()
        {
            _spriteRenderer.sprite = unboxedSprite;
            SetItemState(ItemState.Unboxed);
            unboxParticle.SetActive(true);
            tutorialPointer.SetActive(false);
            _rb2d.freezeRotation = false;
            _trailRenderer.enabled = true;
            OnUnboxed?.Invoke();
            
            // Start the game if it hasn't started yet
            if (!GameEvents.IsGameInProgress())
            {
                GameEvents.OnGameStart?.Invoke();
            }
        }

        // Set the current state of the item
        private void SetItemState(ItemState state)
        {
            _itemState = state;
        }

        // Get the current state of the item
        public ItemState GetItemState()
        {
            return _itemState;
        }

        // Set references to world colliders for ground and death zone detection
        public void SetColliders(Collider2D groundCollider, Collider2D deathCollider)
        {
            _groundCollider = groundCollider;
            _deathCollider = deathCollider;
        }

        // Get reference to ground collider for other components
        public Collider2D GetGroundCollider()
        {
            return _groundCollider;
        }

        // Handle item falling into the death zone
        private void OnTriggerEnter2D(Collider2D col)
        {   
            // Early exit if we cannot access the death collider
            if (!_deathCollider) return; 
            
            // Early exit if we entered a trigger that is *not* the death zone
            if (col != _deathCollider) return; 
            
            // Give a little grace room for the player, so it doesn't feel unfair
            if (transform.position.y > crusherEasing) return;
            
            // Item fell into the death zone - lose a life
            GameEvents.OnLiveLost?.Invoke();
            OnItemNegative?.Invoke();
            _col2d.enabled = false;
            Destroy(gameObject, 2.0f);
        }
    }
}