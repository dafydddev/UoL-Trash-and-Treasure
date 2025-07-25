using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    [Header("Menu Panels")]
    [SerializeField] private UIPanel[] _menuPanels;
    [SerializeField] private UIPanel _defaultPanel;
    private UIPanel _currentPanel;

    [Header("Unity Scenes")]
    [SerializeField] private SceneReference _nextScene;
    [SerializeField] private SceneReference _previousScene;

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
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
            Debug.LogWarning("No menu panels registered.");
            return;
        }
        ShowPanel(panel);
    }

    public void ShowPanel(UIPanel panel)
    {
        if (panel == null || panel == _currentPanel)
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
                if (panel != null)
                {
                    panel.SetActive(false);
                }
            }
    }
}
