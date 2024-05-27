using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Languages
{
    None = -1,
    Korean,
    English,
    Count
}

public static class DataTableIds
{
    public static readonly string[] String =
    {
        "StringTableKr",
        "StringTableEn"
    };

    public static string CurrString
    {
        get
        {
            return String[(int)Variables.SaveData.CurrentLang];
        }
    }

    public static readonly string Stage = "StageTable";
    public static readonly string Monster = "MonsterTable";
    public static readonly string MonsterSpawn = "MonsterSpawnTable";
    public static readonly string Item = "ItemTable";
    public static readonly string ItemDrop = "ItemDropTable";
    public static readonly string Enhance = "EnhanceTable";
    public static readonly string Projectile = "ProjectileTable";

}

public static class SceneIds
{
    public static readonly string Title = "Title";
    public static readonly string Develop = "Develop";
}

public static class FrequentlyUsed
{
    public static readonly string Default = "Default";
}