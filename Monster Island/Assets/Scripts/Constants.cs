using UnityEngine;

public static class PlayerScale
{
    public static readonly Vector3 Left = Vector3.one;
    public static readonly Vector3 Right = new Vector3(-1, 1, 1);
}

public static class MonsterAnim
{
    public static readonly string awake = $"{nameof(awake)}";
    public static readonly string explode = $"{nameof(explode)}";
    public static readonly string sleep = $"{nameof(sleep)}";
}

public static class PlayerAnim
{
    public static readonly string idle = $"{nameof(idle)}";
    public static readonly string move = $"{nameof(move)}";
    public static readonly string hit = $"{nameof(hit)}";
    public static readonly string victory = $"{nameof(victory)}";
    public static readonly string item_collect = $"{nameof(item_collect)}";
}

public static class SkinsNames
{
    public static readonly string @default = $"{nameof(@default)}";
}

public static class SceneNames
{
    public static readonly string Island_one = nameof(Island_one);
    public static readonly string Island_two = nameof(Island_two);
    public static readonly string MainMenu = nameof(MainMenu);
}