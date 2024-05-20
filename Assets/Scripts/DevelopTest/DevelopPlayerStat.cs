using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DevelopPlayerStat : MonoBehaviour
{
    public TextMeshProUGUI statNameText;

    private KeyValuePair<PlayerStat, int> playerStatData;
    public KeyValuePair<PlayerStat, int> PlayerStatData {
        get { return playerStatData; }
        set{
            playerStatData = value;
            if (playerStatData.Key != PlayerStat.Count)
            {
                statNameText.text = playerStatData.Key.ToString();
            }
        }
    }
}
