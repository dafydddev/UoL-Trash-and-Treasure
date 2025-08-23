using UnityEngine;

namespace Gameplay
{
    public class FollowMouseX : MonoBehaviour
    {
        [Header("Following Settings")] [SerializeField]
        private float maxSpeed = 400f;

        [SerializeField] private float bufferZoneFromMouse = 0.1f;
        [SerializeField] private float rampUpDistance = 100f;

        [Header("Sprite to Flip")] [SerializeField]
        private SpriteRenderer spriteRend;

        private Camera _cam;
        private Vector3 _currentPosition;
        private Vector3 _mouseInWorldSpacePosition;

        [Header("Scene Edges")] [SerializeField]
        private float leftEdge = -2.75f;

        [SerializeField] private float rightEdge = 2.75f;

        private void Awake()
        {
            _cam = Camera.main;
        }

        private void Update()
        {
            if (GameEvents.GetIsPaused() || !GameEvents.GetGameInProgress())
            {
                return;
            }

            // Get mouse x position in world space 
            Vector3 mouseWorldPos = _cam.ScreenToWorldPoint(Input.mousePosition);
            float mouseX = mouseWorldPos.x;

            // Get the current sprite position
            _currentPosition = transform.position;

            // Handle sprite flipping based on the direction to mouse
            if (spriteRend)
            {
                spriteRend.flipX = mouseX < _currentPosition.x;
            }

            // Calculate distance to mouse on X-axis only
            float currentDistanceToMouseX = Mathf.Abs(_currentPosition.x - mouseX);

            // ONLY move if distance exceeds the threshold
            if (currentDistanceToMouseX > bufferZoneFromMouse)
            {
                // Calculate acceleration based on distance - further = faster, closer = slower 
                float t = (currentDistanceToMouseX - bufferZoneFromMouse) / rampUpDistance;
                t = Mathf.Clamp01(t);
                float currentSpeed = Mathf.Lerp(0, maxSpeed, t) * Time.deltaTime;
                // Move towards the exact mouse X position using MoveTowards, keeping Y and Z unchanged
                float desiredX = Mathf.MoveTowards(_currentPosition.x, mouseX, currentSpeed);
                float newX = Mathf.Clamp(desiredX, leftEdge, rightEdge);
                transform.position = new Vector3(newX, _currentPosition.y, _currentPosition.z);
            }
        }
    }
}