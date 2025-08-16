using UnityEngine;

namespace Gameplay
{
    public class CraneDropper : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private float minTimeToNextDrop;
        [SerializeField] private float maxTimeToNextDrop;
        [SerializeField] private GameObject[] itemsToSpawn;
        [SerializeField] private float itemSpawnPositionOffset = 0.5f;
        [SerializeField] private float leftEdgeXPosition = -200;
        [SerializeField] private float rightEdgeXPosition = 200;
        [SerializeField] private float topHeight = 2.6f;
        [SerializeField] private float bottomHeight = 1.5f;

        [SerializeField] private Sprite craneOpenSprite;
        [SerializeField] private Sprite craneClosedSprite;

        [SerializeField] private SpriteRenderer fakeItemSpriteRenderer;
        [SerializeField] private SpriteRenderer caneSpriteRenderer;
        private Vector3 _fakeItemTransform;

        private SpriteRenderer _spriteRenderer;
        private SpriteRenderer _caneSpriteRenderer;
        
        [SerializeField] private Collider2D groundCollider;
        [SerializeField] private Collider2D deathCollider;

        private void Start()
        {
            _spriteRenderer = fakeItemSpriteRenderer.GetComponent<SpriteRenderer>();
            _caneSpriteRenderer = caneSpriteRenderer.GetComponent<SpriteRenderer>();
            _caneSpriteRenderer.sprite = craneClosedSprite;
        }

        public void DropItem()
        {
            ToggleCraneSprite();
            SpawnItem();
        }

        private void ToggleCraneSprite()
        {
            _caneSpriteRenderer.sprite =
                _caneSpriteRenderer.sprite == craneOpenSprite ? craneClosedSprite : craneOpenSprite;
        }

        private void SpawnItem()
        {
            _spriteRenderer.enabled = false;
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
    }
}