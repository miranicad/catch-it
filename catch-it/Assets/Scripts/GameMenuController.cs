using UnityEngine;

public class GameMenuController : MonoBehaviour
{
    [SerializeField] private GameObject introPanel;
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject customLevelPanel;
    [SerializeField] private GameObject levelCompletePanel;
    [SerializeField] private GameObject endPanel;
    [SerializeField] private GameObject panicPanel;

    private void Start()
    {
        ShowIntro();
    }

    public void ShowIntro()
    {
        ShowOnly(introPanel);
    }

    public void ShowMainMenu()
    {
        ShowOnly(mainMenuPanel);
    }

    public void ShowCustomLevelMenu()
    {
        ShowOnly(customLevelPanel);
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

    private void ShowOnly(GameObject panel)
    {
        HideAll();

        if (panel != null)
        {
            panel.SetActive(true);
        }
    }
}