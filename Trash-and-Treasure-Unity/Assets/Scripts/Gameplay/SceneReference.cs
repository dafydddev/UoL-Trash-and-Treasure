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
    }
}