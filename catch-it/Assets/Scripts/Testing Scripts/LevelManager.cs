using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public GameObject transitionPanel;
    public int totalSpiders = 5;

    void Update()
    {
        // Alle Spinnen gesammelt?
        if (SpiderCollect.score >= totalSpiders)
        {
            transitionPanel.SetActive(true);
            Time.timeScale = 0; // Spiel pausieren
        }
    }

    public void LoadNextLevel()
    {
        Time.timeScale = 1;
        SpiderCollect.score = 0; // Score zurücksetzen
        SceneManager.LoadScene("Level2");
    }
}