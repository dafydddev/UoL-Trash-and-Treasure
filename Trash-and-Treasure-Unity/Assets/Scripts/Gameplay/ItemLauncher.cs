using UnityEngine;

namespace Gameplay
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    public class ItemLauncher : MonoBehaviour
    {
        [Header("Launch Settings")] [SerializeField, Range(0f, 10f)]
        private float maxForce = 10f;

        [SerializeField, Range(0f, 1f)] public float maxPullDistance = 0.5f;
        [SerializeField] private Collider2D groundCollider2d;

        [Header("Visual Feedback")] public GameObject aimIndicator;

        private Rigidbody2D _rb2d;
        private Collider2D _collider2d;
        private Camera _cam;
        private bool _isDragging = false;
        private Vector3 _startPosition;
        private Vector3 _startMouseWorldPos;


        private void Start()
        {
            _rb2d = GetComponent<Rigidbody2D>();
            _collider2d = GetComponent<Collider2D>();
            _cam = Camera.main;

            GameEvents.OnPauseToggled += HandlePause;

            aimIndicator.transform.localScale = Vector3.one * 0.3f;
            aimIndicator.SetActive(false);

        }

        private void Update()
        {
            if (GameEvents.GetIsPaused())
            {
                return;
            }

            HandleMouseInput();
            if (_isDragging)
            {
                UpdateVisualFeedback();
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
            // Stop any dragging operation
            if (_isDragging)
            {
                _isDragging = false;
                aimIndicator.SetActive(false);
            }
        }

        private void HandleMouseInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mouseWorldPos = _cam.ScreenToWorldPoint(Input.mousePosition);
                mouseWorldPos.z = 0; // Ensure we're working in 2D

                // Check if mouse is over this sprite using 2D collision
                if (_collider2d && _collider2d.OverlapPoint(mouseWorldPos) && _collider2d.IsTouching(groundCollider2d))
                {
                    StartDrag();
                }
            }
            else if (Input.GetMouseButtonUp(0) && _isDragging)
            {
                Launch();
            }
        }

        private void StartDrag()
        {
            _isDragging = true;
            _startPosition = transform.position;
            _startMouseWorldPos = _cam.ScreenToWorldPoint(Input.mousePosition);
            _startMouseWorldPos.z = 0;

            // Stop any existing movement
            _rb2d.linearVelocity = Vector2.zero;
            _rb2d.angularVelocity = 0f;

            // Enable visual feedback
            aimIndicator.SetActive(true);
        }

        private void UpdateVisualFeedback()
        {
            Vector3 currentMouseWorldPos = _cam.ScreenToWorldPoint(Input.mousePosition);
            currentMouseWorldPos.z = 0;

            // Calculate pull direction and distance
            Vector2 pullVector = _startMouseWorldPos - currentMouseWorldPos;
            Vector2 pullDirection = pullVector.normalized;
            float pullDistance = Mathf.Min(pullVector.magnitude, maxPullDistance);

            // Update trajectory line (showing launch direction)
            Vector3 endPoint = _startPosition + (Vector3)(pullDirection * pullDistance);

            // Update aim indicator
            aimIndicator.transform.position = endPoint;

            // Update line color based on power
            float powerRatio = pullDistance / maxPullDistance;
            Color lineColor = Color.Lerp(Color.yellow, Color.red, powerRatio);
        }

        private void Launch()
        {
            Vector3 currentMouseWorldPos = _cam.ScreenToWorldPoint(Input.mousePosition);
            currentMouseWorldPos.z = 0;

            // Calculate launch force
            Vector2 pullVector = _startMouseWorldPos - currentMouseWorldPos;
            Vector2 launchDirection = pullVector.normalized;
            float pullDistance = Mathf.Min(pullVector.magnitude, maxPullDistance);

            // Apply force proportional to pull distance
            float forceMultiplier = (pullDistance / maxPullDistance) * maxForce;
            _rb2d.AddForce(launchDirection * forceMultiplier, ForceMode2D.Impulse);

            // Clean up
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
    }
}