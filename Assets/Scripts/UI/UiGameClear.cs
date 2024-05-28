using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiGameClear : MonoBehaviour
{
    private GameManager gameManager;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI clearScoreText;
    public Image clearScoreCheckImage;

    public TextMeshProUGUI hitCountText;
    public TextMeshProUGUI clearHitCountText;
    public Image clearHitCountCheckImage;

    public TextMeshProUGUI timeText;
    public TextMeshProUGUI clearTimeText;
    public Image clearTimeCheckImage;

    public TextMeshProUGUI earnedGoldText;
    public TextMeshProUGUI clearEarnedGoldText;

    public Button confirmButton;

    private int clearStarCount;
    public List<Image> clearStars;

    private void Awake()
    {
        clearStarCount = 0;
    }

    private void OnEnable()
    {
        if (gameManager == null)
            return;

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

    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        var stringTable = DataTableManager.GetStringTable();
        scoreText.text = stringTable.Get(StringTableIds.Clear_Score_Text);
        hitCountText.text = stringTable.Get(StringTableIds.Clear_Hitcount_Text);
        timeText.text = stringTable.Get(StringTableIds.Clear_Score_Timeleft_Text);
        earnedGoldText.text = stringTable.Get(StringTableIds.Clear_Goldearn_Text);
        var confirmText = confirmButton.GetComponentInChildren<TextMeshProUGUI>();
        if (confirmText != null)
            confirmText.text = stringTable.Get(StringTableIds.Clear_Confirm_Text);
        confirmButton.onClick.AddListener(gameManager.GoToTitle);

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
