using UnityEngine;

namespace Gameplay
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(Item))]
    public class ItemLauncher : MonoBehaviour
    {
        [Header("Launch Settings")] [SerializeField, Range(0f, 10f)]
        private float maxForce = 10f;

        [SerializeField, Range(0f, 1f)] public float maxPullDistance = 0.5f;

        [Header("Visual Feedback")] public GameObject aimIndicator;

        private Item _item;
        private Rigidbody2D _rb2d;
        private Collider2D _collider2d;
        private Camera _cam;
        private bool _isDragging = false;
        private Vector3 _startPosition;
        private Vector3 _startMouseWorldPos;

        private void Start()
        {
            _item = GetComponent<Item>();
            _rb2d = GetComponent<Rigidbody2D>();
            _collider2d = GetComponent<Collider2D>();
            _cam = Camera.main;

            GameEvents.OnPauseToggled += HandlePause;

            aimIndicator.transform.localScale = Vector3.one * 0.5f;
            aimIndicator.SetActive(false);
        }

        private void Update()
        {
            if (GameEvents.GetIsPaused() || !_item)
            {
                return;
            }

            if (_item.GetItemState() == ItemState.Boxed)
            {
                return;
            }

            // Handle mouse release for launching
            if (Input.GetMouseButtonUp(0) && _isDragging)
            {
                Launch();
            }

            if (_isDragging)
            {
                UpdateVisualFeedback();
            }
        }

        // Replace the complex HandleMouseInput with simple OnMouseDown
        private void OnMouseDown()
        {
            if (GameEvents.GetIsPaused() || _item.GetItemState() == ItemState.Boxed)
            {
                return;
            }

            // Check if item is touching ground before allowing drag
            if (_collider2d.IsTouching(_item.GetGroundCollider()))
            {
                StartDrag();
            }
        }

        private void HandlePause(bool isPaused)
        {
            if (isPaused)
            {
                ResetLauncher();
            }
        }

        private void ResetLauncher()
        {
            if (_isDragging)
            {
                _isDragging = false;
                aimIndicator.SetActive(false);
            }
        }

        private void StartDrag()
        {
            _isDragging = true;
            _startPosition = transform.position;
            _startMouseWorldPos = _cam.ScreenToWorldPoint(Input.mousePosition);
            _startMouseWorldPos.z = 0;

            _rb2d.bodyType = RigidbodyType2D.Kinematic;
            _rb2d.linearVelocity = Vector2.zero;
            _rb2d.angularVelocity = 0f;

            aimIndicator.SetActive(true);
        }

        private void UpdateVisualFeedback()
        {
            Vector3 currentMouseWorldPos = _cam.ScreenToWorldPoint(Input.mousePosition);
            currentMouseWorldPos.z = 0;

            Vector2 pullVector = _startMouseWorldPos - currentMouseWorldPos;
            Vector2 pullDirection = pullVector.normalized;
            float pullDistance = Mathf.Min(pullVector.magnitude, maxPullDistance);

            Vector3 endPoint = _startPosition + (Vector3)(pullDirection * pullDistance);
            aimIndicator.transform.position = endPoint;
            
            if (pullVector.magnitude > 0.01f) // Avoid rotation when there's no meaningful pull
            {
                float angle = Mathf.Atan2(pullDirection.y, pullDirection.x) * Mathf.Rad2Deg;
                aimIndicator.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }

            // float powerRatio = pullDistance / maxPullDistance;
            // Color lineColor = Color.Lerp(Color.yellow, Color.red, powerRatio);
        }

        private void Launch()
        {
            Vector3 currentMouseWorldPos = _cam.ScreenToWorldPoint(Input.mousePosition);
            currentMouseWorldPos.z = 0;

            Vector2 pullVector = _startMouseWorldPos - currentMouseWorldPos;
            Vector2 launchDirection = pullVector.normalized;
            float pullDistance = Mathf.Min(pullVector.magnitude, maxPullDistance);
            
            _rb2d.bodyType = RigidbodyType2D.Dynamic;

            float forceMultiplier = (pullDistance / maxPullDistance) * maxForce;
            _rb2d.AddForce(launchDirection * forceMultiplier, ForceMode2D.Impulse);

            _isDragging = false;
            aimIndicator.SetActive(false);
        }

        private void OnDrawGizmos()
        {
            if (_isDragging)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(transform.position, maxPullDistance);
            }
        }

        public bool IsDragging()
        {
            return _isDragging;
        }
    }
}