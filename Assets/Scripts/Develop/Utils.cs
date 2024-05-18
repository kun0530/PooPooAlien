using System.Collections.Generic;
using System.Text;

public static class Utils
{
    public static int WeightedRandomPick(List<float> weights)
    {
        float totalWeight = 0f;
        foreach (var weight in weights)
        {
            totalWeight += weight;
        }

        float pick = UnityEngine.Random.Range(0f, 1f);
        for (int i = 0; i < weights.Count; i++)
        {
            if (weights[i] / totalWeight >= pick)
            {
                return i;
            }
            pick -= weights[i] / totalWeight;
        }

        return -1;
    }

    public static T WeightedRandomPick<T>(List<(T, float)> weights)
    {
        float totalWeight = 0f;
        foreach (var weight in weights)
        {
            totalWeight += weight.Item2;
        }

        float pick = UnityEngine.Random.Range(0f, 1f);
        for (int i = 0; i < weights.Count; i++)
        {
            if (weights[i].Item2 / totalWeight >= pick)
            {
                return weights[i].Item1;
            }
            pick -= weights[i].Item2 / totalWeight;
        }

        return weights[weights.Count - 1].Item1;
    }

    public static string NumberToString(float value)
    {
        string[] unitSymbol = new string[] { "", "만", "억", "조", "경", "해" };

        if( value == 0 ) { return "0"; }
            
        int unitID = 0;
            
        string number = string.Format("{0:# #### #### #### #### ####}", value).TrimStart();
        string[] splits = number.Split(' ');
 
        StringBuilder sb = new StringBuilder();
 
        for (int i = splits.Length; i > 0; i--)
        {
            int digits = 0;
            if (int.TryParse(splits[i - 1], out digits))
            {
                // 앞자리가 0이 아닐때
                if (digits != 0)
                {
                    sb.Insert(0, $"{ digits}{ unitSymbol[unitID] }");
                }
            }
            else
            {
                // 마이너스나 숫자외 문자
                sb.Insert(0, $"{ splits[i - 1] }");
            }
            unitID++;
        }
        return sb.ToString();            
    }
}
