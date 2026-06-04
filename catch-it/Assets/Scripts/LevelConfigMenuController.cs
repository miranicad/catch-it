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

    private string selectedSpiderKindOption = "";

    public void OnSelectSpiderKind(int spiderVisualKindValue)
    {
        SpiderVisualKind spiderVisualKind = (SpiderVisualKind)spiderVisualKindValue;
        defaultConfig.SpiderVisualKind = spiderVisualKind;
        defaultConfig.SpiderMovementKind = spiderVisualKind == SpiderVisualKind.Cartoon ? SpiderMovementKind.Static : SpiderMovementKind.Idle;
        selectedSpiderKindOption = spiderVisualKind.ToString();
        UpdateSelectionSummary();
    }

    public void OnMakeSpidersLargerChanged(bool isOn)
    {
        defaultConfig.SpiderSizeKind = isOn ? SpiderSizeKind.Large : SpiderSizeKind.Small;
        UpdateSelectionSummary();
    }

    public void OnMaxOneActiveSpiderChanged(bool isOn)
    {
        defaultConfig.MaxActiveSpiders = isOn ? 1 : defaultConfig.SpidersToCatch;
        UpdateSelectionSummary();
    }

    public void OnSpidersToCatchChanged(float value)
    {
        int intValue = (int)value;

        defaultConfig.MaxActiveSpiders = Mathf.Min(defaultConfig.MaxActiveSpiders, intValue); // if was larger than new value, reduce to match new value
        defaultConfig.SpidersToCatch = intValue;

        UpdateSelectionSummary();
    }

    public void StartCustomLevelFromMenu()
    {
        LevelConfig config = BuildLevelConfigFromSelected();

        Debug.Log($"Starting custom level with config: {config.DisplayName}, SpidersToCatch: {config.SpidersToCatch}, VisualKind: {config.SpiderVisualKind}, MovementKind: {config.SpiderMovementKind}");

        gameManager.StartCustomLevel(config);
    }

    private void OnChangeNumberOfSpidersToCatch(int value)
    {
        defaultConfig.SpidersToCatch = value;
        defaultConfig.MaxActiveSpiders = 1; // Mathf.Min(defaultConfig.MaxActiveSpiders, value);
    }

    private void UpdateSelectionSummary()
    {
        selectionSummaryText.text = $"Selected Configuration:\n {defaultConfig.SpidersToCatch} spiders, as {selectedSpiderKindOption} kind";
    }

    private LevelConfig BuildLevelConfigFromSelected()
    {
        return new LevelConfig
        {
            LevelId = "custom_runtime_level",
            DisplayName = "Custom Level - " + selectedSpiderKindOption,
            Description = "Custom level created from menu inputs.",

            EnvironmentKind = EnvironmentKind.Inside,
            SpiderVisualKind = defaultConfig.SpiderVisualKind,
            SpiderSizeKind = defaultConfig.SpiderSizeKind,
            SpiderMovementKind = defaultConfig.SpiderMovementKind,

            SpidersToCatch = defaultConfig.SpidersToCatch,
            MaxActiveSpiders = defaultConfig.MaxActiveSpiders,

            PanicModeBehavior = PanicModeBehavior.HideSpiders
        };
    }

    [Obsolete("Method used in InitialVersion of Configuration; keep for now.")]
    public void OnSelectCartoonSpiderKind()
    {
        defaultConfig.SpiderVisualKind = SpiderVisualKind.Fantasy;
        defaultConfig.SpiderMovementKind = SpiderMovementKind.Idle;
        selectedSpiderKindOption = "Cartoon";
        UpdateSelectionSummary();
    }

    [Obsolete("Method used in InitialVersion of Configuration; keep for now.")]
    public void OnSelectMixedSpiderKind()
    {
        defaultConfig.SpiderVisualKind = SpiderVisualKind.Cartoon;
        defaultConfig.SpiderMovementKind = SpiderMovementKind.Static;
        selectedSpiderKindOption = "Mixed";
        UpdateSelectionSummary();
    }

    [Obsolete("Method used in InitialVersion of Configuration; keep for now.")]
    public void OnSelectRealisticSpiderKind()
    {
        defaultConfig.SpiderVisualKind = SpiderVisualKind.Realistic;
        defaultConfig.SpiderMovementKind = SpiderMovementKind.Idle;
        selectedSpiderKindOption = "Realistic";
        UpdateSelectionSummary();
    }

    [Obsolete("Method used in InitialVersion of Configuration; keep for now.")]
    public void OnSpidersToCatchChanged()
    {
        var value = (int)spidersToCatchSlider.value;
        spidersToCatchValueText.text = value.ToString();
        OnChangeNumberOfSpidersToCatch(value);
        UpdateSelectionSummary();
    }
}