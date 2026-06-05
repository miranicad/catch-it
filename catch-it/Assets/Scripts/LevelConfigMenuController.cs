using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelConfigMenuController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameManager gameManager;

    [Header("Count UI")]
    [SerializeField] private Slider spidersToCatchSlider;
    [SerializeField] private TMP_Text spidersToCatchValueText;

    [Header("Selection Summary")]
    [SerializeField] private TMP_Text selectionSummaryText;

    static LevelConfig defaultConfig = new LevelConfig
    {
        EnvironmentKind = EnvironmentKind.Inside,
        SpiderVisualKind = SpiderVisualKind.Cartoon,
        SpiderSizeKind = SpiderSizeKind.Small,
        SpiderMovementKind = SpiderMovementKind.Static,
        SpidersToCatch = 2,
        MaxActiveSpiders = 1,
        PanicModeBehavior = PanicModeBehavior.HideSpiders
    };

    LevelConfig currentConfig = defaultConfig.Clone();
    static LevelConfig lastSelectedConfig;

    void OnEnable()
    {
        currentConfig = lastSelectedConfig?.Clone() ?? defaultConfig.Clone();
        spidersToCatchSlider.value = currentConfig.SpidersToCatch;
        UpdateSelectionSummary();
    }

    public void OnSelectSpiderKind(int spiderVisualKindValue)
    {
        SpiderVisualKind spiderVisualKind = (SpiderVisualKind)spiderVisualKindValue;
        currentConfig.SpiderVisualKind = spiderVisualKind;
        currentConfig.SpiderMovementKind = spiderVisualKind == SpiderVisualKind.Cartoon ? SpiderMovementKind.Static : SpiderMovementKind.Idle;
        UpdateSelectionSummary();
    }

    public void OnMakeSpidersLargerChanged(bool isOn)
    {
        currentConfig.SpiderSizeKind = isOn ? SpiderSizeKind.Large : SpiderSizeKind.Small;
        UpdateSelectionSummary();
    }

    public void OnMaxOneActiveSpiderChanged(bool isOn)
    {
        currentConfig.MaxActiveSpiders = isOn ? 1 : currentConfig.SpidersToCatch;
        UpdateSelectionSummary();
    }

    public void OnSpidersToCatchChanged(float value)
    {
        int intValue = (int)value;

        currentConfig.MaxActiveSpiders = Mathf.Min(currentConfig.MaxActiveSpiders, intValue); // if was larger than new value, reduce to match new value
        currentConfig.SpidersToCatch = intValue;

        spidersToCatchValueText.text = intValue.ToString();

        UpdateSelectionSummary();
    }

    public void StartCustomLevelFromMenu()
    {
        LevelConfig config = BuildLevelConfigFromSelected();

        Debug.Log($"Starting custom level with config: {config.DisplayName}, SpidersToCatch: {config.SpidersToCatch}, VisualKind: {config.SpiderVisualKind}, MovementKind: {config.SpiderMovementKind}");

        lastSelectedConfig = config.Clone();
        gameManager.StartCustomLevel(config);
    }

    private void UpdateSelectionSummary()
    {
        selectionSummaryText.text = $"Selected Configuration:\n {currentConfig.SpidersToCatch} spiders, as {currentConfig.SpiderVisualKind.ToString().ToLower()} kind, of {currentConfig.SpiderSizeKind.ToString().ToLower()} size.";
    }

    private LevelConfig BuildLevelConfigFromSelected()
    {
        return new LevelConfig
        {
            LevelId = "custom_runtime_level",
            DisplayName = "Custom Level - " + currentConfig.SpiderVisualKind.ToString() + (currentConfig.SpiderSizeKind == SpiderSizeKind.Large ? " Large" : "") + " " + currentConfig.EnvironmentKind.ToString(),
            Description = "Custom level created from menu inputs.",

            EnvironmentKind = EnvironmentKind.Inside,
            SpiderVisualKind = currentConfig.SpiderVisualKind,
            SpiderSizeKind = currentConfig.SpiderSizeKind,
            SpiderMovementKind = currentConfig.SpiderMovementKind,

            SpidersToCatch = currentConfig.SpidersToCatch,
            MaxActiveSpiders = currentConfig.MaxActiveSpiders,

            PanicModeBehavior = PanicModeBehavior.HideSpiders
        };
    }

    [Obsolete("Method used in InitialVersion of Configuration; keep for now.")]
    public void OnSelectCartoonSpiderKind()
    {
        currentConfig.SpiderVisualKind = SpiderVisualKind.Fantasy;
        currentConfig.SpiderMovementKind = SpiderMovementKind.Idle;
        UpdateSelectionSummary();
    }

    [Obsolete("Method used in InitialVersion of Configuration; keep for now.")]
    public void OnSelectMixedSpiderKind()
    {
        currentConfig.SpiderVisualKind = SpiderVisualKind.Cartoon;
        currentConfig.SpiderMovementKind = SpiderMovementKind.Static;
        UpdateSelectionSummary();
    }

    [Obsolete("Method used in InitialVersion of Configuration; keep for now.")]
    public void OnSelectRealisticSpiderKind()
    {
        currentConfig.SpiderVisualKind = SpiderVisualKind.Realistic;
        currentConfig.SpiderMovementKind = SpiderMovementKind.Idle;
        UpdateSelectionSummary();
    }

    [Obsolete("Method used in InitialVersion of Configuration; keep for now.")]
    public void OnSpidersToCatchChanged()
    {
        var value = (int)spidersToCatchSlider.value;
        spidersToCatchValueText.text = value.ToString();

        currentConfig.SpidersToCatch = value;
        currentConfig.MaxActiveSpiders = 1;

        UpdateSelectionSummary();
    }
}