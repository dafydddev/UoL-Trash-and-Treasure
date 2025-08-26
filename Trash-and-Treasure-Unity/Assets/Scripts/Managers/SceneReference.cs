using UnityEditor;
using UnityEngine;

namespace Managers
{
    [System.Serializable]
    public class SceneReference : ISerializationCallbackReceiver
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
#endif

        // Gets the name of the scene to load
        public string SceneName => sceneName;

        public void OnBeforeSerialize()
        {
#if UNITY_EDITOR
            // Only sync in the editor and not during play mode
            if (!Application.isPlaying && sceneAsset != null)
            {
                sceneName = sceneAsset.name;
            }
#endif
        }

        public void OnAfterDeserialize()
        {
            // Nothing needed here
        }
    }
}