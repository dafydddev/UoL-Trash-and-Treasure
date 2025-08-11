using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gameplay
{
    public enum ItemType
    {
        Apple,
        Banana,
        Carrot
    }

    public class Item : MonoBehaviour
    {
        [SerializeField] private ItemType type;
        [SerializeField] private int scoreValue;
        [SerializeField] private float shrinkSpeed = 0.1f;
        private bool _isShrinking;

        public ItemType GetItemType() => type;
        public int GetValue() => scoreValue;

        public void Update()
        {
            if (_isShrinking)
            {
                ShrinkGameObject();
            }
        }

        public void SwitchOff()
        {
            _isShrinking = true;
        }

        private void ShrinkGameObject()
        {
            if (gameObject.transform.localScale.x > 0.1f)
            {
                gameObject.transform.localScale -=
                    new Vector3(shrinkSpeed * Time.deltaTime, shrinkSpeed * Time.deltaTime, 0);
            }
            else
            {
                _isShrinking = false;
                Destroy(gameObject);
            }
        }
    }
}