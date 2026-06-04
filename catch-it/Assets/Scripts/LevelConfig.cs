using System;
using UnityEngine;

[Serializable]
public class LevelConfig
{
    public string LevelId;
    public string DisplayName;

    [TextArea]
    public string Description;

    [Header("Spider Difficulty Settings")]
    public EnvironmentKind EnvironmentKind = EnvironmentKind.Inside;
    public SpiderVisualKind SpiderVisualKind = SpiderVisualKind.Cartoon;
    public SpiderSizeKind SpiderSizeKind = SpiderSizeKind.Small;
    public SpiderMovementKind SpiderMovementKind = SpiderMovementKind.Static;

    [Min(1)]
    public int SpidersToCatch = 3;

    [Min(1)]
    public int MaxActiveSpiders = 1;

    [Header("Panic Mode Settings")]
    public PanicModeBehavior PanicModeBehavior = PanicModeBehavior.HideSpiders;

    public string GetSummary()
    {
        return $"{DisplayName}: Your task is to find and catch <b><u>{SpidersToCatch}</u></b>{(SpiderSizeKind == SpiderSizeKind.Small ? "" : " a little larger")} {SpiderVisualKind.ToString().ToLower()} spiders. {MaxActiveSpiders} will appear at a time. They are in an {(EnvironmentKind == EnvironmentKind.Inside ? "indoor" : "open")} environment and have {SpiderMovementKind.ToString().ToLower()} movement.";
    }
}

public enum EnvironmentKind
{
    Outside,
    Inside
}

public enum SpiderVisualKind
{
    None,
    Fantasy,
    Cartoon,
    Realistic,
    Scary
}

public enum SpiderSizeKind
{
    Small,
    Large
}
public enum SpiderMovementKind
{
    Static,
    Idle,
    Walking
}
public enum PanicModeBehavior
{
    HideSpiders,
    TransformSpidersHarmlessly // instead of hiding, change the spiders to a non-threatening visual
}

[Serializable]
public class SpiderPrefabInfo
{
    public GameObject Prefab;
    public SpiderVisualKind VisualKind;

    public SpiderPrefabInfo(GameObject prefab, SpiderVisualKind visualKind)
    {
        Prefab = prefab;
        VisualKind = visualKind;
    }
}