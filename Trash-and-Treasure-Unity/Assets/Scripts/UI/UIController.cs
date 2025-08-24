using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class UIController : MonoBehaviour
    {
        // The menu panels know to the scene
        [SerializeField] private UIPanel[] menuPanels;

        // The default panel to show on scene load
        [SerializeField] private UIPanel defaultPanel;

        // The panel to show when the game is paused
        [SerializeField] private UIPanel pausePanel;

        // The current panel being displayed
        private UIPanel _currentPanel;

        // The next scene (e.g. Level 1 -> Level 2) for when UI wants the next scene
        [SerializeField] private SceneReference nextScene;

        // The previous scene (e.g. Level 1 -> Main Menu) for when UI wants the previous scene
        [SerializeField] private SceneReference previousScene;

        private void Awake()
        {
            // Subscribe to the SceneManagement sceneLoaded event
            SceneManager.sceneLoaded += OnSceneLoaded;
            // Subscribe to the Game Events pause event
            GameEvents.OnPauseToggled += HandlePause;
            // Load the default panel
            UpdateUI(defaultPanel);
        }

        private void OnDestroy()
        {
            // Unsubscribe to the SceneManagement sceneLoaded event
            SceneManager.sceneLoaded -= OnSceneLoaded;
            // Unsubscribe to the Game Events pause event
            GameEvents.OnPauseToggled -= HandlePause;
        }

        public void PauseGame(bool shouldPause)
        {
            // Invoke the pause event (logic in the Game Manager)
            GameEvents.OnPauseToggled?.Invoke(shouldPause);
        }

        private void HandlePause(bool isPaused)
        {
            // When the game is paused, show the pause panel, otherwise show the default one
            UpdateUI(isPaused ? pausePanel : defaultPanel);
        }

        private void OnSceneLoaded(Scene _, LoadSceneMode __)
        {
            // When a new scene is loaded, make sure the default panel is shown
            UpdateUI(defaultPanel);
        }

        public void NextScene()
        {
            // Log an error if we have attempted to load the next screen without setting it
            if (nextScene == null)
            {
                Debug.LogError("Next scene reference is not set.");
                return;
            }

            // Load the scene using the nextScene variable
            LoadSceneByReference(nextScene);
        }

        public void PreviousScene()
        {
            // Log an error if we have attempted to load the previous screen without setting it
            if (previousScene == null)
            {
                Debug.LogError("Previous scene reference is not set.");
                return;
            }

            // Load the scene using the previous variable
            LoadSceneByReference(previousScene);
        }

        public void ReloadCurrentScene()
        {
            // Reload whatever the current scene is
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            // TODO consider scene manager
        }

        private static void LoadSceneByReference(SceneReference sceneRef)
        {
            // Log an error if we have attempted to load a scene without a valid reference
            if (sceneRef == null || string.IsNullOrEmpty(sceneRef.SceneName))
            {
                Debug.LogError("SceneReference is null or missing name.");
                return;
            }

            // Delegate to the SceneManager to load the scene
            SceneManager.LoadScene(sceneRef.SceneName);
        }

        private void UpdateUI(UIPanel panel)
        {
            // Early exit when there are no menu panels
            if (menuPanels == null || menuPanels.Length == 0) return;
            // Hide all the menu panels know to the script
            ShowPanel(panel);
        }

        public void ShowPanel(UIPanel panel)
        {
            // Early exit when there is no panel, or it is already the current panel
            if (!panel || panel == _currentPanel) return;
            // Hide all the other panels
            HideAll();
            // Set the panel to be active
            panel.SetActive(true);
            // Set the panel as the active one
            _currentPanel = panel;
        }

        private void HideAll()
        {
            // Early exit when there are no menu panels
            if (menuPanels == null || menuPanels.Length == 0) return;
            // Loop through the panels and set them all to inactive
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