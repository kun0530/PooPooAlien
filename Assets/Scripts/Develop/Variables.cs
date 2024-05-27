// 씬 전환할 때, 데이터를 넘겨주는 용도
public static class Variables
{
    public static readonly string Version = "1.0.0";
    public static Languages defaultLang = Languages.Korean;

    public static bool isStartGame = true;
    public static int stageId = 1;
    private static SaveDataV1 saveData;
    public static SaveDataV1 SaveData
    {
        get {
            if (saveData == null)
            {
                saveData = SaveLoadSystem.Load() as SaveDataV1;
                if (saveData == null)
                {
                    saveData = new SaveDataV1();
                }
            }
            return saveData;
        }
    }

    public static float CalculateCurrentSaveStat(PlayerStat type)
    {
        var level = SaveData.EnhanceStatData[type];
        return CalculateStat(type, level);
    }

    public static float CalculateStat(PlayerStat type, int level)
    {
        if (level <= 0)
            return 0f;

        var data = DataTableManager.Get<EnhanceTable>(DataTableIds.Enhance).Get(type);

        if (level >= data.MaxLevel)
            return data.BasicStat + data.StatIncrease * (data.MaxLevel - 1);

        return data.BasicStat + data.StatIncrease * (level - 1);
    }
}