using Managers;
using UnityEngine;

namespace Gameplay.Items
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(Item))]
    public class ItemLauncher : MonoBehaviour
    {
        [Header("Launch Settings")] 
        // Maximum force that can be applied when launching
        [SerializeField, Range(0f, 10f)]
        private float maxForce = 10f;

        // Maximum distance the item can be pulled back for aiming
        [SerializeField, Range(0f, 1f)] 
        public float maxPullDistance = 0.5f;

        [Header("Visual Feedback")] 
        // GameObject that shows the aim direction and force
        public GameObject aimIndicator;

        // Component references
        private Item _item;
        private Rigidbody2D _rb2d;
        private Collider2D _collider2d;
        private Camera _cam;
        
        // Drag state tracking
        private bool _isDragging;
        private Vector3 _startPosition;
        private Vector3 _startMouseWorldPos;

        private void Start()
        {
            // Initialize component references
            _item = GetComponent<Item>();
            _rb2d = GetComponent<Rigidbody2D>();
            _collider2d = GetComponent<Collider2D>();
            _cam = Camera.main;

            // Subscribe to pause events
            GameEvents.OnPauseToggled += HandlePause;

            // Setup aim indicator
            aimIndicator.transform.localScale = Vector3.one * 0.5f;
            aimIndicator.SetActive(false);
        }
        
        private void OnDestroy()
        {
            // Unsubscribe from pause events
            GameEvents.OnPauseToggled -= HandlePause;
        }

        private void Update()
        {
            // Early exit when the game is paused or the item is null
            if (GameEvents.IsPaused() || !_item) return;
            // Early exit when the item is boxed (can't launch it)
            if (_item.GetItemState() == ItemState.Boxed) return;
            
            // Handle mouse release for launching
            if (Input.GetMouseButtonUp(0) && _isDragging)
            {
                Launch();
            }

            // Update visual feedback during drag
            if (_isDragging)
            {
                UpdateVisualFeedback();
            }
        }

        // Handle mouse click to start dragging
        private void OnMouseDown()
        {
            // Early exit if the game is paused, or we cannot get the item
            if (GameEvents.IsPaused() || !_item) return;
            // Early exit if we can get the item, but it is boxed
            if (_item.GetItemState() == ItemState.Boxed) return;
            // Check if item is touching ground before allowing drag
            if (_collider2d.IsTouching(_item.GetGroundCollider()))
            {
                StartDrag();
            }
        }

        // Handle pause state changes
        private void HandlePause(bool isPaused)
        {
            if (isPaused)
            {
                ResetLauncher();
            }
        }

        // Reset the launcher state when paused
        private void ResetLauncher()
        {
            // Early exit if we are not dragging
            if (!_isDragging) return;
            _isDragging = false;
            aimIndicator.SetActive(false);
        }

        // Initialise drag operation
        private void StartDrag()
        {
            _isDragging = true;
            _startPosition = transform.position;
            _startMouseWorldPos = _cam.ScreenToWorldPoint(Input.mousePosition);
            _startMouseWorldPos.z = 0;

            // Make rigidbody kinematic during drag to prevent physics interference
            _rb2d.bodyType = RigidbodyType2D.Kinematic;
            _rb2d.linearVelocity = Vector2.zero;
            _rb2d.angularVelocity = 0f;

            // Show aim indicator
            aimIndicator.SetActive(true);
        }

        // Update aim indicator position and rotation based on mouse movement
        private void UpdateVisualFeedback()
        {
            var currentMouseWorldPos = _cam.ScreenToWorldPoint(Input.mousePosition);
            currentMouseWorldPos.z = 0;

            // Calculate pull vector and constrain to max distance
            var pullVector = _startMouseWorldPos - currentMouseWorldPos;
            var pullDirection = pullVector.normalized;
            var pullDistance = Mathf.Min(pullVector.magnitude, maxPullDistance);

            // Position the aim indicator
            var endPoint = _startPosition + pullDirection * pullDistance;
            aimIndicator.transform.position = endPoint;
            
            // Rotate the aim indicator to point in the launch direction
            if (pullVector.magnitude > 0.01f) // Avoid rotation when there's no meaningful pull
            {
                var angle = Mathf.Atan2(pullDirection.y, pullDirection.x) * Mathf.Rad2Deg;
                aimIndicator.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
        }

        // Launch the item with calculated force
        private void Launch()
        {
            var currentMouseWorldPos = _cam.ScreenToWorldPoint(Input.mousePosition);
            currentMouseWorldPos.z = 0;

            // Calculate launch parameters
            var pullVector = _startMouseWorldPos - currentMouseWorldPos;
            var launchDirection = pullVector.normalized;
            var pullDistance = Mathf.Min(pullVector.magnitude, maxPullDistance);
            
            // Re-enable physics
            _rb2d.bodyType = RigidbodyType2D.Dynamic;

            // Apply launch force based on pull distance
            var forceMultiplier = (pullDistance / maxPullDistance) * maxForce;
            _rb2d.AddForce(launchDirection * forceMultiplier, ForceMode2D.Impulse);

            // Clean up drag state
            _isDragging = false;
            aimIndicator.SetActive(false);
        }
        
        // Public method to check if currently dragging
        public bool IsDragging()
        {
            return _isDragging;
        }
    }
}