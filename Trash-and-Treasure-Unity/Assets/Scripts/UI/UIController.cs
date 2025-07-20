using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public enum UIState
    {
        MainMenu,
        Gameplay
    }

    [Header("State")]
    [SerializeField] private UIState currentState;

    [Header("Main Menu Panels")]
    [SerializeField] private GameObject[] mainMenuPanels;
    [SerializeField] private GameObject mainMenuDefault;

    [Header("Gameplay Panels")]
    [SerializeField] private GameObject[] gameplayPanels;
    [SerializeField] private GameObject gameplayDefault;

    private GameObject currentPanel;
    private static UIController instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    private void OnSceneLoaded(Scene _, LoadSceneMode __)
    {
        UpdateUI();
    }

    public void SetState(UIState newState)
    {
        if (currentState == newState)
            return;

        currentState = newState;
        currentPanel = null;
        UpdateUI();
    }

    private void UpdateUI()
    {
        HideAll();

        GameObject[] set = GetCurrentSet();
        GameObject defaultPanel = GetDefaultForCurrentState();

        if (defaultPanel != null)
        {
            ShowPanel(defaultPanel);
        }
    }

    public void ShowPanel(GameObject panel)
    {
        if (panel == null || panel == currentPanel)
            return;

        HideAll();

        panel.SetActive(true);
        currentPanel = panel;
    }

    private void HideAll()
    {
        SetActive(mainMenuPanels, false);
        SetActive(gameplayPanels, false);
        currentPanel = null;
    }

    private GameObject[] GetCurrentSet()
    {
        return currentState == UIState.MainMenu ? mainMenuPanels : gameplayPanels;
    }

    private GameObject GetDefaultForCurrentState()
    {
        return currentState == UIState.MainMenu ? mainMenuDefault : gameplayDefault;
    }

    private static void SetActive(GameObject[] panels, bool active)
    {
        if (panels == null) return;
        foreach (var panel in panels)
        {
            if (panel != null)
                panel.SetActive(active);
        }
    }
}
