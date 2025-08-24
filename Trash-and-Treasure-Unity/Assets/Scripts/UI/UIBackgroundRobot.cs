using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class UIBackgroundRobot : MonoBehaviour
    {
        // Min and Max speed across the screen
        [SerializeField] private float minSpeed;
        [SerializeField] private float maxSpeed;

        // Min and Max timings between runs across the screen
        [SerializeField] private float minTimeToNextRun;
        [SerializeField] private float maxTimeToNextRun;

        // Items that the robot can carry across the screen
        [SerializeField] private Sprite[] itemsToCarry;

        // Sprite renderer used to show the item being carried
        [SerializeField] private SpriteRenderer itemCarryingRenderer;

        // Safe spaces outside the screen, where the robot is fully off the screen
        [SerializeField] private float leftEdgeXPosition = -380;
        [SerializeField] private float rightEdgeXPosition = 380;

        // Top and bottom heights for the robot to use as rows to run across the screen
        [SerializeField] private int topRow;
        [SerializeField] private int bottomRow;
        
        // Scale factors for when the robot is at the top or the bottom
        [SerializeField] private Vector3 topRowScale = new(5f, 5f, 5f);
        [SerializeField] private Vector3 bottomRowScale = new(7f, 7f, 7f);

        // Sprite renderer used to show the robot itself (used to flip the sprite)
        private SpriteRenderer _robotSpriteRender;
        // Data member for the itemCarryingRenderer, see above
        private SpriteRenderer _itemCarryingRenderer;
        
        // Run variables (row, time, speed, direction and state)
        private int _row;
        private float _timeToNextRun;
        private float _speed;
        private bool _movingRight = true;
        private bool _isRunning;

        private void Start()
        {
            // Get the sprite renderer for the robot
            _robotSpriteRender = GetComponent<SpriteRenderer>();
            // Get the sprite renderer for the item being carried
            _itemCarryingRenderer = itemCarryingRenderer;
            // Trigger the first run
            TriggerNewRun();
        }

        private void Update()
        {
            // If the robot is running, move it across the screen
            if (_isRunning)
            {
                // Translate the robot 
                var direction = _movingRight ? 1f : -1f;
                transform.Translate(Vector3.right * (_speed * direction * Time.deltaTime));

                // Check if the robot reached the edge
                if (_movingRight && transform.position.x >= rightEdgeXPosition)
                {
                    // Flip the sprite and reset the robot
                    _isRunning = false;
                    _movingRight = false;
                    FlipRobotSprite();
                    // Schedule the next run
                    _timeToNextRun = PickTimeToNextRun();
                }
                else if (!_movingRight && transform.position.x <= leftEdgeXPosition)
                {
                    // Flip the sprite and reset the robot
                    _isRunning = false;
                    _movingRight = true;
                    FlipRobotSprite();
                    // Schedule the next run
                    _timeToNextRun = PickTimeToNextRun();
                }
            }
            else
            {
                // Wait for the next run
                _timeToNextRun -= Time.deltaTime;
                // Check if we're ready for the next run
                if (_timeToNextRun <= 0)
                {
                    // Trigger the run
                    TriggerNewRun();
                    _isRunning = true;
                }
            }
        }

        private void TriggerNewRun()
        {
            // Pick a speed for the run
            _speed = PickSpeed();
            // Pick the time until the run
            _timeToNextRun = PickTimeToNextRun();
            // Pick an item to carry
            _itemCarryingRenderer.sprite = PickRandomSprite();
            // Pick the run to either be across the top or bottom of the screen
            _row = PickTopOrBottom();
            // Position the robot at either the top or the bottom
            transform.position = new Vector3(transform.position.x, _row, transform.position.z);
            // Scale the robot by the top (smaller) or bottom (larger)
            transform.localScale = _row == topRow ? topRowScale : bottomRowScale;
        }

        private float PickSpeed()
        {
            // Pick a number between the min and max speeds
            return Random.Range(minSpeed, maxSpeed);
        }

        private float PickTimeToNextRun()
        {
            // Pick a number between the min and max timings
            return Random.Range(minTimeToNextRun, maxTimeToNextRun);
        }

        private Sprite PickRandomSprite()
        {
            // Pick a random sprite from the list of items to carry
            return itemsToCarry[Random.Range(0, itemsToCarry.Length)];
        }

        private int PickTopOrBottom()
        {
            // Pick either the top or bottom row
            return Random.Range(0, 2) == 0 ? topRow : bottomRow;
        }

        private void FlipRobotSprite()
        {
            // Flip the sprite (so it can go from facing left-to-right or right-to-left
            _robotSpriteRender.flipX = !_robotSpriteRender.flipX;
        }
    }
}