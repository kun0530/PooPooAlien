using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiGameClear : MonoBehaviour
{
    public GameManager gameManager { get; set; }

    public Button pauseButton;

    public TextMeshProUGUI clearScoreText;
    public Image clearScoreCheckImage;

    public TextMeshProUGUI clearHitCountText;
    public Image clearHitCountCheckImage;

    public TextMeshProUGUI clearTimeText;
    public Image clearTimeCheckImage;

    public TextMeshProUGUI clearEarnedGoldText;

    private int clearStarCount;
    public List<Image> clearStars;

    private void Awake()
    {
        clearStarCount = 0;
    }

    private void OnEnable()
    {
        if (gameManager != null)
        {
            pauseButton.enabled = false;

            var stageData = DataTableManager.Get<StageTable>(DataTableIds.Stage).Get(gameManager.StageId);
            if (gameManager.CurrentScore >= stageData.StageScoreget)
            {
                clearStarCount++;
                clearScoreCheckImage.enabled = true;
                Logger.Log("Star Check");
            }
            if (gameManager.CurrentGameTimer >= stageData.StageTimerleft)
            {
                clearStarCount++;
                clearTimeCheckImage.enabled = true;
                Logger.Log("Time Check");
            }
            if (gameManager.playerHitCount <= stageData.StageHitcount)
            {
                clearStarCount++;
                clearHitCountCheckImage.enabled = true;
                Logger.Log("Hit Check");
            }

            clearStars[clearStarCount].enabled = true;
            Logger.Log($"스타: {clearStarCount}");

            clearScoreText.text = $"{gameManager.CurrentScore}";
            clearHitCountText.text = $"{gameManager.playerHitCount}";
            clearTimeText.text = $"{(int)gameManager.CurrentGameTimer}";
            clearEarnedGoldText.text = $"{gameManager.EarnedGold}";

            Variables.SaveData.Gold += gameManager.EarnedGold + stageData.ClearGold;
            var prevStarCount = Variables.SaveData.StageClearData[gameManager.StageId];
            Variables.SaveData.StageClearData[gameManager.StageId] = prevStarCount < clearStarCount ? clearStarCount : prevStarCount;
            SaveLoadSystem.Save(Variables.SaveData);
        }
    }

    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        foreach (var star in clearStars)
        {
            star.enabled = false;
        }
        clearScoreCheckImage.enabled = false;
        clearTimeCheckImage.enabled = false;
        clearHitCountCheckImage.enabled = false;

        gameObject.SetActive(false);
    }
}
