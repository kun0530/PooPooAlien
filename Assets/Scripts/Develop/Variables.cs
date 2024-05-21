// 씬 전환할 때, 데이터를 넘겨주는 용도
public static class Variables
{
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

    public static float CalculateSaveStat(PlayerStat type)
    {
        var level = Variables.SaveData.EnhanceStatData[type];
        var data = DataTableManager.Get<EnhanceTable>(DataTableIds.Enhance).Get(type);
        return data.BasicStat + data.StatIncrease * (level - 1);
    }
}