using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelConfigMenuController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameManager gameManager;

    [Header("Spawn Containers")]
    [SerializeField] private Transform insideSpawnPointContainer;
    [SerializeField] private Transform outsideSpawnPointContainer;

    [Header("Environment UI")]
    [SerializeField] private TMP_Dropdown environmentDropdown;

    [Header("Spider Visual UI")]
    [SerializeField] private TMP_Dropdown spiderVisualDropdown;

    [Header("Spider Size UI")]
    [SerializeField] private TMP_Dropdown spiderSizeDropdown;

    [Header("Spider Movement UI")]
    [SerializeField] private TMP_Dropdown spiderMovementDropdown;

    [Header("Count UI")]
    [SerializeField] private Slider spidersToCatchSlider;
    [SerializeField] private TMP_Text spidersToCatchValueText;

    [SerializeField] private Slider maxActiveSpidersSlider;
    [SerializeField] private TMP_Text maxActiveSpidersValueText;

    [Header("Panic Mode UI")]
    [SerializeField] private TMP_Dropdown panicBehaviorDropdown;

    private void Start()
    {
        RefreshValueLabels();
    }

    public void OnSpidersToCatchChanged(float value)
    {
        RefreshValueLabels();
    }

    public void OnMaxActiveSpidersChanged(float value)
    {
        RefreshValueLabels();
    }

    private void RefreshValueLabels()
    {
        if (spidersToCatchValueText != null)
        {
            spidersToCatchValueText.text = Mathf.RoundToInt(spidersToCatchSlider.value).ToString();
        }

        if (maxActiveSpidersValueText != null)
        {
            maxActiveSpidersValueText.text = Mathf.RoundToInt(maxActiveSpidersSlider.value).ToString();
        }
    }

    public void StartCustomLevelFromMenu()
    {
        LevelConfig config = BuildLevelConfigFromUi();

        gameManager.StartCustomLevel(config);
    }

    private LevelConfig BuildLevelConfigFromUi()
    {
        EnvironmentKind environmentKind = (EnvironmentKind)environmentDropdown.value;
        SpiderVisualKind visualKind = (SpiderVisualKind)spiderVisualDropdown.value;
        SpiderSizeKind sizeKind = (SpiderSizeKind)spiderSizeDropdown.value;
        SpiderMovementKind movementKind = (SpiderMovementKind)spiderMovementDropdown.value;
        PanicModeBehavior panicBehavior = (PanicModeBehavior)panicBehaviorDropdown.value;

        int spidersToCatch = Mathf.RoundToInt(spidersToCatchSlider.value);
        int maxActiveSpiders = Mathf.RoundToInt(maxActiveSpidersSlider.value);

        maxActiveSpiders = Mathf.Clamp(maxActiveSpiders, 1, spidersToCatch);

        Transform spawnContainer = GetSpawnContainer(environmentKind);

        return new LevelConfig
        {
            LevelId = "custom_runtime_level",
            DisplayName = "Custom Level",
            Description = "Custom level created from menu inputs.",

            EnvironmentKind = environmentKind,
            SpiderVisualKind = visualKind,
            SpiderSizeKind = sizeKind,
            SpiderMovementKind = movementKind,

            SpidersToCatch = spidersToCatch,
            MaxActiveSpiders = maxActiveSpiders,

            spawnPointContainer = spawnContainer,

            PanicModeBehavior = panicBehavior
        };
    }

    private Transform GetSpawnContainer(EnvironmentKind environmentKind)
    {
        switch (environmentKind)
        {
            case EnvironmentKind.Outside:
                return outsideSpawnPointContainer;

            case EnvironmentKind.Inside:
            default:
                return insideSpawnPointContainer;
        }
    }
}