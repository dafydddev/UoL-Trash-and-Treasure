
using UnityEngine;
using UnityEngine.Serialization;

namespace Gameplay
{
    public enum WallType
    {
        Edge,
        Bottom
    }

    public class Walls : MonoBehaviour
    { 
        [FormerlySerializedAs("_wallType")] [SerializeField]
        private WallType wallType;
        public WallType WallType => wallType;
    }
}