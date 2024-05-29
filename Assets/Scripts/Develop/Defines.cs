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

public static class StringTableIds
{
    public static readonly string Stage_Start_Text = "Stage_Start_Text";
    public static readonly string Stage_Enhance_Text = "Stage_Enhance_Text";

    public static readonly string Setting_Title_Text = "Setting_Title_Text";
    public static readonly string Setting_Lang_Text = "Setting_Lang_Text";
    public static readonly string Setting_BGM_Text = "Setting_BGM_Text";
    public static readonly string Setting_VFX_Text = "Setting_VFX_Text";
    public static readonly string Setting_BackToTitle_Text = "Setting_BackToTitle_Text";
    public static readonly string Setting_Quit_Text = "Setting_Quit_Text";

    public static readonly string Enhance_Title_Text = "Enhance_Title_Text";
    public static readonly string Enhance_Choice_Text = "Enhance_Choice_Text";
    public static readonly string Enhance_Possible_Text = "Enhance_Possible_Text";

    public static readonly string Pause_Title_Text = "Pause_Title_Text";
    public static readonly string Pause_BackToMain_Text = "Pause_BackToMain_Text";
    public static readonly string Pause_Restart_Text = "Pause_Restart_Text";
    public static readonly string Pause_Continue_Text = "Pause_Continue_Text";

    public static readonly string Clear_Score_Text = "Clear_Score_Text";
    public static readonly string Clear_Score_Timeleft_Text = "Clear_Score_Timeleft_Text";
    public static readonly string Clear_Hitcount_Text = "Clear_Hitcount_Text";
    public static readonly string Clear_Goldearn_Text = "Clear_Goldearn_Text";
    public static readonly string Clear_Confirm_Text = "Clear_Confirm_Text";

    public static readonly string Gameover_BackToMain_Text = "Gameover_BackToMain_Text";
    public static readonly string Gameover_Retry_Text = "Gameover_Retry_Text";
}

public static class ResourcesPath
{
    public static readonly string[] HelpUiResouces =
    {
        "StringTableKr",
        "StringTableEn"
    };
}