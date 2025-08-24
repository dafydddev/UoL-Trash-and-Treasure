<<<<<<< Updated upstream:Trash-and-Treasure-Unity/Assets/Scripts/Gameplay/SceneReference.cs
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
=======
using UnityEditor;
using UnityEngine;

namespace Managers
{
    [System.Serializable]
    public class SceneReference
    {
#if UNITY_EDITOR
        // Reference to the scene asset in the editor
        [SerializeField] private SceneAsset sceneAsset;
#endif
        // The name of the scene to load
        [SerializeField] private string sceneName;

#if UNITY_EDITOR
        // Gets the scene asset reference for editor use
        public SceneAsset SceneAsset => sceneAsset;

        public void OnValidate()
        {
            // Sync the scene name from the scene asset
            if (sceneAsset != null)
            {
                sceneName = sceneAsset.name;
            }
        }
#endif
        // Gets the name of the scene to load
        public string SceneName => sceneName;
>>>>>>> Stashed changes:Trash-and-Treasure-Unity/Assets/Scripts/Managers/SceneReference.cs
    }
}