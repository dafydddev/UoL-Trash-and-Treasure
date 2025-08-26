using Gameplay.Items;
using Managers;
using UnityEngine;

namespace Gameplay.Scene
{
    public class CraneDropper : MonoBehaviour
    {
        // Time range for dropping items
        [SerializeField] private float minSecondsToNextDrop = 5.0f;
        [SerializeField] private float maxSecondsToNextDrop = 10.0f;

        // Array of item prefabs to spawn
        [SerializeField] private GameObject[] itemsToSpawn;
        
        // Horizontal boundaries for crane movement
        [SerializeField] private float leftEdgeXPosition = -200;
        [SerializeField] private float rightEdgeXPosition = 200;

        // Sprites for the crane open and closed states
        [SerializeField] private Sprite craneOpenSprite;
        [SerializeField] private Sprite craneClosedSprite;

        // Sprite renderers for fake item display and crane
        [SerializeField] private SpriteRenderer fakeItemSpriteRenderer;
        [SerializeField] private SpriteRenderer caneSpriteRenderer;
        
        // Position to spawn items at
        private Vector3 _fakeItemTransform;

        // Cached sprite renderer components
        private SpriteRenderer _fakeSpriteRenderer;
        private SpriteRenderer _caneSpriteRenderer;

        // Colliders for item collision detection
        [SerializeField] private Collider2D groundCollider;
        [SerializeField] private Collider2D deathCollider;

        // Animator component for crane drop animation
        private Animator _animator;
        
        // Timer for the next item drop
        private float _timeToNextDrop;
        
        // Flag to prevent multiple simultaneous animations
        private bool _isAnimating;
        
        // Cached animator parameter hash for performance
        private static readonly int DropTriggerHash = Animator.StringToHash("Drop");

        private void Start()
        {
            // Initialize sprite renderer components
            _fakeSpriteRenderer = fakeItemSpriteRenderer.GetComponent<SpriteRenderer>();
            _caneSpriteRenderer = caneSpriteRenderer.GetComponent<SpriteRenderer>();
            // Set initial crane sprite to closed state
            _caneSpriteRenderer.sprite = craneClosedSprite;
            // Get animator component
            _animator = GetComponent<Animator>();
            // Set the initial drop timer
            _timeToNextDrop = PickNextDropTime();
            // Set animation flag before triggering the initial drop animation
            _isAnimating = true;
            // Trigger the initial drop animation
            _animator.SetTrigger(DropTriggerHash);
        }

        // Toggle between open and closed crane sprites
        private void ToggleCraneSprite()
        {
            _caneSpriteRenderer.sprite =
                _caneSpriteRenderer.sprite == craneOpenSprite ? craneClosedSprite : craneOpenSprite;
        }

        private void Update()
        {
            // Early exit if the game is not in progress, or we are in the middle of animating already
            if (!GameEvents.IsGameInProgress() || _isAnimating) return;
            
            // Decrement the drop timer
            _timeToNextDrop -= Time.deltaTime;

            // Check if it's time to drop an item
            if (_timeToNextDrop <= 0)
            {
                // Move the crane to a new random horizontal position
                transform.position = new Vector3(PickNewPosition(), transform.position.y, transform.position.z);

                // Trigger the drop animation
                _animator.SetTrigger(DropTriggerHash);

                // Reset timer for next drop
                _timeToNextDrop = PickNextDropTime();

                // Set flag to prevent retriggering during animation
                _isAnimating = true;
            }
        }

        // Called by animation event to drop an item
        public void DropItem()
        {
            ToggleCraneSprite();
            SpawnItem();
        }

        // Called by animation event when drop animation is complete
        public void AnimationComplete()
        {
            // Clear the animation flag first
            _isAnimating = false;
            // Reset timer for next drop
            _timeToNextDrop = PickNextDropTime();
            // Toggle the crane sprite
            ToggleCraneSprite();
            // Re-enable the fake item sprite
            _fakeSpriteRenderer.enabled = true;
        }

        // Spawn a random item at the fake item position
        private void SpawnItem()
        {
            // Hide the fake item sprite during spawn
            _fakeSpriteRenderer.enabled = false;
            // Get the spawn position from the fake item transform
            _fakeItemTransform = fakeItemSpriteRenderer.gameObject.transform.position;
            // Pick a random item to spawn
            var itemToSpawn = PickRandomItem();
            // Instantiate the item at the spawn position
            var spawnedItem = Instantiate(itemToSpawn, _fakeItemTransform, Quaternion.identity);
            // Configure the item's colliders
            var item = spawnedItem.GetComponent<Item>();
            item.SetColliders(groundCollider, deathCollider);
        }

        // Select a random item from the available items array
        private GameObject PickRandomItem()
        {
            return itemsToSpawn[Random.Range(0, itemsToSpawn.Length)];
        }

        // Generate a random time for the next item drop
        private float PickNextDropTime()
        {
            return Random.Range(minSecondsToNextDrop, maxSecondsToNextDrop);
        }

        // Generate a random horizontal position within the crane's movement bounds
        private float PickNewPosition()
        {
            return Random.Range(leftEdgeXPosition, rightEdgeXPosition);
        }
    }
}