using System;
using UnityEngine;

namespace Gameplay.Scene
{
    public class PushZone : MonoBehaviour
    {
        // Direction enum for the push zone force direction
        private enum Direction
        {
            Left,
            Right
        }

        // Direction to push objects that enter the zone
        [SerializeField] private Direction direction;
        // Strength of the force applied to objects
        [SerializeField] private float forceStrength = 5f;
        
        // Direction vectors for left and right forces
        private readonly Vector2 _leftDirection = Vector2.left;
        private readonly Vector2 _rightDirection = Vector2.right;

        private void OnTriggerEnter2D(Collider2D other)
        {
            // Get the rigidbody component from the colliding object
            var rb = other.GetComponent<Rigidbody2D>();
            if (rb == null) return;
            
            // Apply force based on the configured direction
            switch (direction)
            {
                case Direction.Left:
                    rb.AddForce(_leftDirection * forceStrength, ForceMode2D.Impulse);
                    break;
                case Direction.Right:
                    rb.AddForce(_rightDirection * forceStrength, ForceMode2D.Impulse);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}