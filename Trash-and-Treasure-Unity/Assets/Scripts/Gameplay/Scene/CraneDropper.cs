using UnityEngine;

namespace Gameplay
{
    public class CraneDropper : MonoBehaviour
    {
        [SerializeField] private float minSecondsToNextDrop = 5.0f;
        [SerializeField] private float maxSecondsToNextDrop = 10.0f;

        [SerializeField] private GameObject[] itemsToSpawn;
        [SerializeField] private float leftEdgeXPosition = -200;

        [SerializeField] private float rightEdgeXPosition = 200;

        [SerializeField] private Sprite craneOpenSprite;
        [SerializeField] private Sprite craneClosedSprite;

        [SerializeField] private SpriteRenderer fakeItemSpriteRenderer;
        [SerializeField] private SpriteRenderer caneSpriteRenderer;
        private Vector3 _fakeItemTransform;

        private SpriteRenderer _fakeSpriteRenderer;
        private SpriteRenderer _caneSpriteRenderer;

        [SerializeField] private Collider2D groundCollider;
        [SerializeField] private Collider2D deathCollider;

        private Animator _animator;
        private float _timeToNextDrop;
        private bool _isAnimating = false;
        private static readonly int DropTriggerHash = Animator.StringToHash("Drop");

        private void Start()
        {
            _fakeSpriteRenderer = fakeItemSpriteRenderer.GetComponent<SpriteRenderer>();
            _caneSpriteRenderer = caneSpriteRenderer.GetComponent<SpriteRenderer>();
            _caneSpriteRenderer.sprite = craneClosedSprite;
            _animator = GetComponent<Animator>();
            _timeToNextDrop = PickNextDropTime();
            // Do the initial drop
            _animator.SetTrigger(DropTriggerHash);
        }

        private void ToggleCraneSprite()
        {
            _caneSpriteRenderer.sprite =
                _caneSpriteRenderer.sprite == craneOpenSprite ? craneClosedSprite : craneOpenSprite;
        }

        private void Update()
        {
            if (GameEvents.GetIsPaused() || !GameEvents.GetGameInProgress() || _isAnimating)
            {
                return;
            }

            _timeToNextDrop -= Time.deltaTime;

            if (_timeToNextDrop <= 0)
            {
                // Set a new x position
                transform.position = new Vector3(PickNewPosition(), transform.position.y, transform.position.z);

                // Trigger the animation
                _animator.SetTrigger(DropTriggerHash);

                // Reset timer for next drop
                _timeToNextDrop = PickNextDropTime();

                // Set flag to prevent retriggering
                _isAnimating = true;
            }
        }

        public void DropItem()
        {
            ToggleCraneSprite();
            SpawnItem();
        }

        public void AnimationComplete()
        {
            // Reset timer for next drop and clear the animation flag
            _timeToNextDrop = PickNextDropTime();
            _isAnimating = false;
            ToggleCraneSprite();
            _fakeSpriteRenderer.enabled = true;
        }

        private void SpawnItem()
        {
            _fakeSpriteRenderer.enabled = false;
            _fakeItemTransform = fakeItemSpriteRenderer.gameObject.transform.position;
            var itemToSpawn = PickRandomItem();
            var spawnedItem = Instantiate(itemToSpawn, _fakeItemTransform, Quaternion.identity);
            var item = spawnedItem.GetComponent<Item>();
            item.SetColliders(groundCollider, deathCollider);
        }

        private GameObject PickRandomItem()
        {
            return itemsToSpawn[Random.Range(0, itemsToSpawn.Length)];
        }

        private float PickNextDropTime()
        {
            return Random.Range(minSecondsToNextDrop, maxSecondsToNextDrop);
        }

        private float PickNewPosition()
        {
            return Random.Range(leftEdgeXPosition, rightEdgeXPosition);
        }
    }
}