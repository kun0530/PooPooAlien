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
}