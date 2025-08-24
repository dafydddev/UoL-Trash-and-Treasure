using Managers;
using UnityEngine;

namespace Gameplay.Scene
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
            // Early exit if the game is not in progress *or* is paused
            if (!GameEvents.IsGameInProgress() || GameEvents.IsPaused()) return;
            
            // Get mouse x position in world space 
            var mouseWorldPos = _cam.ScreenToWorldPoint(Input.mousePosition);
            var mouseX = mouseWorldPos.x;

            // Get the current sprite position
            _currentPosition = transform.position;

            // Handle sprite flipping based on the direction to mouse
            if (spriteRend)
            {
                spriteRend.flipX = mouseX < _currentPosition.x;
            }

            // Calculate distance to mouse on X-axis only
            var currentDistanceToMouseX = Mathf.Abs(_currentPosition.x - mouseX);

            // Exit when the distance does not the threshold
            if (!(currentDistanceToMouseX > bufferZoneFromMouse)) return;
            
            // Calculate acceleration based on distance - further = faster, closer = slower 
            var t = (currentDistanceToMouseX - bufferZoneFromMouse) / rampUpDistance;
            t = Mathf.Clamp01(t);
            var currentSpeed = Mathf.Lerp(0, maxSpeed, t) * Time.deltaTime;
            // Move towards the exact mouse X position using MoveTowards, keeping Y and Z unchanged
            var desiredX = Mathf.MoveTowards(_currentPosition.x, mouseX, currentSpeed);
            var newX = Mathf.Clamp(desiredX, leftEdge, rightEdge);
            transform.position = new Vector3(newX, _currentPosition.y, _currentPosition.z);
        }
    }
}