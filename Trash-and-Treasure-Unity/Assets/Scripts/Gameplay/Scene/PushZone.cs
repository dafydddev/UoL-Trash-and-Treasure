using UnityEngine;

namespace Gameplay
{
    public class PushZone : MonoBehaviour
    {
        private enum Direction
        {
            Left,
            Right
        }
        
        [SerializeField] private Direction direction;
        [SerializeField] private float forceStrength = 5f; 
        private Vector2 _leftDirection = Vector2.left; 
        private Vector2 _rightDirection = Vector2.right;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (rb == null) return;
            if (direction == Direction.Left)
            {
                rb.AddForce(_leftDirection.normalized * forceStrength, ForceMode2D.Impulse);
            }
            else
            {
                rb.AddForce(_rightDirection.normalized * forceStrength, ForceMode2D.Impulse);
            }
        }
    }
}