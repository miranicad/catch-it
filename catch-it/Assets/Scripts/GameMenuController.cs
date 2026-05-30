using UnityEngine;

public class GameMenuController : MonoBehaviour
{
    [Header("Canvas Panel References")]
    [SerializeField] private GameObject introPanel;
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject customLevelPanel;
    [SerializeField] private GameObject levelCompletePanel;
    [SerializeField] private GameObject endPanel;
    [SerializeField] private GameObject panicPanel;

    [Header("Canvas Placement References")]
    [SerializeField] private GameMenuRepositionCanvas repositionCanvas;

    private void Start()
    {
        repositionCanvas = FindFirstObjectByType<GameMenuRepositionCanvas>();
        ShowIntro();
    }

    public void ShowIntro()
    {
        ShowOnly(introPanel, placeInFrontOfPlayer: true);
    }

    public void ShowMainMenu()
    {
        ShowOnly(mainMenuPanel);
    }

    public void ShowCustomLevelMenu()
    {
        ShowOnly(customLevelPanel, placeInFrontOfPlayer: false);
    }

    public void ShowLevelComplete()
    {
        ShowOnly(levelCompletePanel);
    }

    public void ShowEndScreen()
    {
        ShowOnly(endPanel);
    }

    public void ShowPanicPanel()
    {
        ShowOnly(panicPanel);
    }

    public void HideAll()
    {
        introPanel.SetActive(false);
        mainMenuPanel.SetActive(false);
        customLevelPanel.SetActive(false);
        levelCompletePanel.SetActive(false);
        endPanel.SetActive(false);
        panicPanel.SetActive(false);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game.");
        Application.Quit();
    }

    private void ShowOnly(GameObject panel, bool placeInFrontOfPlayer = true)
    {
        HideAll();

        Debug.Log($"Showing panel: {panel.name}");
        if (panel != null)
        {
            panel.SetActive(true);
        }

        if (placeInFrontOfPlayer)
        {
            repositionCanvas.PlaceInFrontOfPlayer();
        }
    }
}