using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    [Header("Menu Panels")]
    [SerializeField] private GameObject[] menuPanels;
    [SerializeField] private GameObject defaultPanel;
    private GameObject currentPanel;

    [Header("Unity Scenes")]
    [SerializeField] private SceneReference mainMenuScene;
    [SerializeField] private SceneReference gameplayScene;

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
        UpdateUI();
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

    public void LoadMainMenu()
    {
        LoadScene(mainMenuScene);
    }

    public void LoadGameplay()
    {
        LoadScene(gameplayScene);
    }

    private void UpdateUI()
    {
        HideAll();
        if (menuPanels == null || menuPanels.Length == 0)
        {
            Debug.LogWarning("No menu panels registered.");
            return;
        }
        ShowPanel(defaultPanel);
    }

    public void ShowPanel(GameObject panel)
    {
        if (panel == null || panel == currentPanel)
        {
            return;
        }
        HideAll();
        panel.SetActive(true);
        currentPanel = panel;
    }

    private void HideAll()
    {
        SetAllPanels(menuPanels, false);
        currentPanel = null;
    }

    private static void SetAllPanels(GameObject[] panels, bool active)
    {
        if (panels == null)
        {
            return;
        }
        foreach (var panel in panels)
            {
                if (panel != null)
                {
                    panel.SetActive(active);
                }
            }
    }
}
