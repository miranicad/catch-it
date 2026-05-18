using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
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
    [SerializeField] private List<LevelConfig> predefinedLevels = new();

    [Header("References")]
    [SerializeField] private GameMenuController menuController;

    [SerializeField] private OVRScreenFade screenFade;
    public Image fadeImage; // full-screen image for fading

    [Header("Scene References")]
    [SerializeField] private DynamicSpiderSpawner spiderSpawner;
    [SerializeField] private Transform playerInsideSpawnPoint;
    [SerializeField] private Transform xrRigRoot;
    [SerializeField] private Transform centerEyeAnchor;

    private readonly List<LevelConfig> activeLevels = new();

    private int currentLevelIndex = 0;
    private int caughtSpiders = 0;

    public LevelConfig CurrentLevel
    {
        get
        {
            if (activeLevels.Count == 0)
            {
                return null;
            }

            return activeLevels[currentLevelIndex];
        }
    }

    private void Start()
    {
        // Do not start gameplay immediately.
        // Show intro/menu UI instead.
        Debug.Log("Waiting for player to start from menu.");
    }

    public void StartPredefinedLevels()
    {
        Debug.Log("METHOD CALLED FOR StartPredefinedLevels.");

        activeLevels.Clear();
        activeLevels.AddRange(predefinedLevels);

        if (activeLevels.Count == 0)
        {
            Debug.LogError("No predefined levels configured.");
            return;
        }

        StartLevel(0);
    }

    public void StartCustomLevel(LevelConfig customConfig)
    {
        activeLevels.Clear();
        activeLevels.Add(customConfig);

        StartLevel(0);
    }

    public void StartNextLevel()
    {
        int nextLevelIndex = currentLevelIndex + 1;
        if (CurrentLevel != null && nextLevelIndex < activeLevels.Count)
        {
            Debug.Log("Starting next level.");
            StartLevel(nextLevelIndex);
        }
        else
        {
            Debug.LogError("No current level to advance from.");
        }
    }

    public void RegisterSpiderCaught(GameObject spider)
    {
        if (CurrentLevel == null)
        {
            Debug.LogWarning("No active level.");
            return;
        }

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

    public void ActivatePanicMode()
    {
        // todo Later: show calm UI, pause panel, return-to-menu button, etc.
        if (CurrentLevel == null)
        {
            return;
        }

        menuController.ShowPanicPanel();

        Debug.Log("Panic mode activated");

        spiderSpawner.ClearSpiders();

        switch (CurrentLevel.PanicModeBehavior)
        {
            case PanicModeBehavior.HideSpiders:
                // Show calm/pause UI.
                break;

            case PanicModeBehavior.ReturnToMenu:
                // Show menu UI.
                break;
        }
    }

    private void CompleteCurrentLevel()
    {
        // todo Later: show final screen / experiment finished UI.
        Debug.Log($"Level complete: {CurrentLevel.DisplayName}");

        spiderSpawner.ClearSpiders();

        int nextLevelIndex = currentLevelIndex + 1;

        if (nextLevelIndex < activeLevels.Count)
        {
            menuController.ShowLevelComplete();
            Debug.Log("Waiting for player to decide what's next.");
            // StartLevel(nextLevelIndex);
        }
        else
        {
            Debug.Log("All active levels complete!");

            // Show end screen / completion UI here.
            menuController.ShowEndScreen();
        }
    }

    IEnumerator LoadLevelRoutine(int levelIndex)
    {
        yield return StartCoroutine(Fade(1f, 0.4f));

        spiderSpawner.ClearSpiders();

        // Initialize level
        if (levelIndex < 0 || levelIndex >= activeLevels.Count)
        {
            Debug.LogError($"Invalid level index: {levelIndex}");
            yield break;
        }

        currentLevelIndex = levelIndex;
        caughtSpiders = 0;

        LevelConfig config = CurrentLevel;

        // spawn/move player to spawn point in the loaded scene
        yield return null; // let Unity finish activation
        var isFirstLevelOfEnvironmentKind = activeLevels.FindIndex(l => l.EnvironmentKind == config.EnvironmentKind) == levelIndex;
        if (isFirstLevelOfEnvironmentKind && config.EnvironmentKind == EnvironmentKind.Inside)
        {
            // For the first level, if it's an inside environment, spawn the player at the inside spawn point.
            // For later levels, we can consider more complex transitions (e.g., fade out/in, moving the player, etc.)
            if (playerInsideSpawnPoint != null)
            {
                PositionPlayerAtSpawn(playerInsideSpawnPoint);
            }
            else
            {
                Debug.LogWarning("Player inside spawn point is not assigned.");
            }
        }
        else if (isFirstLevelOfEnvironmentKind && config.EnvironmentKind == EnvironmentKind.Outside)
        {
            PositionPlayerAtSpawn(null);
            Debug.LogWarning("Player outside spawn point is not assigned."); // todo if have outside level
        }

        Debug.Log($"Starting level: {config.DisplayName}");

        spiderSpawner.SpawnSpiders(config);
        menuController.HideAll(); // handle ui display / showing and hiding relevant menus, panels, etc.

        yield return StartCoroutine(Fade(0f, 0.4f));
    }


    private void LoadLevel(int levelIndex)
    {
        spiderSpawner.ClearSpiders();

        // Initialize level
        if (levelIndex < 0 || levelIndex >= activeLevels.Count)
        {
            Debug.LogError($"Invalid level index: {levelIndex}");
            return;
        }

        currentLevelIndex = levelIndex;
        caughtSpiders = 0;

        LevelConfig config = CurrentLevel;

        // spawn/move player to spawn point in the loaded scene
        var isFirstLevelOfEnvironmentKind = activeLevels.FindIndex(l => l.EnvironmentKind == config.EnvironmentKind) == levelIndex;
        if (isFirstLevelOfEnvironmentKind && config.EnvironmentKind == EnvironmentKind.Inside)
        {
            // For the first level, if it's an inside environment, spawn the player at the inside spawn point.
            // For later levels, we can consider more complex transitions (e.g., fade out/in, moving the player, etc.)
            if (playerInsideSpawnPoint != null)
            {
                PositionPlayerAtSpawn(playerInsideSpawnPoint);
            }
            else
            {
                Debug.LogWarning("Player inside spawn point is not assigned.");
            }
        }
        else if (isFirstLevelOfEnvironmentKind && config.EnvironmentKind == EnvironmentKind.Outside)
        {
            PositionPlayerAtSpawn(null);
            Debug.LogWarning("Player outside spawn point is not assigned."); // todo if have outside level
        }

        Debug.Log($"Starting level: {config.DisplayName}");

        spiderSpawner.SpawnSpiders(config);
        menuController.HideAll(); // handle ui display / showing and hiding relevant menus, panels, etc.
    }


    private void StartLevel(int levelIndex)
    {
        StartCoroutine(FadeToBlackAndBack(() => LoadLevel(levelIndex)));
    }

    private void PositionPlayerAtSpawn(Transform spawnPoint)
    {
        if (xrRigRoot == null || centerEyeAnchor == null || spawnPoint == null)
        {
            Debug.LogError("Missing XR rig root, center eye anchor, or spawn point.");
            return;
        }

        // First rotate the rig to match the spawn yaw.
        float currentHeadYaw = centerEyeAnchor.eulerAngles.y;
        float targetYaw = spawnPoint.eulerAngles.y;
        float yawDifference = targetYaw - currentHeadYaw;

        xrRigRoot.RotateAround(centerEyeAnchor.position, Vector3.up, yawDifference);

        // Then move the rig so the headset ends up at the spawn position.
        Vector3 positionOffset = spawnPoint.position - centerEyeAnchor.position;
        xrRigRoot.position += positionOffset;

        Debug.Log($"XR player repositioned to spawn: {spawnPoint.name}");
    }

    private IEnumerator FadeToBlackAndBack(System.Action middleAction = null)
    {
        if (screenFade == null)
        {
            Debug.LogWarning("No OVRScreenFade assigned.");
            middleAction?.Invoke();
            yield break;
        }

        screenFade.FadeOut();

        yield return new WaitForSeconds(1.0f);

        middleAction?.Invoke();

        yield return new WaitForSeconds(0.3f);

        screenFade.FadeIn();
    }

    private IEnumerator Fade(float targetAlpha, float duration)
    {
        if (fadeImage == null) yield break;
        float start = fadeImage.color.a;
        float t = 0f;
        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            float a = Mathf.Lerp(start, targetAlpha, t / duration);
            var c = fadeImage.color; c.a = a; fadeImage.color = c;
            yield return null;
        }
    }
}
