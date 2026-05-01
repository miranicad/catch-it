using System.Collections.Generic;
using UnityEngine;

public class LevelProgressionManager : MonoBehaviour
{
    // Ideas for Minimum implementation that still looks good:
    // Main menu: “Start”
    // Level 1 starts in inside environment
    // User catches 3 cartoon/small spiders
    // Completion panel: “Level complete — Continue”
    // Level 2 starts in same environment
    // User catches 5 realistic/small spiders
    // Panic button is demonstrated
    // Optional Level 3 if stable
    // End screen: “Thank you / Experiment complete”


    [Header("Level Configuration")]
    [SerializeField] private List<LevelConfig> levels = new();

    [Header("Scene References")]
    [SerializeField] private DynamicSpiderSpawner spiderSpawner;

    private int currentLevelIndex = 0;
    private int caughtSpiders = 0;

    public LevelConfig CurrentLevel => levels[currentLevelIndex];

    private void Start()
    {
        StartLevel(0);
    }

    public void StartLevel(int levelIndex)
    {
        if (levelIndex < 0 || levelIndex >= levels.Count)
        {
            Debug.LogError($"Invalid level index: {levelIndex}");
            return;
        }

        currentLevelIndex = levelIndex;
        caughtSpiders = 0;

        LevelConfig config = CurrentLevel;

        Debug.Log($"Starting level: {config.DisplayName}");

        spiderSpawner.ClearSpiders();
        spiderSpawner.SpawnSpiders(config);
    }

    public void RegisterSpiderCaught(GameObject spider)
    {
        caughtSpiders++;

        Debug.Log($"Spider caught: {caughtSpiders}/{CurrentLevel.SpidersToCatch}");

        Destroy(spider);

        if (caughtSpiders >= CurrentLevel.SpidersToCatch)
        {
            CompleteCurrentLevel();
        }
        else
        {
            spiderSpawner.SpawnSingleSpider(CurrentLevel);
        }
    }

    public int GetCurrentScore()
    {
        return caughtSpiders;
    }

    private void CompleteCurrentLevel()
    {
        Debug.Log($"Level complete: {CurrentLevel.DisplayName}");

        spiderSpawner.ClearSpiders();

        int nextLevelIndex = currentLevelIndex + 1;

        if (nextLevelIndex < levels.Count)
        {
            StartLevel(nextLevelIndex);
        }
        else
        {
            Debug.Log("All levels complete!");
            // todo Later: show final screen / experiment finished UI.
        }
    }

    public void ActivatePanicMode()
    {
        if (!CurrentLevel.PanicModeEnabled)
        {
            return;
        }

        Debug.Log("Panic mode activated");

        spiderSpawner.ClearSpiders();

        // todo Later: show calm UI, pause panel, return-to-menu button, etc.
    }
}
