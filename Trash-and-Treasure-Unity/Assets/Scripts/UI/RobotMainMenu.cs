using UnityEngine;

namespace UI
{
    public class RobotMainMenu : MonoBehaviour
    {
        [SerializeField] private float minSpeed;
        [SerializeField] private float maxSpeed;
        [SerializeField] private float minTimeToNextRun;
        [SerializeField] private float maxTimeToNextRun;
        [SerializeField] private Sprite[] itemsToCarry;
        [SerializeField] private SpriteRenderer itemCarryingRenderer;
        [SerializeField] private float leftEdgeXPosition = -380;
        [SerializeField] private float rightEdgeXPosition = 380;
        [SerializeField] private int topHeight;
        [SerializeField] private int bottomHeight;
        
        private SpriteRenderer _itemCarryingRenderer;
        private SpriteRenderer _robotSpriteRender;
        private int _height;
        private float _timeToNextRun;
        private float _speed;
        private bool _movingRight = true;
        private bool _isRunning = false;
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {
            _robotSpriteRender = GetComponent<SpriteRenderer>();
            _itemCarryingRenderer = itemCarryingRenderer.gameObject.GetComponent<SpriteRenderer>();
            TriggerNewRun();
        }
        
        private void Update()
        {
            if (_isRunning)
            {
                // Move the robot
                float direction = _movingRight ? 1f : -1f;
                transform.Translate(Vector3.right * (_speed * direction * Time.deltaTime));
                
                // Check if the robot reached the edge
                if (_movingRight && transform.position.x >= rightEdgeXPosition)
                {
                    _isRunning = false;
                    _movingRight = false;
                    FlipRobotSprite();
                    _timeToNextRun = PickTimeToNextRun();
                }
                else if (!_movingRight && transform.position.x <= leftEdgeXPosition)
                {
                    _isRunning = false;
                    _movingRight = true;
                    FlipRobotSprite();
                    _timeToNextRun = PickTimeToNextRun();
                }
            }
            else
            {
                // Wait for the next run
                _timeToNextRun -= Time.deltaTime;
                if (_timeToNextRun <= 0)
                {
                    TriggerNewRun();
                    _isRunning = true;
                }
            }
        }

        private void TriggerNewRun()
        {
            _speed = PickSpeed();
            _timeToNextRun = PickTimeToNextRun();
            _itemCarryingRenderer.sprite = PickRandomSprite();
            _height = PickRandomHeight();
            transform.position = new Vector3(transform.position.x, _height, transform.position.z);
            transform.localScale = _height == topHeight ? new Vector3(5f, 5f, 5f) : new Vector3(7f, 7f, 7f);
        }
        
        private float PickSpeed()
        {
            return Random.Range(minSpeed, maxSpeed);
        }

        private float PickTimeToNextRun()
        {
            return Random.Range(minTimeToNextRun, maxTimeToNextRun);
        }

        private Sprite PickRandomSprite()
        {
            return itemsToCarry[Random.Range(0, itemsToCarry.Length)];
        }

        private int PickRandomHeight()
        {
            return Random.Range(0, 2) == 0 ? topHeight : bottomHeight;
        }

        private void FlipRobotSprite()
        {
            _robotSpriteRender.flipX = !_robotSpriteRender.flipX;
        }
    }
}
