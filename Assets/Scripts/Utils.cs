using System.Collections.Generic;

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
}
