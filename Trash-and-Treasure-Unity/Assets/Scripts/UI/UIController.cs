using Gameplay;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class UIController : MonoBehaviour
    {
        [Header("Menu Panels")] [SerializeField]
        private UIPanel[] menuPanels;

        [SerializeField] private UIPanel defaultPanel;
        [SerializeField] private UIPanel pausePanel;

        private UIPanel _currentPanel;

        [Header("Unity Scenes")] [SerializeField]
        private SceneReference nextScene;

        [SerializeField] private SceneReference previousScene;

        private void Awake()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            GameEvents.OnPauseToggled += HandlePause;
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            GameEvents.OnPauseToggled -= HandlePause;
        }

        public void PauseGame(bool shouldPause)
        {
            GameEvents.OnPauseToggled?.Invoke(shouldPause);
        }

        private void HandlePause(bool isPaused)
        {
            UpdateUI(isPaused ? pausePanel : defaultPanel);
        }

        private void OnSceneLoaded(Scene _, LoadSceneMode __)
        {
            UpdateUI(defaultPanel);
        }

        public void NextScene()
        {
            if (nextScene == null)
            {
                Debug.LogError("Next scene reference is not set.");
                return;
            }
            LoadSceneByReference(nextScene);
        }

        public void PreviousScene()
        {
            if (previousScene == null)
            {
                Debug.LogError("Previous scene reference is not set.");
                return;
            }

            LoadSceneByReference(previousScene);
        }

        public void LoadSceneByName(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        public void ReloadCurrentScene()
        {
            // Delegate to the Scenes Manager
            ScenesManager.ReloadCurrentScene();
        }

        private static void LoadSceneByReference(SceneReference sceneRef)
        {
            // Delegate to the Scenes Manager
            ScenesManager.LoadSceneByReference(sceneRef);

        }

        private void UpdateUI(UIPanel panel)
        {
            HideAll();
            if (menuPanels == null || menuPanels.Length == 0)
            {
                return;
            }

            ShowPanel(panel);
        }

        public void ShowPanel(UIPanel panel)
        {
            if (!panel || panel == _currentPanel)
            {
                return;
            }

            HideAll();
            panel.SetActive(true);
            _currentPanel = panel;
        }

        private void HideAll()
        {
            if (menuPanels == null)
            {
                return;
            }

            foreach (var panel in menuPanels)
            {
                if (panel)
                {
                    panel.SetActive(false);
                }
            }
        }
    }
}