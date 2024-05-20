using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class DevelopTestManager : MonoBehaviour
{
    public Camera mainCamera;
    public RectTransform gameUi;
    public RectTransform developTestUi;
    public float UiBoundary;

    private Dictionary<PlayerStat, int> enhanceStatData;
    public DevelopPlayerStat statInputField;
    public Transform playerStatContent;

    private void Start()
    {
        mainCamera.rect = new Rect(0, 0, UiBoundary, 1f);

        gameUi.anchorMin = new Vector2(0f, 0f);
        gameUi.anchorMax = new Vector2(UiBoundary, 1f);
        
        developTestUi.anchorMin = new Vector2(UiBoundary, 0f);
        developTestUi.anchorMax = new Vector2(1f, 1f);

        enhanceStatData = Variables.SaveData.EnhanceStatData;
        foreach (var statData in enhanceStatData)
        {
            var playerStat = Instantiate(statInputField);
            playerStat.transform.SetParent(playerStatContent, false);
            playerStat.PlayerStatData = statData;
        }
    }
}
