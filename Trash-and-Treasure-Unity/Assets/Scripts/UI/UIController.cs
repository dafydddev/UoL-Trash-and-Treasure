using Gameplay;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class UIController : MonoBehaviour
    {
        [Header("Menu Panels")] [SerializeField]
        private UIPanel[] _menuPanels;

        [SerializeField] 
        private UIPanel _defaultPanel;
        [SerializeField] 
        private UIPanel _pausePanel;

        private UIPanel _currentPanel;
        
        [Header("Unity Scenes")] [SerializeField]
        private SceneReference _nextScene;

        [SerializeField] 
        private SceneReference _previousScene;
        
        private bool _isPaused = false;

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
            UpdateUI(isPaused ? _pausePanel : _defaultPanel);
        }

        private void OnSceneLoaded(Scene _, LoadSceneMode __)
        {
            UpdateUI(_defaultPanel);
        }

        public void NextScene()
        {
            if (_nextScene == null)
            {
                Debug.LogError("Next scene reference is not set.");
                return;
            }

            LoadScene(_nextScene);
        }

        public void PreviousScene()
        {
            if (_previousScene == null)
            {
                Debug.LogError("Previous scene reference is not set.");
                return;
            }

            LoadScene(_previousScene);
        }

        private void LoadScene(SceneReference sceneRef)
        {
            if (sceneRef == null || string.IsNullOrEmpty(sceneRef.SceneName))
            {
                Debug.LogError("SceneReference is null or missing name.");
                return;
            }

            SceneManager.LoadScene(sceneRef.SceneName);
        }

        private void UpdateUI(UIPanel panel)
        {
            HideAll();
            if (_menuPanels == null || _menuPanels.Length == 0)
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
            if (_menuPanels == null)
            {
                return;
            }

            foreach (var panel in _menuPanels)
            {
                if (panel)
                {
                    panel.SetActive(false);
                }
            }
        }
    }
}