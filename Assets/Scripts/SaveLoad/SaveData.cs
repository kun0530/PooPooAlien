using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SaveData
{
    public int Version { get; protected set; }

    public abstract SaveData VersionUp();
}

public class SaveDataV1 : SaveData
{
    private float gold = 0;
    public float Gold {
        get { return gold; }
        set
        {
            if (value >= 100000f)
                value = 99999f;
            if (value < 0f)
                value = 0f;
            gold = value;
        }
    }
    private Dictionary<PlayerStat, int> enhanceStatData;
    public Dictionary<PlayerStat, int> EnhanceStatData{
        get
        {
            if (enhanceStatData == null)
            {
                enhanceStatData = new Dictionary<PlayerStat, int>();
                for (int i = 0; i < (int)PlayerStat.Count; i++)
                {
                    enhanceStatData.Add((PlayerStat)i, 1);
                }
            }
            return enhanceStatData;
        }
    }

    private Dictionary<int, int> stageClearData;
    public Dictionary<int, int> StageClearData{
        get
        {
            if (stageClearData == null)
            {
                stageClearData = new Dictionary<int, int>();
                for (int i = 0; i <= 10; i++)
                {
                    stageClearData.Add(i, -1);
                }
            }
            return stageClearData;
        }
    }

    private Languages currentLang = Variables.defaultLang;
    public Languages CurrentLang{
        get
        {
            if ((int)currentLang <= (int)Languages.None || (int)currentLang >= (int)Languages.Count)
            {
                currentLang = Variables.defaultLang;
            }
            return currentLang;
        }
        set
        {
            if ((int)value <= (int)Languages.None || (int)value >= (int)Languages.Count)
                return;

            currentLang = value;
        }
    }

    public SaveDataV1()
    {
        Version = 1;
    }

    public override SaveData VersionUp()
    {
        return null;
    }
}