using UnityEngine;

namespace Gameplay
{
    public class CatapultObject : MonoBehaviour
    {
        public float launchForce = 20f;
        public float rightwardForce = 1f;

        private Rigidbody2D _rb;
        private bool _isDragging;
        private Vector3 _dragStart;

        private Camera _cam;
        private Collider2D _col;

        private void Start()
        {
            _cam = Camera.main;
            _rb = GetComponent<Rigidbody2D>();
            _col = GetComponent<Collider2D>();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);
                if (_col.OverlapPoint(mousePos))
                {
                    _isDragging = true;
                    _dragStart = mousePos;
                }
            }
            else if (Input.GetMouseButtonUp(0) && _isDragging)
            {
                var mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);
                var direction = _dragStart - mousePos;
                _rb.AddForce(direction * launchForce, ForceMode2D.Impulse);
                _isDragging = false;
            }
            
            if (!_isDragging)
            {
                var direction = Vector2.right;
                _rb.AddForce(direction, ForceMode2D.Force);
            }
            
        }
    }
}