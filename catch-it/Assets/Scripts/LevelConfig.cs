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

    [Header("Spawn Configuration")]
    public Transform spawnPointContainer; // optional, will be null if custom level created from menu

    [Header("Panic Mode Settings")]
    public PanicModeBehavior PanicModeBehavior = PanicModeBehavior.HideSpiders;
}

public enum EnvironmentKind
{
    Outside,
    Inside
}

public enum SpiderVisualKind
{
    None,
    Cartoon, // todo! idea: make it funky colors as well instead of black? because black is already pretttty realtic tbh
    Realistic
}

public enum SpiderSizeKind
{
    Small,
    Large
}
public enum SpiderMovementKind
{
    Static,
    Slow
}
public enum PanicModeBehavior
{
    HideSpiders,
    ReturnToMenu
}