using UnityEngine;

<<<<<<< Updated upstream:Trash-and-Treasure-Unity/Assets/Scripts/UI/SceneReference.cs
#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class SceneReference
=======
namespace Managers
>>>>>>> Stashed changes:Trash-and-Treasure-Unity/Assets/Scripts/Managers/SceneReference.cs
{
#if UNITY_EDITOR
    [SerializeField] private SceneAsset sceneAsset;
#endif

    [SerializeField] private string sceneName;

#if UNITY_EDITOR
    public SceneAsset SceneAsset
    {
        get
        {
            return sceneAsset;
        }
    }

    public void OnValidate()
    {
        if (sceneAsset != null)
        {
            sceneName = sceneAsset.name;
        }
    }
#endif

    public string SceneName
    {
        get
        {
            return sceneName;
        }
    }
}