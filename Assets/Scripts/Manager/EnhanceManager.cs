using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnhanceManager : MonoBehaviour
{
    public TitleUiManager uiManager;

    private EnhanceTable enhanceTable;

    public TextMeshProUGUI goldText;
    private string goldTextFormat = "{0}K";
    
    public Button exitButton;

    public ScrollRect scrollRect;
    public GameObject content;
    public EnhanceStatPanel enhanceStatPanelPrefab;
    private Dictionary<PlayerStat, EnhanceStatPanel> enhanceStats = new Dictionary<PlayerStat, EnhanceStatPanel>();

    public Image selectedStatIcon; 
    public TextMeshProUGUI selectedStatNameText;
    public TextMeshProUGUI selectedStatLevelText;
    private string levelFormat = "{0} / {1}";
    public TextMeshProUGUI selectedStatDescText;
    public TextMeshProUGUI selectedStatIncreaseText;
    private string increaseFormat = "{0} ▶ {1}";
    public TextMeshProUGUI selectedStatRequiredGoldText;

    private EnhanceData selectedEnhanceData;
    public EnhanceData SelectedEnhanceData{
        get{ return selectedEnhanceData; }
        set{
            if (value == null)
            {
                selectedEnhanceData = null;
                selectedStatIcon.enabled = false;
                selectedStatNameText.enabled = false;
                selectedStatLevelText.enabled = false;
                selectedStatDescText.enabled = false;
                selectedStatIncreaseText.enabled = false;
                selectedStatRequiredGoldText.enabled = false;
                return;
            }

            selectedEnhanceData = value;
            selectedStatIcon.enabled = true;
            selectedStatIcon.sprite = selectedEnhanceData.GetIcon();

            var currentLevel = Variables.SaveData.EnhanceStatData[(PlayerStat)SelectedEnhanceData.Stat];
            var maxLevel = selectedEnhanceData.MaxLevel;
            var currentStat = Variables.CalculateCurrentSaveStat((PlayerStat)SelectedEnhanceData.Stat);
            var nextStat = Variables.CalculateStat((PlayerStat)SelectedEnhanceData.Stat, currentLevel + 1);
            float requiredGold = SelectedEnhanceData.RequiredGold + SelectedEnhanceData.RequiredGoldIncrease * (currentLevel - 1);

            selectedStatNameText.enabled = true;
            selectedStatNameText.text = selectedEnhanceData.Name;
            selectedStatLevelText.enabled = true;
            selectedStatLevelText.text = string.Format(levelFormat, currentLevel, maxLevel);
            selectedStatDescText.enabled = true;
            selectedStatDescText.text = selectedEnhanceData.Desc;
            selectedStatIncreaseText.enabled = true;
            selectedStatRequiredGoldText.enabled = true;
            if (currentLevel >= selectedEnhanceData.MaxLevel)
            {
                selectedStatIncreaseText.text = $"{currentStat}";
                selectedStatRequiredGoldText.text = "강화 완료";
            }
            else
            {
                selectedStatIncreaseText.text = string.Format(increaseFormat, currentStat, nextStat);
                selectedStatRequiredGoldText.text = string.Format(goldTextFormat, requiredGold);
            }
        }
    }

    private void Awake()
    {
        enhanceTable = DataTableManager.Get<EnhanceTable>(DataTableIds.Enhance);
        for (int i = 0; i < (int)PlayerStat.Count; i++)
        {
            var panel = Instantiate(enhanceStatPanelPrefab);
            panel.transform.SetParent(content.transform, false);
            panel.Data = enhanceTable.Get((PlayerStat)i);

            var button = panel.GetComponent<Button>();
            button.onClick.AddListener( () => {
                SelectedEnhanceData = panel.Data;
            });

            enhanceStats.Add((PlayerStat)i, panel);
        }
    }

    private void OnEnable()
    {
        scrollRect.verticalNormalizedPosition = 1f;
        SelectedEnhanceData = null;
    }

    private void Start()
    {
        var buttonImage = exitButton.GetComponent<Image>();
        if (buttonImage != null)
            buttonImage.alphaHitTestMinimumThreshold = 0.1f;

        exitButton.onClick.AddListener(() => {
            uiManager.Status = UiStatus.StageSelect;
        });

        UpdateGoldText();
    }

    public void UpdateGoldText()
    {
        goldText.text = string.Format(goldTextFormat, Utils.NumberToString(Variables.SaveData.Gold));
    }

    public void EnhanceStat()
    {
        if (SelectedEnhanceData == null)
            return;
        
        int currentLevel = Variables.SaveData.EnhanceStatData[(PlayerStat)SelectedEnhanceData.Stat];
        float requiredGold = SelectedEnhanceData.RequiredGold + SelectedEnhanceData.RequiredGoldIncrease * (currentLevel - 1);

        Logger.Log($"현재 레벨: {currentLevel}");
        Logger.Log($"필요 금액: {Utils.NumberToString(requiredGold)}");

        if (currentLevel >= SelectedEnhanceData.MaxLevel)
        {
            Logger.Log("최대 레벨");
            return;
        }

        if (Variables.SaveData.Gold < requiredGold)
        {
            Logger.Log("돈 없음");
            return;
        }

        Variables.SaveData.Gold -= requiredGold;
        currentLevel = ++Variables.SaveData.EnhanceStatData[(PlayerStat)SelectedEnhanceData.Stat];
        Logger.Log($"강화: {currentLevel}");
        Logger.Log($"남은 금액: {Utils.NumberToString(Variables.SaveData.Gold)}");
        UpdateGoldText();

        foreach (var enhanceStat in enhanceStats)
        {
            if (enhanceStat.Value != null)
                enhanceStat.Value.Data = enhanceStat.Value.Data;
        }

        SaveLoadSystem.Save(Variables.SaveData);
    }
}
